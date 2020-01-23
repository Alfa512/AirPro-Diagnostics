using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Admin.Models.WorkTypes;
using AirPro.Site.Attributes;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Areas.Admin.Controllers
{
    [AuthorizeRoles(ApplicationRoles.WorkTypeAdmin)]
    public class WorkTypesController : BaseController
    {
        public ActionResult Index()
        {
            return View("WorkTypeDashboard");
        }

        #region Groups

        [HttpPost]
        public ActionResult GetWorkTypeGroupsByPage(int current, int rowCount, string searchPhrase)
        {
            var result = JsonConvert.SerializeObject(
                Factory.GetAllByGridPage<IWorkTypeGroupDto>(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase), Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return Content(result, "application/json");
        }

        [HttpGet]
        public ActionResult ManageTypeGroup(string id)
        {
            var workTypeGroup = !string.IsNullOrEmpty(id)
                ? Mapper.Map<WorkTypeGroupViewModel>(Factory.GetById<IWorkTypeGroupDto>(id))
                : new WorkTypeGroupViewModel { WorkTypeGroupActiveInd = true };

            return PartialView("_WorkTypeGroupManage", workTypeGroup);
        }

        [HttpPost]
        public ActionResult ManageTypeGroup(WorkTypeGroupViewModel workTypeGroup)
        {
            // Check Model.
            if (ModelState.IsValid)
            {
                workTypeGroup = Mapper.Map<WorkTypeGroupViewModel>(Factory.Save(workTypeGroup as IWorkTypeGroupDto));
            }

            return PartialView("_WorkTypeGroupManage", workTypeGroup);
        }

        #endregion

        #region Types

        [HttpPost]
        public ActionResult GetWorkTypesByPage(int current, int rowCount, string searchPhrase)
        {
            var result = JsonConvert.SerializeObject(
                Factory.GetAllByGridPage<IWorkTypeDto>(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase), Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return Content(result, "application/json");
        }

        [HttpGet]
        public ActionResult ManageType(string id)
        {
            id = string.IsNullOrWhiteSpace(id) ? "0" : id;

            var workType = Mapper.Map<WorkTypeViewModel>(Factory.GetById<IWorkTypeDto>(id));
                
            ViewBag.GroupTypeSelection = GroupTypeSelection(workType);

            return PartialView("_WorkTypeManage", workType);
        }

        [HttpPost]
        public ActionResult ManageType(WorkTypeViewModel workType)
        {
            // Check Model.
            if (ModelState.IsValid)
            {
                var update = Mapper.Map<IWorkTypeDto>(workType);
                workType = Mapper.Map<WorkTypeViewModel>(Factory.Save(update));
            }
            else
            {
                workType = Mapper.Map<WorkTypeViewModel>(Factory.GetById<IWorkTypeDto>(workType.WorkTypeId.ToString()));
            }

            ViewBag.GroupTypeSelection = GroupTypeSelection(workType);

            return PartialView("_WorkTypeManage", workType);
        }

        private List<SelectListItem> GroupTypeSelection(WorkTypeViewModel type)
        {
            // Load Groups.
            var groups = Factory.GetDisplayList<IWorkTypeGroupDto>().OrderBy(g => g.Value)
                .Select(g => new SelectListItem { Value = g.Key, Text = g.Value }).ToList();

            // Check Type.
            if (type?.WorkTypeGroupId > 0 && groups.All(g => g.Value != type.WorkTypeGroupId.ToString()))
            {
                // Add If Not Found.
                groups.Add(new SelectListItem{ Value = type.WorkTypeGroupId.ToString(), Text = $@"{ type.WorkTypeGroupName } (Disabled)" });
            }

            return groups;
        }

        #endregion

    }
}