using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using AirPro.Entities;
using AirPro.Entities.Scan;
using AirPro.Library.Models.Concrete;
using AirPro.Logging;

namespace AirPro.Library
{
    public class ScanLibrary : BaseLibrary
    {
        public ScanLibrary(EntityDbContext context, IIdentity user) : base(context, user) { }

        public async Task<ScanRequestTypeEditViewModel[]> GetScanRequestTypes()
        {
            ScanRequestTypeEditViewModel[] result = null;

            try
            {
                // Load Request Types.
                var scanRequestTypes = await Db.ScanRequestTypes.OrderBy(s => s.SortOrder).ToListAsync();

                var viewModels = from t in scanRequestTypes
                    select (new ScanRequestTypeEditViewModel(t, User.TimeZoneInfoId));

                result = viewModels.ToArray();

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return result;
        }

        public async Task<ScanRequestTypeEditViewModel> GetScanRequestTypeById(int scanRequestTypeId)
        {
            ScanRequestTypeEditViewModel result = null;

            try
            {
                var scanRequestTypeEntity = await Db.ScanRequestTypes.FindAsync(scanRequestTypeId);

                result = new ScanRequestTypeEditViewModel(scanRequestTypeEntity, User.TimeZoneInfoId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return result;
        }

        public async Task UpdateScanRequestType(ScanRequestTypeEditViewModel model)
        {
            try
            {
                // Load Request Type.
                var type = await Db.ScanRequestTypes.FindAsync(model.RequestTypeID);
                await Db.Entry(type).Reference(m => m.CreatedBy).LoadAsync();
                await Db.Entry(type).Reference(m => m.UpdatedBy).LoadAsync();
                await Db.Entry(type).Reference(m => m.NextRequestType).LoadAsync();

                // Update Request Type.
                type.TypeName = model.TypeName;
                type.Instructions = model.Instructions;
                type.ActiveFlag = model.ActiveFlag;
                type.InvoiceMemo = model.InvoiceMemo;
                type.BillableFlag = model.BillableFlag;
                type.DefaultPrice = Convert.ToDouble(model.DefaultPrice.Replace("$", ""));
                type.ReportTemplateHtml = model.ReportTemplateHtml;
                type.UpdatedBy = User;
                type.UpdatedDt = DateTimeOffset.UtcNow;

                if (model.CategoryTypes == null)
                {
                    model.CategoryTypes = Enumerable.Empty<int>();
                }

                var reqsToDelete = type.CategoryTypes.Where(x => model.CategoryTypes.All(y => y != x.RequestCategoryId)).ToList();
                foreach (var item in reqsToDelete)
                {
                    type.CategoryTypes.Remove(item);
                }

                var reqsToAdd = model.CategoryTypes.Where(x => type.CategoryTypes.All(y => y.RequestCategoryId != x)).ToList();
                foreach (var item in reqsToAdd)
                {
                    type.CategoryTypes.Add(new RequestCategoryTypeEntityModel
                    {
                        RequestCategoryId = item,
                        RequestTypeId = type.RequestTypeId
                    });
                }

                Db.Entry(type).State = EntityState.Modified;

                // Save.
                await Db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public List<RequestCategoryViewModel> GetRequestCategories()
        {
            var result = Enumerable.Empty<RequestCategoryViewModel>().ToList();

            try
            {
                var categories = Db.ScanRequestCategories.Where(d => d.IsActive);

                result = categories.Select(d => new RequestCategoryViewModel
                {
                    RequestCategoryId = d.RequestCategoryId,
                    RequestCategoryName = d.RequestCategoryName,
                    IsActive = d.IsActive
                }).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return result;
        }
    }
}
