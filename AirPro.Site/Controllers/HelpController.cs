using AirPro.Service;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Models.Help;
using AirPro.Site.Models.Home;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AirPro.Site.Controllers
{
    [Authorize]
    public class HelpController : BaseController
    {
        private const string ReleaseNoteControlId = "ReleaseNoteShown";

        public async Task<ActionResult> Index()
        {
            var model = new HelpIndexViewModel();
            model.Version = typeof(HelpController).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            model.VersionSelectListItems = (await Factory.GetDisplayListAsync<IReleaseNoteDto>()).Select(x => x.Value).Distinct().ToList();
            if(!model.VersionSelectListItems.Contains(model.Version))
            {
                model.VersionSelectListItems.Add(model.Version);
            }
            return View(model);
        }

        public ActionResult GetReleaseNotes(string version)
        {
            var model = new ReleaseNoteInfoViewModel
            {
                ReleaseNotes = Factory.GetAll<IReleaseNoteDto>(new ServiceArgs
                {
                    {"Version", version},
                    {"UserGuid", User.Identity.GetUserId()}
                })
            };

            if (!model.ReleaseNotes.Any()) return new EmptyResult();

            return PartialView("_ReleaseNote", model);
        }

        public ActionResult GetReleaseNoteInformationIfNew()
        {
            if (!User.Identity.IsAuthenticated) return new EmptyResult();

            var currentAppVersion = typeof(HelpController).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            var releaseNoteShownUserPreference = GetReleaseNoteShownUserPreference()?.GetSettings<ReleaseNoteShownPreferenceDto>();
            var showReleaseNoteModal = releaseNoteShownUserPreference?.Version != currentAppVersion;

            if (!showReleaseNoteModal) return new EmptyResult();

            var model = new ReleaseNoteInfoViewModel
            {
                ReleaseNotes = Factory.GetAll<IReleaseNoteDto>(new ServiceArgs
                {
                    {"Version", currentAppVersion},
                    {"UserGuid", User.Identity.GetUserId()}
                })
            };

            if (!model.ReleaseNotes.Any()) return new EmptyResult();

            return PartialView("_ReleaseNoteInfoModal", model);
        }

        public ActionResult TrackReleaseNoteInfoModalShown()
        {
            var releaseNoteShownUserPreference = GetReleaseNoteShownUserPreference();
            var currentAppVersion = typeof(HomeController).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            if (releaseNoteShownUserPreference == null)
            {
                var userPreferenceDto = new UserPreferenceDto
                {
                    ControlId = ReleaseNoteControlId,
                    UserGuid = Guid.Parse(User.Identity.GetUserId()),
                    SettingsJson = JsonConvert.SerializeObject(new ReleaseNoteShownPreferenceDto { Version = currentAppVersion })
                };
                Factory.Save<IUserPreferenceDto>(userPreferenceDto);
            }
            else
            {
                var settings = releaseNoteShownUserPreference.GetSettings<ReleaseNoteShownPreferenceDto>();
                if (settings.Version != currentAppVersion)
                {
                    settings.Version = currentAppVersion;
                    releaseNoteShownUserPreference.SettingsJson = JsonConvert.SerializeObject(settings);
                    Factory.Save<IUserPreferenceDto>(releaseNoteShownUserPreference);
                }
            }

            return new EmptyResult();
        }

        private IUserPreferenceDto GetReleaseNoteShownUserPreference()
        {
            var releaseNoteShownUserPreference = Factory.GetAll<IUserPreferenceDto>(new ServiceArgs
                                                                                                        {
                                {"ControlId", ReleaseNoteControlId},
                                {"UserGuid", User.Identity.GetUserId()
                }
                            }).FirstOrDefault();
            return releaseNoteShownUserPreference;
        }
    }
}