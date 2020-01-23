using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Entities.Repair;
using AirPro.Entities.Scan;
using System.Security.Principal;
using AirPro.Entities;
using AirPro.Library.Models.Concrete;
using AirPro.Logging;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Entities.Access;
using AirPro.Storage;

namespace AirPro.Library
{
    public class RepairLibrary : BaseLibrary
    {
        private ServiceFactory Factory { get; }

        public RepairLibrary(EntityDbContext db, IIdentity identity) : base(db, identity)
        {
            Factory = new ServiceFactory(db, identity);
        }

        public async Task UpdateRepairOrder(RepairEditViewModel update)
        {
            try
            {
                // Load Repair.
                var repair =
                    Db.RepairOrders.First(r => r.OrderId == update.RepairOrderID);
                if (repair == null)
                    throw new NullReferenceException("Unable to locate Repair.");

                repair.ShopGuid = update.ShopGuid;
                repair.ShopReferenceNumber = update.ShopReferenceNumber;
                repair.InsuranceCompany = Db.InsuranceCompanies.Find(update.InsuranceCompany.InsuranceCompanyId);
                repair.InsuranceCompanyOther = update.InsuranceCompanyOther;
                repair.Odometer = update.Odometer;
                repair.AirBagsDeployed = update.AirBagsDeployed;
                repair.AirBagsVisualDeployments = update.AirBagsVisualDeployments;
                repair.DrivableInd = update.DrivableInd;
                repair.UpdatedBy = User;
                repair.UpdatedDt = DateTimeOffset.UtcNow;

                var param = new
                {
                    OrderId = repair.OrderId,
                    PointsOfImpact = update.ImpactPoints == null
                        ? string.Empty
                        : string.Join(",", update.ImpactPoints.ToList())
                };
                await Db.Database
                    .SqlQuery<object>(
                        $"Repair.usp_SaveOrderPointsOfImpact @OrderId = '{param.OrderId}' , @PointsOfImpact = '{param.PointsOfImpact}'")
                    .ToListAsync();

                Db.Entry(repair).State = EntityState.Modified;
                await Db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, update);

                throw;
            }
        }

        public async Task<int> CreateScanRequest(IRepairRequestScanDto scanRequest)
        {
            try
            {
                // Check Open Scan Requests.
                if (Db.ScanRequests.Any(
                    r => r.Repair.OrderId == scanRequest.OrderId && r.Report == null))
                    throw new Exception("Open Scan Request Found.");

                // Load Repair.
                var repair = Db.RepairOrders.Find(scanRequest.OrderId);
                if (repair == null)
                    throw new NullReferenceException("Unable to Locate Repair.");

                if (repair.Status != RepairStatuses.Active)
                    throw new NullReferenceException("Unable to Create Request for a Closed Repair.");

                Guid.TryParse(scanRequest.ContactUserGuid, out var contactUserId);
                var isUser = Db.Users.Any(x => x.Id == contactUserId);
                // Create New Request.
                var request = new RequestEntityModel
                {
                    OrderId = repair.OrderId,
                    ProblemDescription = scanRequest.ProblemDescription,
                    OtherWarningInfo = scanRequest.OtherWarningInfo,
                    Notes = scanRequest.Notes,
                    ContactUserGuid = contactUserId == Guid.Empty || !isUser ? null : new Guid?(contactUserId),
                    ShopContactGuid = contactUserId == Guid.Empty || isUser ? null : new Guid?(contactUserId),
                    SeatRemovedInd = scanRequest.SeatRemovedInd,
                    CreatedBy = User,
                    CreatedDt = DateTimeOffset.UtcNow,
                    RequestTypeId = scanRequest.RequestTypeID,
                    RequestCategoryId = scanRequest.RequestTypeCategoryId == 0 ? null : scanRequest.RequestTypeCategoryId,
                };

                if(!string.IsNullOrWhiteSpace(scanRequest.ContactOtherFirstName) && !string.IsNullOrWhiteSpace(scanRequest.ContactOtherLastName) && !string.IsNullOrWhiteSpace(scanRequest.ContactOtherPhone))
                {
                    ShopContactEntityModel shopContact = new ShopContactEntityModel
                    {
                        ShopGuid = repair.ShopGuid,
                        FirstName = scanRequest.ContactOtherFirstName,
                        LastName = scanRequest.ContactOtherLastName,
                        PhoneNumber = scanRequest.ContactOtherPhone
                    };
                    request.ShopContact = shopContact;
                }

                // Set Tool.
                if (scanRequest.ToolId > 0)
                    request.ToolId = scanRequest.ToolId;

                // Add Request.
                Db.ScanRequests.Add(request);

                // Add Warning Indicators.
                if (scanRequest.WarningIndicators != null)
                {
                    foreach (int i in scanRequest.WarningIndicators)
                    {
                        var indicator = Db.ScanWarningIndicators.Find(i);
                        if (indicator != null)
                        {
                            var requestIndicator = new RequestWarningIndicatorEntityModel
                            {
                                Request = request,
                                WarningIndicator = indicator
                            };

                            Db.ScanRequestWarningIndicators.Add(requestIndicator);
                        }
                    }
                }

                // Update DB.
                await Db.SaveChangesAsync();

                // Notify Technicians (If NOT Self Scan).
                if (request.RequestTypeId == 6) return request.RequestId;

                using (var queue = new MessageQueue())
                {
                    // Sent Email.
                    var sendMessage = queue
                        .AddNotificationQueueMessageAsync(NotificationTemplate.ScanRequestEmail, request.RequestId, User.Id)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    // Complete.
                    await sendMessage;
                }

                return request.RequestId;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, scanRequest);

                throw;
            }
        }

        public async Task<bool> CreateRepairOrder(OrderEntityModel repair)
        {
            try
            {
                repair.CreatedBy = User;
                repair.ShopGuid = repair.ShopGuid;
                repair.InsuranceCompanyId = repair.InsuranceCompanyId;

                Db.RepairOrders.Add(repair);
                await Db.SaveChangesAsync();

                var param = new
                {
                    OrderId = repair.OrderId,
                    PointsOfImpact = repair.ImpactPoints == null
                        ? string.Empty
                        : string.Join(",", repair.ImpactPoints.ToList())
                };
                await Db.Database
                    .SqlQuery<object>(
                        $"Repair.usp_SaveOrderPointsOfImpact @OrderId = '{param.OrderId}' , @PointsOfImpact = '{param.PointsOfImpact}'")
                    .ToListAsync();

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, repair);

                throw;
            }
        }
        
        public bool FindActiveRepairsByVinAndShop(string vin, Guid shopGuid)
        {
            var exists = Db.RepairOrders.Any(d =>
                d.ShopGuid == shopGuid
                && d.VehicleVIN == vin
                && d.Status == RepairStatuses.Active);

            return exists;
        }
    }
}