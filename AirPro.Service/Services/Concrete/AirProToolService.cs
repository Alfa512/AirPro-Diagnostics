using AirPro.Common.Enumerations;
using AirPro.Entities.Inventory;
using AirPro.Service.DTOs.Concrete;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Services.Abstract;
using AirPro.Service.Services.Interface;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AirPro.Service.Services.Concrete
{
    internal class AirProToolService : ServiceBase, IService<IAirProToolDto>
    {
        public AirProToolService(IServiceSettings settings) : base(settings)
        {
        }

        public IAirProToolDto GetById(string id)
        {
            var entity = Db.AirProTools
                    .Include(d => d.Subscriptions)
                    .Include(d => d.Deposits)
                    .FirstOrDefault(t => t.ToolId.ToString() == id);

            var airProToolDto = Mapper.Map<AirProToolDto>(entity);
            return airProToolDto;
        }

        public Task<IAirProToolDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string id)
        {
            var airProTool = Db.AirProTools.Select(x => new { x.ToolName, x.ToolId }).FirstOrDefault(x => x.ToolId.ToString() == id);
            return airProTool != null ? airProTool.ToolName : null;
        }

        public Task<string> GetDisplayNameAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, string>> GetDisplayList(ServiceArgs args = null)
        {
            // Create Tools List.
            var tools = new List<AirProToolEntityModel>();

            var toolTypes = new List<ToolType> { ToolType.AirPro };
            if (args != null && args.ContainsKey("ToolTypes"))
            {
                toolTypes = args["ToolTypes"].ToString().Split(',').Select(x => (ToolType)Enum.Parse(typeof(ToolType), x)).ToList();
            }

            // Check for Shop Guid Filter.
            if (args != null && args.ContainsKey("ShopGuid") && Guid.TryParse(args["ShopGuid"].ToString(), out var shopGuid))
            {
                // Select Tools Based on Shop.
                tools.AddRange(Db.AirProToolShops.Where(s => s.ShopGuid == shopGuid && toolTypes.Any(y => y == s.Tool.Type)).Select(t => t.Tool)
                    .Union(Db.AirProToolAccounts.Where(a => a.Account.Shops.Any(s => s.ShopGuid == shopGuid) && toolTypes.Any(y => y == a.Tool.Type))
                        .Select(t => t.Tool)).ToList());
            }
            else
            {
                // Load All.
                tools.AddRange(Db.AirProTools.Where(x => toolTypes.Any(y => y == x.Type)).ToList());
            }

            // Return Tools.
            return tools.Select(t => new KeyValuePair<string, string>(t.ToolId.ToString(),
                    t.ToolName)).ToList();
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> GetDisplayListAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAirProToolDto> GetAll(ServiceArgs args = null)
        {
            var result = new List<IAirProToolDto>();

            if (!UserHasRoles(ApplicationRoles.InventoryDeviceView, ApplicationRoles.InventoryDeviceEdit)) return result;

            result.AddRange(
                Db.AirProTools
                    .Include(t => t.CreatedBy)
                    .Include(t => t.UpdatedBy)
                    .Include(t => t.Subscriptions)
                    .Include(d => d.Deposits)
                    .Include(d => d.Shops)
                    .Include(d => d.Accounts)
                    .ToList()
                    .Select(Mapper.Map<AirProToolDto>));

            return result;
        }

        public Task<IEnumerable<IAirProToolDto>> GetAllAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IAirProToolDto> GetAllByGridPage(ServiceArgs args = null)
        {
            // Search Tools.
            var search = args?["SearchPhrase"]?.ToString().ToLower();

            bool toolTypeParseResult = Enum.TryParse<ToolType>(Enum.GetNames(typeof(ToolType)).FirstOrDefault(x => x.ToLower() == search) ?? "", out var type);
            var filtered = string.IsNullOrEmpty(search)
                ? Db.AirProTools
                : Db.AirProTools.Where(t => t.ToolId.ToString().Contains(search) || t.ToolKey.ToString().Contains(search)
                    || t.AutoEnginuityNum.ToLower().Contains(search) || t.AutoEnginuityVersion.ToLower().Contains(search)
                    || t.CarDaqNum.ToLower().Contains(search) || t.DGNum.ToLower().Contains(search)
                    || t.TeamViewerId.ToLower().Contains(search) || t.WindowsVersion.ToLower().Contains(search)
                    || t.TabletModel.ToLower().Contains(search) || t.HubModel.ToLower().Contains(search)
                    || t.Shops.Any(s => s.Shop.Name.Contains(search)) || t.Accounts.Any(a => a.Account.Name.Contains(search))
                    || t.ToolName.ToLower().Contains(search) || t.Type == type);

            // Execute.
            var result = filtered
                .Include(t => t.CreatedBy)
                .Include(t => t.UpdatedBy)
                .Include(t => t.Subscriptions)
                .Include(d => d.Deposits)
                .Include(d => d.Shops)
                .Include(d => d.Accounts)
                .ToList()
                .Select(Mapper.Map<AirProToolDto>).ToList<IAirProToolDto>();

            // Set Default Sort.
            args?.SetDefaultSort("ToolId ASC");

            // Get Page.
            var page = result.GetGridPageFromCollection(args);

            // Return.
            return page;
        }

        public Task<IGridPageDto<IAirProToolDto>> GetAllByGridPageAsync(ServiceArgs args = null)
        {
            throw new NotImplementedException();
        }

        public IGridPageDto<IAirProToolDto> GetAllByGridPage(int pageNumber, int pageSize, string sort, string searchPhrase)
        {
            // Load Arguments.
            var args = new ServiceArgs();
            args.AddGridOptions(pageNumber, pageSize, sort, searchPhrase);

            // Return.
            return GetAllByGridPage(args);
        }

        public IAirProToolDto Save(IAirProToolDto tool)
        {
            AirProToolEntityModel toolEntity;
            UpdateResultDto result = new UpdateResultDto(false, "Unknown Error Occured.");

            // Check for New Tool.
            if (!tool.ToolId.HasValue)
            {
                // Verify Access for Add.
                if (!UserHasRoles(ApplicationRoles.InventoryDeviceCreate))
                {
                    // No Ability to Add.
                    tool.UpdateResult = new UpdateResultDto(false, "You do not have access to create a record.");
                    return tool;
                }

                // Get Entity.
                toolEntity = Mapper.Map<AirProToolEntityModel>(tool);

                // Generate Tool Key.
                toolEntity.ToolKey = Guid.NewGuid();

                // Get Memberships.
                var toolAccounts = tool.AccountAssignments.Select(a => new AirProToolAccountEntityModel
                {
                    Tool = toolEntity,
                    AccountGuid = a,
                    CreatedByUserGuid = User.UserGuid
                });
                var toolShops = tool.ShopAssignments.Select(s => new AirProToolShopEntityModel
                {
                    Tool = toolEntity,
                    ShopGuid = s.Key,
                    CreatedByUserGuid = User.UserGuid
                });

                // Set Creation.
                toolEntity.CreatedByUserGuid = User.UserGuid;
                toolEntity.CreatedDt = DateTimeOffset.UtcNow;

                // Add Entities.
                Db.AirProTools.Add(toolEntity);
                Db.AirProToolAccounts.AddRange(toolAccounts);
                Db.AirProToolShops.AddRange(toolShops);

                // Create Result.
                result = new UpdateResultDto(true, "Tool Created Successfully.");
            }
            else
            {
                var canDeviceEdit = UserHasRoles(ApplicationRoles.InventoryDeviceEdit);
                var canAssignmentEdit = UserHasRoles(ApplicationRoles.InventoryAssignmentEdit);
                var canDepositEdit = UserHasRoles(ApplicationRoles.InventoryDepositEdit);
                var canSubscriptionEdit = UserHasRoles(ApplicationRoles.InventorySubscriptionEdit);

                // Verify Access to Edit.
                if (!canDeviceEdit && !canAssignmentEdit && !canSubscriptionEdit && !canDepositEdit)
                {
                    // No Ability to Edit.
                    tool.UpdateResult = new UpdateResultDto(false, "You do not have access to modify this record.");
                    return tool;
                }

                // Load Tool for Update.
                toolEntity = Db.AirProTools.FirstOrDefault(t => t.ToolId == tool.ToolId);

                // Check Tool.
                if (toolEntity != null)
                {

                    if (canDeviceEdit)
                    {
                        // Update Tool Fields.
                        toolEntity.ToolPassword = tool.ToolPassword;
                        toolEntity.SelfScanEnabledInd = tool.SelfScanEnabledInd;
                        toolEntity.Type = tool.Type;


                        toolEntity.AutoEnginuityNum = tool.AutoEnginuityNum;
                        toolEntity.AutoEnginuityVersion = tool.AutoEnginuityVersion;
                        toolEntity.CarDaqNum = tool.CarDaqNum;
                        toolEntity.DGNum = tool.DGNum;
                        toolEntity.TeamViewerId = tool.TeamViewerId;
                        toolEntity.TeamViewerPassword = tool.TeamViewerPassword;
                        toolEntity.WindowsVersion = tool.WindowsVersion;
                        toolEntity.TabletModel = tool.TabletModel;
                        toolEntity.HubModel = tool.HubModel;
                        toolEntity.IPV6DisabledInd = tool.IPV6DisabledInd;
                        toolEntity.OneDriveSyncEnabledInd = tool.OneDriveSyncEnabledInd;
                        toolEntity.UpdatesServiceInd = tool.UpdatesServiceInd;
                        toolEntity.MeteredConnectionInd = tool.MeteredConnectionInd;

                        toolEntity.OBD2YConnector = tool.OBD2YConnector;
                        toolEntity.AELatestCode = tool.AELatestCode;
                        toolEntity.ChargerStyle = tool.ChargerStyle;
                        toolEntity.TabletSerialNumber = tool.TabletSerialNumber;
                        toolEntity.WifiCard = tool.WifiCard;
                        toolEntity.WifiHardwareId = tool.WifiHardwareId;
                        toolEntity.WifiDriverDate = tool.WifiDriverDate;
                        toolEntity.WifiDriverVersion = tool.WifiDriverVersion;
                        toolEntity.WifiMacAddress = tool.WifiMacAddress;
                        toolEntity.ImageVersion = tool.ImageVersion;
                        toolEntity.CellularActiveInd = tool.CellularActiveInd;
                        toolEntity.CellularProvider = tool.CellularProvider;
                        toolEntity.CellularIMEI = tool.CellularIMEI;

                        toolEntity.J2534Brand = tool.J2534Brand;
                        toolEntity.J2534Model = tool.J2534Model;
                        toolEntity.J2534Serial = tool.J2534Serial;
                    }

                    if (canSubscriptionEdit)
                    {
                        toolEntity.HondaVersion = tool.HondaVersion;
                        toolEntity.FJDSVersion = tool.FJDSVersion;
                        toolEntity.TechstreamVersion = tool.TechstreamVersion;
                        SetSubscription(tool, toolEntity);
                    }
                    if (canDepositEdit)
                    {
                        SetDeposits(tool, toolEntity);
                    }

                    // Set Update.
                    toolEntity.UpdatedByUserGuid = User.UserGuid;
                    toolEntity.UpdatedDt = DateTimeOffset.UtcNow;
                    Db.Entry(toolEntity).State = EntityState.Modified;

                    if (canAssignmentEdit)
                    {
                        SetShopsAndUpdate(tool, toolEntity);
                        SetAccountsAndUpdate(tool, toolEntity);
                    }

                    // Create Result.
                    result = new UpdateResultDto(true, "Tool Updated Successfully.");
                }
            }

            // Save.
            Db.SaveChanges();

            // Set Update Results.
            tool.UpdateResult = result;

            return tool;
        }

        public Task<IAirProToolDto> SaveAsync(IAirProToolDto update)
        {
            throw new NotImplementedException();
        }

        private void SetShopsAndUpdate(IAirProToolDto tool, AirProToolEntityModel update)
        {
            // Check Update.
            if (update == null || tool.ToolId == null) return;

            var reqsToDelete = update.Shops
                                   ?.Where(x => tool.ShopAssignments.All(y => y.Key != x.ShopGuid))
                                   .ToList() ?? new List<AirProToolShopEntityModel>();

            var reqsToAdd = tool.ShopAssignments
                                ?.Where(x => update.Shops.All(y => y.ShopGuid != x.Key))
                                .ToList() ?? new List<KeyValuePair<Guid, string>>();

            foreach (var item in reqsToDelete)
            {
                item.UpdatedByUserGuid = User.UserGuid;
                item.UpdatedDt = DateTimeOffset.UtcNow;
            }
            Db.SaveChanges();

            foreach (var item in reqsToDelete)
            {
                update.Shops?.Remove(item);
            }

            foreach (var item in reqsToAdd)
            {
                update.Shops?.Add(new AirProToolShopEntityModel
                {
                    ShopGuid = item.Key,
                    ToolId = tool.ToolId.Value,
                    CreatedByUserGuid = User.UserGuid
                });
            }
        }

        private void SetAccountsAndUpdate(IAirProToolDto tool, AirProToolEntityModel update)
        {
            // Check Update.
            if (update == null || tool.ToolId == null) return;

            var reqsToDelete = update.Accounts
                                   ?.Where(x => tool.AccountAssignments.All(y => y != x.AccountGuid))
                                   .ToList() ?? new List<AirProToolAccountEntityModel>();

            var reqsToAdd = tool.AccountAssignments
                                ?.Where(x => update.Accounts.All(y => y.AccountGuid != x))
                                .ToList() ?? new List<Guid>();

            foreach (var item in reqsToDelete)
            {
                item.UpdatedByUserGuid = User.UserGuid;
                item.UpdatedDt = DateTimeOffset.UtcNow;
            }
            Db.SaveChanges();

            foreach (var item in reqsToDelete)
            {
                update.Accounts?.Remove(item);
            }

            foreach (var item in reqsToAdd)
            {
                update.Accounts?.Add(new AirProToolAccountEntityModel
                {
                    AccountGuid = item,
                    ToolId = tool.ToolId.Value,
                    CreatedByUserGuid = User.UserGuid
                });
            }
        }

        private void SetDeposits(IAirProToolDto tool, AirProToolEntityModel update)
        {
            if (tool.Subscriptions == null) return;

            var reqsToAdd = tool.Deposits
                .Where(x => update.Deposits.All(y => y.ToolDepositId != x.ToolDepositId))
                .ToList();

            var reqsToUpdate = tool.Deposits
                .Where(x => update.Deposits.Any(y => y.ToolDepositId == x.ToolDepositId))
                .ToList();

            foreach (var item in reqsToAdd)
            {
                update.Deposits.Add(new AirProToolDepositEntityModel
                {
                    ToolId = tool.ToolId.GetValueOrDefault(),
                    Date = item.Date,
                    Description = item.Description,
                    Amount = item.Amount
                });
            }

            foreach (var item in reqsToUpdate)
            {
                var dbObj = update.Deposits.FirstOrDefault(d => d.ToolDepositId == item.ToolDepositId);
                if (dbObj == null) continue;

                dbObj.Date = item.Date;
                dbObj.Description = item.Description;
                dbObj.Amount = item.Amount;
                dbObj.DeleteInd = item.DeleteInd;
            }
        }

        private void SetSubscription(IAirProToolDto tool, AirProToolEntityModel update)
        {
            if (tool.Subscriptions == null) return;

            var reqsToAdd = tool.Subscriptions
                .Where(x => update.Subscriptions.All(y => y.ToolSubscriptionId != x.ToolSubscriptionId))
                .ToList();

            var reqsToUpdate = tool.Subscriptions
                .Where(x => update.Subscriptions.Any(y => y.ToolSubscriptionId == x.ToolSubscriptionId))
                .ToList();

            var reqsToDelete = update.Subscriptions
                .Where(
                    x => tool.Subscriptions.All(y => y.ToolSubscriptionId != x.ToolSubscriptionId))
                .ToList();

            foreach (var item in reqsToAdd)
            {
                update.Subscriptions.Add(new AirProToolSubscriptionEntityModel
                {
                    ToolId = tool.ToolId.GetValueOrDefault(),
                    Vendor = item.Vendor,
                    Username = item.Username,
                    Password = item.Password
                });
            }

            foreach (var item in reqsToUpdate)
            {
                var dbObj = update.Subscriptions.FirstOrDefault(d => d.ToolSubscriptionId == item.ToolSubscriptionId);
                if (dbObj == null) continue;

                dbObj.Vendor = item.Vendor;
                dbObj.Username = item.Username;
                dbObj.Password = item.Password;
            }

            foreach (var item in reqsToDelete)
            {
                update.Subscriptions.Remove(item);
            }
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}