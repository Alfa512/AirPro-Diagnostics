using System;
using System.Collections.Generic;
using AirPro.Entities.Access;
using AirPro.Entities.Repair;
using AirPro.Entities.Scan;
using AirPro.Entities.Support;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Entities.Billing;
using AirPro.Entities.Common;
using AirPro.Entities.Inventory;
using AirPro.Entities.Diagnostics;
using AirPro.Entities.Notifications;
using AirPro.Entities.Reporting;
using AirPro.Entities.Service;
using AirPro.Entities.Technician;

namespace AirPro.Entities
{
    public partial class EntityDbContext : IdentityDbContext<UserEntityModel, RoleEntityModel, Guid, UserLoginEntityModel, UserRoleEntityModel, UserClaimEntityModel>
    {
        #region Common Tables

        public DbSet<StateEntityModel> States { get; set; }
        public DbSet<ReleaseNoteEntityModel> ReleaseNotes { get; set; }
        public DbSet<ReleaseNoteRoleEntityModel> ReleaseNoteRoles { get; set; }
        public DbSet<UploadEntityModel> Uploads { get; set; }
        public DbSet<UploadTypeEntityModel> UploadTypes { get; set; }
        public DbSet<NoteEntityModel> Notes { get; set; }
        public DbSet<NoteTypeEntityModel> NoteTypes { get; set; }

        #endregion

        #region Service Tables

        public DbSet<CccEstimateEntityModel> CccEstimates { get; set; }

        public DbSet<MitchellRequestEntityModel> MitchellRepairs { get; set; }

        public DbSet<MitchellReportEntityModel> MitchellReports { get; set; }

        public DbSet<MitchellRegistrationEntityModel> MitchellRegistrations { get; set; }

        public DbSet<CCCInsuranceCompanyEntityModel> CCCInsuranceCompanies { get; set; }

        #endregion

        #region App Tables

        public DbSet<RequestLogEntityModel> RequestLogs { get; set; }
        public DbSet<ConnectionEntityModel> Connections { get; set; }
        public DbSet<ConnectionLogEntityModel> ConnectionLogs { get; set; }
        public DbSet<ApplicationExceptionEntityModel> ApplicationExceptions { get; set; }

        #endregion

        #region Notifications

        public DbSet<NotificationTypeEntityModel> NotificationTypes { get; set; }
        public DbSet<NotificationTypeRoleEntityModel> NotificationTypeRoles { get; set; }
        public DbSet<NotificationUserOptOutEntityModel> NotificationUserOptOuts { get; set; }
        public DbSet<NotificationLogEntityModel> NotificationLogs { get; set; }
        public DbSet<NotificationTemplateEntityModel> NotificationTemplates { get; set; }
        public ICollection<NotificationUserEntityModel> NotificationUsers(Guid shopGuid, NotificationTypes type)
            => this.Database.SqlQuery<NotificationUserEntityModel>("EXEC [Notification].[usp_GetNotificationUsers] @ShopGuid, @NotificationType;", 
                new SqlParameter("@ShopGuid", shopGuid.ToString()), new SqlParameter("@NotificationType", type.ToString())).ToList();

        #endregion

        #region Scan Tables

        public DbSet<RequestEntityModel> ScanRequests { get; set; }

        public DbSet<RequestArchiveEntityModel> ScanRequestArchives { get; set; }

        public DbSet<RequestTypeEntityModel> ScanRequestTypes { get; set; }

        public DbSet<RequestWarningIndicatorEntityModel> ScanRequestWarningIndicators { get; set; }

        public DbSet<WarningIndicatorEntityModel> ScanWarningIndicators { get; set; }

        public DbSet<ReportEntityModel> ScanReports { get; set; }

        public DbSet<ReportArchiveEntityModel> ScanReportArchives { get; set; }

        public DbSet<WorkTypeEntityModel> ScanWorkTypes { get; set; }

        public DbSet<WorkTypeGroupEntityModel> ScanWorkTypeGroups { get; set; }

        public DbSet<WorkTypeRequestTypeEntityModel> ScanWorkTypeRequestTypes { get; set; }

        public DbSet<ReportWorkTypeEntityModel> ScanReportWorkTypes { get; set; }

        public DbSet<RequestCategoryEntityModel> ScanRequestCategories { get; set; }

        public DbSet<RequestCategoryTypeEntityModel> ScanRequestCategoryTypes { get; set; }

        public DbSet<ReportDecisionEntityModel> ScanReportDecisions { get; set; }

        public DbSet<ReportOrderTroubleCodeEntityModel> ScanReportOrderTroubleCodes { get; set; }
        public DbSet<ReportTroubleCodeRecommendationEntityModel> ScanReportTroubleCodeRecommendations { get; set; }

        public DbSet<DecisionEntityModel> ScanDecisions { get; set; }
        public DbSet<DecisionRequestCategoryEntityModel> ScanDecisionRequestCategories { get; set; }
        public DbSet<DecisionRequestTypeEntityModel> ScanDecisionRequestTypes { get; set; }
        public DbSet<DecisionVehicleMakeEntityModel> ScanDecisionVehicleMakes { get; set; }

        public DbSet<TroubleCodeRecommendationEntityModel> ScanTroubleCodeRecommendations { get; set; }
        public DbSet<CancelReasonTypeEntityModel> CancelReasonTypes { get; set; }
        public DbSet<ReportVehicleMakeToolEntityModel> ReportVehicleMakeTools { get; set; }
        
        public DbSet<ReportValidationRuleEntityModel> ScanReportValidationRules { get; set; }

        #endregion

        #region Access Tables

        public DbSet<UserRoleEntityModel> UserRoles { get; set; }

        public DbSet<AccountEntityModel> Accounts { get; set; }

        public DbSet<AccountArchiveEntityModel> AccountArchives { get; set; }

        public DbSet<UserAccountEntityModel> UserAccounts { get; set; }

        public DbSet<UserAccountArchiveEntityModel> UserAccountArchives { get; set; }

        public DbSet<ShopEntityModel> Shops { get; set; }

        public DbSet<ShopContactEntityModel> ShopContacts { get; set; }

        public DbSet<ShopArchiveEntityModel> ShopArchives { get; set; }

        public DbSet<ShopRequestTypeEntityModel> ShopRequestTypes { get; set; }

        public DbSet<UserShopEntityModel> UserShops { get; set; }

        public DbSet<UserShopArchiveEntityModel> UserShopArchives { get; set; }

        public DbSet<GroupEntityModel> Groups { get; set; }

        public DbSet<GroupArchiveEntityModel> GroupArchives { get; set; }

        public DbSet<GroupRoleEntityModel> GroupRoles { get; set; }

        public DbSet<GroupRoleArchiveEntityModel> GroupRoleArchives { get; set; }

        public DbSet<UserGroupEntityModel> UserGroups { get; set; }

        public DbSet<UserGroupArchiveEntityModel> UserGroupArchives { get; set; }

        public DbSet<LoginEntityModel> Logins { get; set; }

        public DbSet<UserArchiveEntityModel> UserArchives { get; set; }

        public DbSet<UserPreferenceEntityModel> UserPreferences { get; set; }

        public DbSet<RegistrationEntityModel> Registrations { get; set; }

        #endregion

        #region Billing Tables

        public DbSet<PaymentEntityModel> Payments { get; set; }

        public DbSet<PaymentTypeEntityModel> PaymentTypes { get; set; }

        public DbSet<PaymentTransactionEntityModel> PaymentTransactions { get; set; }

        public DbSet<PricingPlanEntityModel> PricingPlans { get; set; }

        public DbSet<PricingPlanWorkTypeEntityModel> PricingPlanWorkTypes { get; set; }

        public DbSet<PricingPlanRequestTypeEntityModel> PricingPlanRequestTypes { get; set; }

        public DbSet<EstimatePlanEntityModel> EstimatePlans { get; set; }

        public DbSet<CurrencyEntityModel> Currencies { get; set; }

        public DbSet<BillingCycleEntityModel> BillingCycles { get; set; }

        #endregion

        #region Repair Tables

        public DbSet<OrderEntityModel> RepairOrders { get; set; }

        public DbSet<OrderStatusEntityModel> RepairOrderStatuses { get; set; }

        public DbSet<VehicleEntityModel> RepairVehicles { get; set; }

        public DbSet<VehicleLookupEntityModel> RepairVehicleLookups { get; set; }

        public DbSet<InvoiceEntityModel> RepairInvoices { get; set; }

        public DbSet<InsuranceCompanyEntityModel> InsuranceCompanies { get; set; }

        public DbSet<VehicleMakeEntityModel> RepairVehicleMakes { get; set; }

        public DbSet<VehicleMakeTypeEntityModel> RepairVehicleMakeTypes { get; set; }

        public DbSet<PointOfImpactEntityModel> PointOfImpacts { get; set; }

        public DbSet<VehicleMakeToolEntityModel> VehicleMakeTools { get; set; }


        #endregion

        #region Diagnostics

        public DbSet<DiagnosticToolEntityModel> DiagnosticTools { get; set; }
        public DbSet<DiagnosticResultEntityModel> DiagnosticResults { get; set; }
        public DbSet<DiagnosticResultTroubleCodeEntityModel> DiagnosticResultTroubleCodes { get; set; }
        public DbSet<DiagnosticResultFreezeFrameEntityModel> DiagnosticResultFreezeFrames { get; set; }

        public DbSet<DiagnosticUploadFileTypeEntityModel> DiagnosticUploadFileTypes { get; set; }
        public DbSet<DiagnosticUploadEntityModel> DiagnosticUploads { get; set; }

        public DbSet<DiagnosticVehicleControllerEntityModel> DiagnosticVehicleControllers { get; set; }

        #endregion

        #region Technician Tables

        public DbSet<TechnicianProfileEntityModel> TechnicianProfiles { get; set; }

        public DbSet<LocationEntityModel> Locations { get; set; }

        #endregion

        #region Reporting Tables

        public DbSet<ReportTemplateEntityModel> ReportTemplates { get; set; }

        #endregion

        #region Inventory Tables

        public DbSet<AirProToolEntityModel> AirProTools { get; set; }

        public DbSet<AirProToolShopEntityModel> AirProToolShops { get; set; }

        public DbSet<AirProToolAccountEntityModel> AirProToolAccounts { get; set; }

        public DbSet<AirProToolArchiveEntityModel> AirProToolArchives { get; set; }

        public DbSet<AirProToolAccountArchiveEntityModel> AirProToolAccountsArchive { get; set; }

        public DbSet<AirProToolShopArchiveEntityModel> AirProToolShopsArchive { get; set; }

        #endregion

        public EntityDbContext() : base("DefaultConnection") { }

        public EntityDbContext(string connectionString) : base(connectionString) { }

        public static EntityDbContext Create()
        {
            return new EntityDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Update ASP Net Tables.
            modelBuilder.Entity<UserEntityModel>()
                .ToTable(tableName: "Users", schemaName: "Access")
                .Property(p => p.Id).HasColumnName("UserGuid");

            modelBuilder.Entity<RoleEntityModel>()
                .ToTable(tableName: "Roles", schemaName: "Access")
                .Property(p => p.Id).HasColumnName("RoleGuid");

            modelBuilder.Entity<UserRoleEntityModel>()
                .ToTable(tableName: "UserRoles", schemaName: "Access");
            modelBuilder.Entity<UserRoleEntityModel>()
                .Property(p => p.UserId).HasColumnName("UserGuid");
            modelBuilder.Entity<UserRoleEntityModel>()
                .Property(p => p.RoleId).HasColumnName("RoleGuid");

            modelBuilder.Entity<UserLoginEntityModel>()
                .ToTable(tableName: "UserLogins", schemaName: "Access");
            modelBuilder.Entity<UserLoginEntityModel>()
                .Property(p => p.UserId).HasColumnName("UserGuid");

            modelBuilder.Entity<UserClaimEntityModel>()
                .ToTable(tableName: "UserClaims", schemaName: "Access")
                .Property(p => p.Id).HasColumnName("ClaimId");
            modelBuilder.Entity<UserClaimEntityModel>()
                .Property(p => p.UserId).HasColumnName("UserGuid");
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(FormatValidationException(ex), ex.EntityValidationErrors);
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException ex)
            {
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(FormatValidationException(ex), ex.EntityValidationErrors);
            }
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(FormatValidationException(ex), ex.EntityValidationErrors);
            }
        }

        private string FormatValidationException(DbEntityValidationException ex)
        {
            // Retrieve the error messages as a list of strings.
            var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            // Join the list to a single string.
            var fullErrorMessage = string.Join("; ", errorMessages);

            // Combine the original exception message with the new one.
            return string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
        }
    }
}
