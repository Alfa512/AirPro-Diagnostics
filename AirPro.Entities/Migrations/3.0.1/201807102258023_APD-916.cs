using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD916 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Access.AccountsArchive",
                c => new
                    {
                        ArchiveId = c.Int(nullable: false, identity: true),
                        ArchiveDt = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "GETUTCDATE()"),
                        AccountGuid = c.Guid(nullable: false),
                        ActiveInd = c.Boolean(nullable: false),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        DiscountPercentage = c.Int(nullable: false),
                        Fax = c.String(),
                        Name = c.String(),
                        Phone = c.String(),
                        StateId = c.Int(nullable: false),
                        Zip = c.String(),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ArchiveId);
            
            CreateTable(
                "Access.GroupsArchive",
                c => new
                    {
                        ArchiveId = c.Int(nullable: false, identity: true),
                        ArchiveDt = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "GETUTCDATE()"),
                        Description = c.String(),
                        GroupGuid = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ArchiveId);
            
            CreateTable(
                "Access.GroupRolesArchive",
                c => new
                    {
                        ArchiveId = c.Int(nullable: false, identity: true),
                        ArchiveDt = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "GETUTCDATE()"),
                        GroupGuid = c.Guid(nullable: false),
                        RoleGuid = c.Guid(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ArchiveId);
            
            CreateTable(
                "Access.ShopsArchive",
                c => new
                    {
                        ArchiveId = c.Int(nullable: false, identity: true),
                        ArchiveDt = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "GETUTCDATE()"),
                        AccountGuid = c.Guid(nullable: false),
                        ActiveInd = c.Boolean(nullable: false),
                        AdditionalScanCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        AllowAutoRepairClose = c.Boolean(nullable: false),
                        AllowDemoScan = c.Boolean(nullable: false),
                        AllowScanAnalysis = c.Boolean(nullable: false),
                        AllowSelfScan = c.Boolean(nullable: false),
                        AllowSelfScanAssessment = c.Boolean(nullable: false),
                        AutomaticRepairCloseDays = c.Int(),
                        AverageVehiclesPerMonth = c.Int(),
                        CCCShopId = c.String(),
                        City = c.String(),
                        CurrencyId = c.Int(nullable: false),
                        DefaultInsuranceCompanyId = c.Int(),
                        DiscountPercentage = c.Int(nullable: false),
                        EstimatePlanId = c.Int(),
                        Fax = c.String(),
                        FirstScanCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HideFromReports = c.Boolean(nullable: false),
                        Name = c.String(),
                        Notes = c.String(),
                        Phone = c.String(),
                        PricingPlanId = c.Int(),
                        ShopFixedPriceInd = c.Boolean(nullable: false),
                        ShopGuid = c.Guid(nullable: false),
                        ShopNumber = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        Zip = c.String(),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ArchiveId);
            
            CreateTable(
                "Access.UserAccountsArchive",
                c => new
                    {
                        ArchiveId = c.Int(nullable: false, identity: true),
                        ArchiveDt = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "GETUTCDATE()"),
                        AccountGuid = c.Guid(nullable: false),
                        UserGuid = c.Guid(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ArchiveId);
            
            CreateTable(
                "Access.UsersArchive",
                c => new
                    {
                        ArchiveId = c.Int(nullable: false, identity: true),
                        ArchiveDt = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "GETUTCDATE()"),
                        UserGuid = c.Guid(nullable: false),
                        UserName = c.String(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        ContactNumber = c.String(),
                        FirstName = c.String(),
                        JobTitle = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        SessionId = c.String(),
                        ShopBillingNotification = c.Boolean(nullable: false),
                        ShopReportNotification = c.Boolean(nullable: false),
                        TimeZoneInfoId = c.String(),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        AccessFailedCount = c.Int(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ArchiveId);
            
            CreateTable(
                "Access.UserGroupsArchive",
                c => new
                    {
                        ArchiveId = c.Int(nullable: false, identity: true),
                        ArchiveDt = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "GETUTCDATE()"),
                        GroupGuid = c.Guid(nullable: false),
                        UserGuid = c.Guid(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ArchiveId);
            
            CreateTable(
                "Access.UserShopsArchive",
                c => new
                    {
                        ArchiveId = c.Int(nullable: false, identity: true),
                        ArchiveDt = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "GETUTCDATE()"),
                        ShopGuid = c.Guid(nullable: false),
                        UserGuid = c.Guid(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ArchiveId);

            AddColumn("Access.Users", "CreatedByUserGuid", c => c.Guid(nullable: false, defaultValueSql: "Common.udf_GetEmptyGuid()"));
            AddColumn("Access.Users", "CreatedDt", c => c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "GETUTCDATE()"));
            AddColumn("Access.Users", "UpdatedByUserGuid", c => c.Guid());
            AddColumn("Access.Users", "UpdatedDt", c => c.DateTimeOffset(precision: 7));
            AddColumn("Access.Shops", "CreatedByUserGuid", c => c.Guid(nullable: false, defaultValueSql: "Common.udf_GetEmptyGuid()"));
            AddColumn("Access.Shops", "CreatedDt", c => c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "GETUTCDATE()"));
            AddColumn("Access.Shops", "UpdatedByUserGuid", c => c.Guid());
            AddColumn("Access.Shops", "UpdatedDt", c => c.DateTimeOffset(precision: 7));
            CreateIndex("Access.Users", "CreatedByUserGuid");
            CreateIndex("Access.Users", "UpdatedByUserGuid");
            CreateIndex("Access.Shops", "CreatedByUserGuid");
            CreateIndex("Access.Shops", "UpdatedByUserGuid");
            AddForeignKey("Access.Shops", "CreatedByUserGuid", "Access.Users", "UserGuid");
            AddForeignKey("Access.Shops", "UpdatedByUserGuid", "Access.Users", "UserGuid");

            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessUsersArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessUserShopsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessUserAccountsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessUserGroupsArchive");

            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessShopsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessAccountsArchive");

            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessGroupsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessGroupRolesArchive");
        }
        
        public override void Down()
        {
            DropForeignKey("Access.Shops", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Access.Shops", "CreatedByUserGuid", "Access.Users");
            DropIndex("Access.Shops", new[] { "UpdatedByUserGuid" });
            DropIndex("Access.Shops", new[] { "CreatedByUserGuid" });
            DropIndex("Access.Users", new[] { "UpdatedByUserGuid" });
            DropIndex("Access.Users", new[] { "CreatedByUserGuid" });
            DropColumn("Access.Shops", "UpdatedDt");
            DropColumn("Access.Shops", "UpdatedByUserGuid");
            DropColumn("Access.Shops", "CreatedDt");
            DropColumn("Access.Shops", "CreatedByUserGuid");
            DropColumn("Access.Users", "UpdatedDt");
            DropColumn("Access.Users", "UpdatedByUserGuid");
            DropColumn("Access.Users", "CreatedDt");
            DropColumn("Access.Users", "CreatedByUserGuid");
            DropTable("Access.UserShopsArchive");
            DropTable("Access.UserGroupsArchive");
            DropTable("Access.UsersArchive");
            DropTable("Access.UserAccountsArchive");
            DropTable("Access.ShopsArchive");
            DropTable("Access.GroupRolesArchive");
            DropTable("Access.GroupsArchive");
            DropTable("Access.AccountsArchive");

            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessUsersArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessUserShopsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessUserAccountsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessUserGroupsArchive");

            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessShopsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessAccountsArchive");

            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessGroupsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessGroupRolesArchive");
        }
    }
}
