using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Entities;
using AirPro.Entities.Billing;
using AirPro.Library.Models.Concrete;
using AirPro.Logging;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using UniMatrix.Common.Extensions;

namespace AirPro.Library
{
    public class PaymentsLibrary : BaseLibrary
    {
        private ServiceFactory Factory { get; }

        public PaymentsLibrary(EntityDbContext context, IIdentity user) : base(context, user)
        {
            Factory = new ServiceFactory(context, user);
        }

        public async Task<AcceptPaymentViewModel> GetAcceptPaymentViewModel(int? invoiceId, int currencyId)
        {
            // Load Payment Type Select List.
            var paymentTypes = Db.PaymentTypes
                .Where(t => t.PaymentTypeActiveInd)
                .OrderBy(t => t.PaymentTypeSortOrder)
                .Select(t => new SelectListItem
                {
                    Value = t.PaymentTypeId.ToString(),
                    Text = t.PaymentTypeName
                }).ToList();

            paymentTypes.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = @"<-- Select Payment Type -->"
            });

            // Load Shop ID from Invoice ID.
            var shopGuid = Db.RepairOrders.FirstOrDefault(o => invoiceId.HasValue && o.OrderId == invoiceId.Value)?.ShopGuid;

            // Load Shop Select List.
            var shops = Factory.GetDisplayList<IShopDto>().Select(s => new SelectListItem
            {
                Value = s.Key,
                Text = s.Value
            }).ToList();

            // Check Shop ID.
            IEnumerable<OutstandingInvoiceItemViewModel> invoices = new List<OutstandingInvoiceItemViewModel>();
            if (Db.Shops.Any(s => s.ShopGuid == shopGuid))
            {
                // Load Unpaid Invoices.
                invoices = await GetOutstandingInvoiceItemViewModels(currencyId, shopGuid);

                // Update Selected Invoice.
                invoices.Where(i => invoiceId.HasValue && i.InvoiceId == invoiceId.Value).ToList().ForEach(i => { i.Selected = true; } );
            }
            else
            {
                // Add Default Selection.
                shops.Insert(0, new SelectListItem
                {
                    Value = "0",
                    Text = @"<-- Select Shop -->"
                });
            }

            // Create Payment.
            var payment = new AcceptPaymentViewModel
            {
                PaymentReceivedFromShopGuid = shopGuid ?? Guid.Empty,
                PaymentTypeSelectListItems = paymentTypes.OrderBy(t => t.Text),
                PaymentReceivedFromShopSelectListItems = shops,
                ShopOutstandingInvoiceItems = invoices
            };

            return payment;
        }

        public async Task<IEnumerable<OutstandingInvoiceItemViewModel>> GetOutstandingInvoiceItemViewModels(int currencyId, Guid? shopGuid = null)
        {
            var shopParam = shopGuid == null
                ? new SqlParameter("@ShopGuid", DBNull.Value)
                : new SqlParameter("@ShopGuid", shopGuid);

            var currencyParam = currencyId <= 0 
                ? new SqlParameter("@CurrencyId", DBNull.Value) 
                : new SqlParameter("@CurrencyId", currencyId);
            
            var sqlParams = new[]
            {
                shopParam,
                currencyParam,
                new SqlParameter("@Search", DBNull.Value), 
            };

            var unpaidInvoices =
                await
                    Db.Database.SqlQuery<OutstandingInvoiceItemViewModel>(
                        "Billing.usp_GetOutstandingInvoices @ShopGuid, @CurrencyId, @Search", sqlParams).ToListAsync();

            // Localize Invoiced Date.
            unpaidInvoices.ForEach(e => e.InvoiceDateTime = e.InvoiceDateTime.ConvertToUserTime(User.TimeZoneInfoId));

            // Localize Repair Date.
            unpaidInvoices.ForEach(e => e.RepairCreatedDateTime = e.RepairCreatedDateTime.ConvertToUserTime(User.TimeZoneInfoId));

            return unpaidInvoices;
        }

        public async Task<int?> SaveAcceptPaymentViewModel(AcceptPaymentViewModel payment)
        {
            // Validate Payment Information.
            if (payment.PaymentReceivedFromShopGuid == Guid.Empty)
                throw new ArgumentNullException("Shop", "Payment Shop must be selected to save payment.");

            if (payment.PaymentTypeID == 0)
                throw new ArgumentNullException("Type", "Payment Type must be selected to save payment.");

            if (payment.ShopInvoicesSelected == null || !payment.ShopInvoicesSelected.Any())
                throw new ArgumentNullException("Invoices", "Invoices must be selected to save payment.");

            // Load Outstanding Invoices.
            var outstandingInvoices = (await GetOutstandingInvoiceItemViewModels(payment.CurrencyId, payment.PaymentReceivedFromShopGuid)).ToList();

            // Calculate Balance & Discount.
            decimal discountAmount = 0;
            decimal outstandingBalances = 0;
            foreach (var invoiceId in payment.ShopInvoicesSelected)
            {
                var amount = outstandingInvoices.Where(i => i.InvoiceId == invoiceId).Select(i => i.InvoiceBalanceAmount).FirstOrDefault();
                outstandingBalances += amount;
                discountAmount += Math.Round(amount * ((decimal) payment.PaymentDiscountPercentage / 100), 2,
                    MidpointRounding.AwayFromZero);
            }

            // Validate Balance Collection.
            if (payment.PaymentAmount > outstandingBalances - discountAmount)
                throw new InvalidOperationException("Payment Amount can NOT exceed applied amount to Invoices. (Payment > Invoice Total)");
            
            // Create Payment Information.
            var newPayment = new PaymentEntityModel
            {
                PaymentReceivedFromShop = Db.Shops.Find(payment.PaymentReceivedFromShopGuid),
                PaymentAmount = payment.PaymentAmount,
                DiscountPercentage = payment.PaymentDiscountPercentage,
                CurrencyId = payment.CurrencyId,
                CreatedBy = User,
                CreatedDt = DateTimeOffset.UtcNow,
                PaymentType = Db.PaymentTypes.Find(payment.PaymentTypeID),
                PaymentReferenceNumber = payment.PaymentReferenceNumber,
                PaymentMemo = payment.PaymentMemo,
                PaymentDt = payment.PaymentDate
            };

            // Save Payment.
            Db.Payments.Add(newPayment);

            // Create Payment Transactions.
            var availableBalance = payment.PaymentAmount;
            var newPaymentTrans = new List<PaymentTransactionEntityModel>();
            foreach (var invoice in outstandingInvoices.Where(i => payment.ShopInvoicesSelected.Contains(i.InvoiceId)).ToList())
            {
                // Check Balance.
                if (availableBalance < 0) continue;

                // Calculate Payment.
                var discount = Math.Round(invoice.InvoiceBalanceAmount * ((decimal)payment.PaymentDiscountPercentage / 100), 2, MidpointRounding.AwayFromZero);
                var remaining = invoice.InvoiceBalanceAmount - discount;
                var applied = availableBalance > remaining ? remaining : availableBalance;

                // Create New Transaction.
                var tran = new PaymentTransactionEntityModel
                {
                    Invoice = Db.RepairInvoices.Find(invoice.InvoiceId),
                    DiscountAmountApplied = discount,
                    InvoiceAmountApplied = applied,
                    Payment = newPayment,
                    CreatedBy = User,
                    CreatedDt = DateTimeOffset.UtcNow
                };

                // Update Order if Balance Satisfied.
                if (invoice.InvoiceBalanceAmount == (tran.InvoiceAmountApplied + tran.DiscountAmountApplied))
                    tran.Invoice.Repair.Status = RepairStatuses.Paid;

                // Update Available Balance.
                availableBalance = availableBalance - tran.InvoiceAmountApplied;

                // Add Transaction.
                newPaymentTrans.Add(tran);
            }

            // Save Transactions.
            Db.PaymentTransactions.AddRange(newPaymentTrans);

            // Update.
            await Db.SaveChangesAsync();

            return newPayment.PaymentId;
        }

        public async Task<bool> VoidPayment(int paymentId)
        {
            try
            {
                // Void Payment Transactions.
                var transactions = Db.PaymentTransactions.Where(pt => pt.PaymentId == paymentId).ToList();
                transactions.ForEach(t => t.PaymentTransactionVoidInd = true);
                transactions.ForEach(t => t.PaymentTransactionVoidByUser = User);
                transactions.ForEach(t => t.PaymentTransactionVoidDt = DateTimeOffset.UtcNow);
                transactions.ForEach(t => t.Invoice.Repair.Status = RepairStatuses.Invoiced);

                // Void Payment.
                var payment = Db.Payments.Where(p => p.PaymentId == paymentId).ToList();
                payment.ForEach(p => p.PaymentVoidInd = true);
                payment.ForEach(p => p.PaymentVoidByUser = User);
                payment.ForEach(p => p.PaymentVoidDt = DateTimeOffset.UtcNow);

                // Update Database.
                await Db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                return false;
            }

            return true;
        }
    }
}
