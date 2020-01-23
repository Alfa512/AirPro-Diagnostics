using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Library;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Attributes;
using AirPro.Site.Hubs;
using AirPro.Site.Results;
using AirPro.Storage;
using AutoMapper;
using Microsoft.AspNet.Identity;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Billing.Controllers
{
    [AuthorizeRoles(ApplicationRoles.InvoiceCreate, ApplicationRoles.InvoiceDelete, ApplicationRoles.InvoiceEdit, ApplicationRoles.InvoiceView)]
    public class InvoicingController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetInvoicesByPage(int current, int rowCount, string searchPhrase, string statusFilter)
        {
            // Load Arguments.
            var args = new ServiceArgs();
            args.AddGridOptions(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase);
            args.Add("StatusFilter", Enum.TryParse(statusFilter, out RepairStatuses status) ? status : (object)null);

            // Load Grid Page.
            var repairs = Factory.GetAllByGridPage<IInvoiceDto>(args);

            // Return Result.
            return new JsonCamelCaseResult(repairs, JsonRequestBehavior.DenyGet);
        }

        [AuthorizeRoles(ApplicationRoles.InvoiceCreate, ApplicationRoles.InvoiceDelete, ApplicationRoles.InvoiceEdit, ApplicationRoles.InvoiceView)]
        public ActionResult Invoice(int id)
        {
            ViewBag.Currencies = SelectListItemCache.BillingCurrencySelectItems();

            var invoiceDto = Factory.GetById<IInvoiceDto>(id.ToString());
            var invoiceViewModel = Mapper.Map<Models.InvoiceViewModel>(invoiceDto);

            return View(invoiceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(ApplicationRoles.InvoiceCreate, ApplicationRoles.InvoiceDelete, ApplicationRoles.InvoiceEdit)]
        public async Task<ActionResult> Invoice(Models.InvoiceViewModel invoice)
        {
            var invoiceDto = Mapper.Map<IInvoiceDto>(invoice);
            var result = Factory.Save(invoiceDto);

            var repairId = invoice.RepairId;

            // Send Notifications.
            if (result.SendNotifications)
            {
                using (var queue = new MessageQueue())
                {
                    var userGuid = Guid.Parse(User.Identity.GetUserId());
                    await queue.AddNotificationQueueMessageAsync(template: NotificationTemplate.BillingEmail, id: repairId, userGuid: userGuid);
                    await queue.AddNotificationQueueMessageAsync(template: NotificationTemplate.ShopInvoiceEmail, id: repairId, userGuid: userGuid);
                }
            }

            if (invoice.InvoiceCompleteInd)
            {
                string invoiceUrl = Url.Action("Invoice", "Download", new { Area = string.Empty, ID = repairId }, Request?.Url?.Scheme);
                await new ClientHubMessenger().InvoiceCompleted(repairId, invoiceUrl).ConfigureAwait(continueOnCapturedContext: false);
            }

            return RedirectToAction("Dashboard");
        }
    }
}
