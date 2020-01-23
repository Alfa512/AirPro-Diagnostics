using AirPro.Common.Enumerations;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Models.Notes;
using AirPro.Site.Results;
using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AirPro.Site.Controllers
{
    [Authorize]
    public class NoteController : BaseController
    {
        public ActionResult NoteControl(NoteType type, string key, string title)
        {
            var model = new NoteControlViewModel
            {
                Type = type,
                Key = key
            };

            if (!string.IsNullOrEmpty(title))
                model.Title = title;

            var user = Factory.GetById<IUserDto>(User.Identity.GetUserId());
            model.User = user.DisplayName;

            return PartialView("_NoteControl", model);
        }

        [HttpPost]
        public async Task<ActionResult> GetNotes(NoteType type, string key)
        {
            var canView = User.IsInRole($"{type}NoteView") || User.IsInRole($"{type}NoteEdit");
            if (!canView) return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Note View Permission Denied.");

            // Load Arguments.
            var args = new ServiceArgs();
            args.Add("NoteTypeId", (int)type);
            args.Add("NoteKey", key);

            var result = await Factory.GetAllAsync<INoteDto>(args);

            return new JsonCamelCaseResult(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id, NoteType type)
        {
            var canDelete = User.IsInRole($"{type}NoteDelete");
            if(!canDelete) return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Note Delete Permission Denied.");
            
            return await Factory.DeleteAsync<INoteDto>(id.ToString())
                ? new HttpStatusCodeResult(HttpStatusCode.OK, "Note Deleted.")
                : new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Note Delete Failed.");
        }

        [HttpPost]
        public async Task<ActionResult> Save(NoteViewModel note)
        {
            var canCreate = User.IsInRole($"{note.NoteTypeId}NoteCreate");
            var canEdit = User.IsInRole($"{note.NoteTypeId}NoteEdit");
            if (!canEdit && !canCreate) return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Note Save Permission Denied.");

            try
            {
                // Save Note.
                var result = await Factory.SaveAsync((INoteDto)note);
                if (result?.UpdateResult?.Success ?? false)
                    return new JsonCamelCaseResult(result, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                Logging.Logger.LogException(e);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Note Save Failed.");
        }
    }
}