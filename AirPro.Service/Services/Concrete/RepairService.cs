using AirPro.Common.Enumerations;
using AirPro.Entities.Repair;
using AirPro.Entities.Scan;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AirPro.Storage;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AirPro.Service.Services.Concrete
{
    internal class RepairService : ServiceBase, IService<IRepairDto>
    {
        public RepairService(IServiceSettings settings) : base(settings) { }

        public IRepairDto GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IRepairDto> GetByIdAsync(string id)
        {
            // Load Argument.
            var args = new ServiceArgs { { "RepairId", id } };

            // Load Repair.
            var search = await GetAllByGridPageAsync(args);

            // Return First Repair.
            return search.Rows.FirstOrDefault();
        }

        public string GetDisplayName(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IRepairDto> GetAll(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IRepairDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IRepairDto> GetAllByGridPage(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IGridPageDto<IRepairDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            IGridPageDto<IRepairDto> result;

            using (var x = await Conn.QueryMultipleAsync("Repair.usp_GetRepairsByUser", param: ParamsFromArgs(User.UserGuid, args), commandType: CommandType.StoredProcedure))
            {
                // Load Grid Page.
                result = x.ReadFirst<GridPageDto<IRepairDto>>();

                // Load Repairs.
                result.Rows = x.Read<RepairDto>().ToList();

                // Load Downloads.
                var downloads = x.Read<RepairDownloadDto>().ToList();

                // Add Downloads to Repairs.
                foreach (var row in result.Rows)
                    row.RepairDownloads = downloads.Where(d => d.RepairId == row.RepairId).ToList();
            }

            return result;
        }

        public IGridPageDto<IRepairDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IRepairDto Save(IRepairDto update)
        {
            // Load Type.
            var result = update;

            if (!UserHasRoles(ApplicationRoles.RepairCreate, ApplicationRoles.RepairEdit))
            {
                // No Group Access.
                result.UpdateResult = new UpdateResultDto(false, "You do not have access to Repairs.");
                return result;
            }

            var repair = update.RepairId == 0 ? null : Db.RepairOrders.Find(update.RepairId);
            if (repair == null)
            {
                repair = new OrderEntityModel
                {
                    AirBagsDeployed = update.AirBagsDeployed,
                    AirBagsVisualDeployments = update.AirBagsVisualDeployments,
                    DrivableInd = update.DrivableInd,
                    InsuranceCompanyId = update.InsuranceCompanyId,
                    InsuranceCompanyOther = update.InsuranceCompanyOther,
                    InsuranceReferenceNumber = update.InsuranceReferenceNumber,
                    ShopReferenceNumber = update.ShopRONumber,
                    ShopGuid = update.ShopGuid,
                    Odometer = Convert.ToInt32(update.Odometer),
                    CreatedByUserGuid = User.UserGuid,
                    CreatedDt = DateTime.UtcNow,
                    VehicleVIN = update.VehicleVIN,
                    Status = RepairStatuses.Active
                };

                Db.RepairOrders.Add(repair);
                Db.SaveChanges();

                result.RepairId = repair.OrderId;

                var updateMessage = "Repair Created Successfully.";
                result.UpdateResult = new UpdateResultDto(true, updateMessage);
            }
            else
            {
                repair.AirBagsDeployed = update.AirBagsDeployed;
                repair.AirBagsVisualDeployments = update.AirBagsVisualDeployments;
                repair.DrivableInd = update.DrivableInd;
                repair.InsuranceCompanyId = update.InsuranceCompanyId;
                repair.InsuranceCompanyOther = update.InsuranceCompanyOther;
                repair.InsuranceReferenceNumber = update.InsuranceReferenceNumber;
                repair.ShopReferenceNumber = update.ShopRONumber;
                repair.ShopGuid = update.ShopGuid;
                repair.Odometer = Convert.ToInt32(update.Odometer);
                repair.VehicleVIN = update.VehicleVIN;
                repair.UpdatedDt = DateTimeOffset.UtcNow;
                repair.UpdatedByUserGuid = User.UserGuid;
                repair.Status = RepairStatuses.Active;

                Db.SaveChanges();

                var updateMessage = "Repair Updated Successfully.";
                result.UpdateResult = new UpdateResultDto(true, updateMessage);
            }

            var param = new
            {
                OrderId = repair.OrderId,
                PointsOfImpact = update.PointsOfImpact == null ? string.Empty : string.Join(",", update.PointsOfImpact.ToList())
            };

            Db.Database.SqlQuery<object>(
                    $"Repair.usp_SaveOrderPointsOfImpact @OrderId = '{param.OrderId}' , @PointsOfImpact = '{param.PointsOfImpact}'")
                .ToListAsync();

            return result;
        }

        public Task<IRepairDto> SaveAsync(IRepairDto update)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            // Check Access.
            if (!UserHasRoles(ApplicationRoles.RepairCreate, ApplicationRoles.RepairEdit))
            {
                return false;
            }

            // Load Repair.
            var repairId = Convert.ToInt32(id);
            var repair = await Db.RepairOrders.Include(x => x.Shop).FirstOrDefaultAsync(d => d.OrderId == repairId);
            if (repair == null) return false;

            // Update Repair.
            repair.Status = repair.Status == RepairStatuses.Completed || repair.Status == RepairStatuses.Canceled
                ? RepairStatuses.Active
                : repair.ScanRequests.Count > 0 ? RepairStatuses.Completed : RepairStatuses.Canceled;
            repair.UpdatedDt = DateTimeOffset.UtcNow;
            repair.UpdatedByUserGuid = User.UserGuid;

            // Save Repair.
            await Db.SaveChangesAsync();

            // Check Invoicing.
            if (!repair.Shop.AutomaticInvoicesInd || repair.Status != RepairStatuses.Completed) return true;

            // Load Invoice.
            var invoiceService = new InvoiceService(Settings);
            var invoice = invoiceService.GetById(id);

            // Complete Invoice.
            invoice.InvoiceCompleteInd = true;

            // Save Invoice.
            invoiceService.Save(invoice);

            // Generate Notifications.
            using (var queue = new MessageQueue())
            {
                await queue.AddNotificationQueueMessageAsync(template: NotificationTemplate.BillingEmail,
                    id: repairId, userGuid: User.UserGuid);
                await queue.AddNotificationQueueMessageAsync(template: NotificationTemplate.ShopInvoiceEmail,
                    id: repairId, userGuid: User.UserGuid);
            }

            return true;
        }

        private static object ParamsFromArgs(Guid userGuid, ServiceArgs args = null)
        {
            var sort = (args != null && args.ContainsKey("SortOrder") ? args["SortOrder"].ToString() : string.Empty).Replace(",", "").Split(' ');

            return new
            {
                UserGuid = userGuid,
                RepairId = args != null && args.ContainsKey("RepairId") && int.TryParse(args["RepairId"].ToString(), out var repairId) ? repairId : 0,
                AgingRepairs = args != null && args.ContainsKey("AgingRepairs") && bool.TryParse(args["AgingRepairs"].ToString(), out var isAgingRepairs) && isAgingRepairs,
                Search = args != null && args.ContainsKey("SearchPhrase") ? args["SearchPhrase"].ToString() : string.Empty,
                RepairStatusId = args != null && args.ContainsKey("StatusFilter") && Enum.TryParse(args["StatusFilter"].ToString(), out RepairStatuses status) ? status : RepairStatuses.Active,
                ShopGuid = args != null && args.ContainsKey("ShopGuid") && Guid.TryParse(args["ShopGuid"].ToString(), out var shopGuid) ? shopGuid : Guid.Empty,
                CurrentPage = args != null && args.ContainsKey("CurrentPage") && int.TryParse(args["CurrentPage"].ToString(), out var page) ? page : 1,
                RowCount = args != null && args.ContainsKey("RowCount") && int.TryParse(args["RowCount"].ToString(), out var rows) ? rows : 25,
                SortCol = sort.Length > 0 ? sort[0] : "CreatedDt",
                SortDir = sort.Length > 1 ? sort[1] : "DESC",
            };
        }
    }
}
