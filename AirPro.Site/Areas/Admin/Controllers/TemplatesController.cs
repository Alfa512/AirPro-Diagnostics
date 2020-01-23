using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Entities;
using AirPro.Library;
using AirPro.Library.Models.Concrete;
using AirPro.Site.Attributes;

namespace AirPro.Site.Areas.Admin.Controllers
{
    [AuthorizeRoles(ApplicationRoles.NotificationAdmin)]
    public class TemplatesController : Controller
    {
        private EntityDbContext db = new EntityDbContext();
        private TemplateLibrary lib => new TemplateLibrary(db, User.Identity);

        // GET: NotificationTemplates
        public async Task<ActionResult> Index()
        {
            return View(await lib.GetAllEditViewTemplates());
        }

        // GET: NotificationTemplates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateEditViewModel template = await lib.GetEditViewTemplate((int)id);
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
        }

        // GET: NotificationTemplates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateEditViewModel template = await lib.GetEditViewTemplate((int)id);
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
        }

        // POST: NotificationTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TemplateID,Subject,EmailBody,TextMessage")] TemplateEditViewModel template)
        {
            if (ModelState.IsValid)
            {
                await lib.UpdateEditViewTemplate(template);
                return RedirectToAction("Index");
            }
            return View(await lib.GetEditViewTemplate(template.TemplateID));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
