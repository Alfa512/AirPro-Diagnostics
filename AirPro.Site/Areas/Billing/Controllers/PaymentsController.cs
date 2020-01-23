using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Entities;
using AirPro.Library;
using AirPro.Library.Models.Concrete;
using AirPro.Logging;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Attributes;
using AirPro.Site.Results;
using AirPro.Storage;
using Microsoft.AspNet.Identity;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Billing.Controllers
{
    [AuthorizeRoles(ApplicationRoles.PaymentCreate, ApplicationRoles.PaymentView)]
    public class PaymentsController : BaseController
    {
        private EntityDbContext _db;
        private EntityDbContext Db => _db ?? (_db = new EntityDbContext(MvcApplication.ConnectionString));

        private PaymentsLibrary _lib;
        private PaymentsLibrary Lib => _lib ?? (_lib = new PaymentsLibrary(Db, User.Identity));

        private string ErrorNotice
        {
            get { return TempData.Keys.Contains("ErrorNotice") ? TempData["ErrorNotice"].ToString() : null; }
            set { TempData["ErrorNotice"] = value; }
        }

        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            // Set Error.
            ViewBag.ErrorNotice = ErrorNotice;

            // Return Dashboard.
            return View();
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.PaymentCreate)]
        public ActionResult GetOutstandingInvoicesByPage(int current, int rowCount, string searchPhrase, string currencyId, string shopGuid)
        {
            // Load Arguments.
            var args = new ServiceArgs();
            args.AddGridOptions(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase);
            args.Add("ShopGuid", shopGuid);
            args.Add("CurrencyId", currencyId);
            // Load Grid Page.
            var repairs = Factory.GetAllByGridPage<IOutstandingInvoiceDto>(args);

            // Return Result.
            return new JsonCamelCaseResult(repairs, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.PaymentCreate)]
        public ActionResult GetRecentPaymentsByPage(int current, int rowCount, string searchPhrase, string shopGuid = null)
        {
            // Load Arguments.
            var args = new ServiceArgs();
            args.AddGridOptions(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase);
            if(string.IsNullOrWhiteSpace(shopGuid)) args.Add("ShopGuid", shopGuid);
            // Load Grid Page.
            var repairs = Factory.GetAllByGridPage<IRecentPaymentDto>(args);

            // Return Result.
            return new JsonCamelCaseResult(repairs, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [AuthorizeRoles(ApplicationRoles.PaymentCreate)]
        public async Task<ActionResult> AcceptPayment(int? id, int currencyId) // Invoice ID
        {
            ViewBag.Currencies = SelectListItemCache.BillingCurrencySelectItems();

            // Return Partial.
            return PartialView("_AcceptPayment", model: await Lib.GetAcceptPaymentViewModel(id, currencyId));
        }

        [HttpGet]
        [AuthorizeRoles(ApplicationRoles.PaymentDelete)]
        public async Task<ActionResult> VoidPayment(int id) // Payment ID
        {
            // Void Payment.
            if (await Lib.VoidPayment(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public async Task<ActionResult> AcceptPaymentGrid(int currencyId, Guid? id)
        {
            // Return Partial.
            return PartialView("_AcceptPaymentGrid", await Lib.GetOutstandingInvoiceItemViewModels(currencyId, id));    
        }

        public int GetShopDiscountPercentage(Guid? id)
        {
            var discount = 0;

            // Check ID.
            if (!id.HasValue) return discount;

            // Load Shop.
            var shop = Db.Shops.Find(id);

            // Check Shop.
            if (shop == null) return discount;

            // Load Discount.
            discount += shop.DiscountPercentage;
            discount += shop.Account.DiscountPercentage;

            // Check Discount.
            if (discount > 100) discount = 100;

            return discount;
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.PaymentCreate)]
        public async Task<ActionResult> AcceptPayment(AcceptPaymentViewModel payment)
        {
            try
            {
                // Save Payment.
                var paymentId = await Lib.SaveAcceptPaymentViewModel(payment);
                if (paymentId == null)
                {
                    ErrorNotice = "Your payment could not be saved, please try again.";
                }
                else
                {
                    using (var queue = new MessageQueue())
                    {
                        var userGuid = Guid.Parse(User.Identity.GetUserId());
                        await queue.AddNotificationQueueMessageAsync(template: NotificationTemplate.ShopStatementEmail, id: paymentId.Value, userGuid: userGuid);
                    }
                }
            }
            catch (Exception ex)
            {
                // Set Display Text.
                ErrorNotice = ex.Message;

                // Log Error.
                Logger.LogException(ex);
            }

            // Return to Dashboard.
            return RedirectToAction("Dashboard");
        }
    }
}
