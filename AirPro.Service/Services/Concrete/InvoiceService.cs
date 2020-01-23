using AirPro.Common.Enumerations;
using AirPro.Entities.Repair;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace AirPro.Service.Services.Concrete
{
    internal class InvoiceService : ServiceBase, IService<IInvoiceDto>
    {
        public InvoiceService(IServiceSettings settings) : base(settings)
        {
        }

        public IInvoiceDto GetById(string id)
        {
            // Load Invoice.
            InvoiceDto invoice;
            var workItems = new List<InvoiceWorkItemDto>();
            var invoiceDictionary = new Dictionary<int, InvoiceDto>();
            using (var reader = Conn.QueryMultiple("Billing.usp_GetBillingInvoice @UserGuid, @RepairId",
                new { User.UserGuid, RepairId = id}))
            {
                invoice = reader.Read<InvoiceDto, InvoiceLineItemDto, InvoiceDto>((p, c) =>
                    {
                        if (!invoiceDictionary.TryGetValue(p.RepairId, out var i))
                        {
                            i = p;
                            p.LineItems = new List<InvoiceLineItemDto>();
                            invoiceDictionary.Add(i.RepairId, i);
                        }

                        if (c != null)
                            (i.LineItems as List<InvoiceLineItemDto>)?.Add(c);

                        return i;
                    },
                    splitOn: "RequestId").FirstOrDefault();
                    workItems.AddRange(reader.Read<InvoiceWorkItemDto>().ToList());
            }

            // Process Work Items.
            foreach (var item in invoice?.LineItems ?? new List<IInvoiceLineItemDto>())
            {
                var requestWorkItems = workItems.Where(x => x.RequestId == item.RequestId).ToList<IInvoiceWorkItemDto>();
                item.WorkItems = requestWorkItems;
            }

            // Update Memo.
            if (invoice != null && string.IsNullOrEmpty(invoice?.InvoiceCustomerMemo))
            {
                invoice.InvoiceCustomerMemo = string.Join("",
                    invoice.LineItems.OrderBy(i => i.RequestSortOrder).Select(i => i.RequestGeneratedMemo)
                        .Distinct());
            }

            // Return.
            return invoice;
        }

        public Task<IInvoiceDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IInvoiceDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IInvoiceDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IInvoiceDto> GetAllByGridPage(ServiceArgs args = null)
        {
            // Load Status.
            var status = 3;
            if ((args?.ContainsKey("StatusFilter") ?? false) && Enum.TryParse(args["StatusFilter"]?.ToString(),  out RepairStatuses statusFilter))
            {
                if (statusFilter == RepairStatuses.Completed || statusFilter == RepairStatuses.Invoiced)
                    status = (int)statusFilter;
            }

            // Load Grid Items.
            var query = Conn.Query<InvoiceDto>("Billing.usp_GetBillingInvoiceGrid", 
                new
                {
                    UserGuid = User.UserGuid,
                    Search = args?["SearchPhrase"]?.ToString(),
                    Status = status
                }, null, true, null, CommandType.StoredProcedure).ToList();

            // Set Default Sort.
            args?.SetDefaultSort("InvoicedDt DESC, RepairLastUpdatedDt ASC");

            // Load Grid Page.
            var result = query.ToList<IInvoiceDto>().GetGridPageFromCollection(args);

            // Return.
            return result;
        }

        public Task<IGridPageDto<IInvoiceDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IInvoiceDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            // Load Arguments.
            var args = new ServiceArgs();
            args.AddGridOptions(pageNumber, pageSize, sort, searchPhrase);

            // Return Grid.
            return GetAllByGridPage(args);
        }

        public IInvoiceDto Save(IInvoiceDto invoice)
        {
            invoice.SendNotifications = false;
            var rep = Db.RepairOrders.Find(invoice.RepairId);
            var inv = Db.RepairInvoices.SingleOrDefault(x => x.Repair.OrderId == rep.OrderId);

            var userGuid = User.UserGuid;

            if (inv != null)
            {
                // Update Invoice.
                if (inv.CustomerMemo != invoice.InvoiceCustomerMemo)
                    inv.CustomerMemo = invoice.InvoiceCustomerMemo;

                // Check Invoiced.
                if (inv.InvoicedInd != invoice.InvoiceCompleteInd)
                {
                    inv.InvoicedInd = invoice.InvoiceCompleteInd;
                    if (invoice.InvoiceCompleteInd)
                    {
                        invoice.SendNotifications = true;
                        inv.InvoicedDt = DateTimeOffset.UtcNow;
                        inv.InvoicedByUserGuid = userGuid;
                    }
                    else
                    {
                        inv.InvoicedByUser = null;
                        inv.InvoicedDt = null;
                    }
                }

                inv.CurrencyId = invoice.InvoiceCurrencyId;
                inv.UpdatedByUserGuid = userGuid;
                inv.UpdatedDt = DateTimeOffset.UtcNow;
            }
            else
            {
                // Create Invoice.
                inv = new InvoiceEntityModel
                {
                    Repair = rep,
                    CurrencyId = invoice.InvoiceCurrencyId,
                    CustomerMemo = invoice.InvoiceCustomerMemo,
                    CreatedByUserGuid = userGuid
                };

                // Check Invoiced.
                if (invoice.InvoiceCompleteInd)
                {
                    invoice.SendNotifications = true;

                    inv.InvoicedInd = invoice.InvoiceCompleteInd;
                    inv.InvoicedDt = DateTimeOffset.UtcNow;
                    inv.InvoicedByUserGuid = userGuid;
                }

                Db.Entry(inv).State = System.Data.Entity.EntityState.Added;
            }

            // Update Repair Status.
            rep.Status = inv.InvoicedInd ? RepairStatuses.Invoiced : RepairStatuses.Completed;

            // Update Scan Reports.
            if (invoice.LineItems != null)
            {
                foreach (var upd in invoice.LineItems)
                {
                    // Load Current Report.
                    var cur = Db.ScanReports.Find(upd.ReportId);
                    if (upd.InvoicedInd && !cur.InvoicedInd)
                    {
                        cur.InvoicedByUserGuid = userGuid;
                        cur.InvoicedDt = DateTimeOffset.UtcNow;
                    }

                    if (!upd.InvoicedInd)
                    {
                        cur.InvoicedByUser = null;
                        cur.InvoicedDt = null;
                    }

                    cur.InvoicedInd = upd.InvoicedInd;
                    cur.InvoiceAmount = upd.InvoicedAmount;

                    foreach (var workItem in upd.WorkItems ?? new List<IInvoiceWorkItemDto>())
                    {
                        var curWorkItem = Db.ScanReportWorkTypes.FirstOrDefault(x =>
                            x.WorkTypeId == workItem.WorkTypeId && x.ReportId == upd.ReportId);
                        if (curWorkItem != null)
                        {
                            if (workItem.InvoicedInd && !curWorkItem.InvoicedInd)
                            {
                                curWorkItem.InvoicedByUserGuid = userGuid;
                            }

                            if (!workItem.InvoicedInd)
                            {
                                curWorkItem.InvoicedByUser = null;
                            }

                            curWorkItem.InvoicedInd = workItem.InvoicedInd;
                            curWorkItem.InvoiceAmount = workItem.InvoicedAmount;
                        }
                    }
                }
            }

            // Update DB.
            Db.SaveChanges();

            return invoice;
        }

        public Task<IInvoiceDto> SaveAsync(IInvoiceDto update)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
