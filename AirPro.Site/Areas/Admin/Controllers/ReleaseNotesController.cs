using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Admin.Models.ReleaseNote;
using AirPro.Site.Attributes;
using AirPro.Site.Results;
using AutoMapper;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Admin.Controllers
{
    [AuthorizeRoles(ApplicationRoles.ReleaseNoteCreate, ApplicationRoles.ReleaseNoteDelete, ApplicationRoles.ReleaseNoteEdit, ApplicationRoles.ReleaseNoteView)]
    public class ReleaseNotesController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetReleaseNotesByPage(int current, int rowCount, string searchPhrase)
        {
            // Load Grid.
            return new JsonCamelCaseResult(Factory.GetAllByGridPage<IReleaseNoteDto>(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.ReleaseNoteDelete)]
        public ActionResult DeleteReleaseNote(string id)
        {
            return new JsonCamelCaseResult(Factory.Delete<IReleaseNoteDto>(id));
        }

        [HttpGet]
        [AuthorizeRoles(ApplicationRoles.ReleaseNoteEdit, ApplicationRoles.ReleaseNoteCreate)]
        public ActionResult ManageReleaseNote(string id)
        {
            // Load Release Note.
            var releaseNoteViewModel = !string.IsNullOrEmpty(id)
                ? Mapper.Map<ReleaseNoteViewModel>(Factory.GetById<IReleaseNoteDto>(id))
                : new ReleaseNoteViewModel
                {
                    
                };

            return PartialView("_ReleaseNoteManage", releaseNoteViewModel);
        }

        [HttpPost]
        [AuthorizeRoles(ApplicationRoles.ReleaseNoteEdit, ApplicationRoles.ReleaseNoteCreate)]
        public ActionResult ManageReleaseNote(ReleaseNoteViewModel model)
        {
            // Check Model.
            if (ModelState.IsValid)
            {
                model = Mapper.Map<ReleaseNoteViewModel>(Factory.Save(model as IReleaseNoteDto));
            }

            return PartialView("_ReleaseNoteManage", model);
        }
    }
}