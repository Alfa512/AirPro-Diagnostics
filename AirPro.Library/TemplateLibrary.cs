using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using AirPro.Entities;
using AirPro.Entities.Notifications;
using AirPro.Library.Models.Concrete;

namespace AirPro.Library
{
    public class TemplateLibrary : BaseLibrary
    {
        public TemplateLibrary (EntityDbContext context, IIdentity user) : base(context, user) { }

        public async Task<TemplateEditViewModel> GetEditViewTemplate(int templateId)
        {
            NotificationTemplateEntityModel template = await Db.NotificationTemplates.FindAsync(templateId);

            TemplateEditViewModel model = new TemplateEditViewModel(template, User.TimeZoneInfoId);

            return model;
        }

        public async Task<TemplateEditViewModel[]> GetAllEditViewTemplates()
        {
            var result = (from template in await Db.NotificationTemplates.ToListAsync() select new TemplateEditViewModel(template, User.TimeZoneInfoId)).ToList();

            return result.ToArray<TemplateEditViewModel>();
        }

        public async Task UpdateEditViewTemplate (TemplateEditViewModel template)
        {
            NotificationTemplateEntityModel update = await Db.NotificationTemplates.FindAsync(template.TemplateID);
            await Db.Entry(update).Reference(r => r.CreatedBy).LoadAsync();

            update.EmailBody = template.EmailBody;
            update.Subject = template.Subject;
            update.TextMessage = template.TextMessage;
            update.UpdatedBy = User;
            update.UpdatedDt = DateTimeOffset.Now;

            await Db.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
