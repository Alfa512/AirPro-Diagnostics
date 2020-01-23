using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Site.Attributes;

namespace AirPro.Site.Controllers
{
    // TODO: Remove Legacy Controller. Replaced 09/26/2018
    [AuthorizeRoles(ApplicationRoles.Technician, ApplicationRoles.ReportCreate, ApplicationRoles.ReportEdit, ApplicationRoles.ReportView)]
    public class ScansController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Queue", "Request");
        }

        public ActionResult Dashboard()
        {
            return RedirectToActionPermanent("Dashboard", "Request");
        }

        public ActionResult TechnicianQueue()
        {
            return RedirectToActionPermanent("Queue", "Request");
        }

        [ActionName("Request")]
        public ActionResult ScanRequest(int id)
        {
            return RedirectToActionPermanent("Report", "Request", new { id });
        }
    }
}