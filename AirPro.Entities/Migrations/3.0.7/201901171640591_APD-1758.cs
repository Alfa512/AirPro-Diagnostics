namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;

    public partial class APD1758 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Access.Registrations",
                c => new
                    {
                        RegistrationId = c.Guid(nullable: false, identity: true),
                        RegistrationStatus = c.Int(nullable: false),
                        StatusUpdateDt = c.DateTimeOffset(precision: 7),
                        CallbackUrl = c.String(),
                        Email = c.String(),
                        DifferentShopInfo = c.Boolean(nullable: false),
                        CompletedDt = c.DateTimeOffset(precision: 7),
                        RegistrationUserId = c.Int(nullable: false),
                        RegistrationAccountId = c.Int(nullable: false),
                        RegistrationShopId = c.Int(nullable: false),
                        AccountGuid = c.Guid(),
                        ShopGuid = c.Guid(),
                        ClientUserGuid = c.Guid(),
                        StatusUpdateByUserGuid = c.Guid(),
                        CompletedByUserGuid = c.Guid(),
                        PassedStep = c.Int(),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.RegistrationId)
                .ForeignKey("Access.Accounts", t => t.AccountGuid)
                .ForeignKey("Access.Users", t => t.ClientUserGuid)
                .ForeignKey("Access.Users", t => t.CompletedByUserGuid)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Access.RegistrationAccounts", t => t.RegistrationAccountId, cascadeDelete: true)
                .ForeignKey("Access.RegistrationShops", t => t.RegistrationShopId, cascadeDelete: true)
                .ForeignKey("Access.RegistrationUsers", t => t.RegistrationUserId, cascadeDelete: true)
                .ForeignKey("Access.Shops", t => t.ShopGuid)
                .ForeignKey("Access.Users", t => t.StatusUpdateByUserGuid)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.RegistrationUserId)
                .Index(t => t.RegistrationAccountId)
                .Index(t => t.RegistrationShopId)
                .Index(t => t.AccountGuid)
                .Index(t => t.ShopGuid)
                .Index(t => t.ClientUserGuid)
                .Index(t => t.StatusUpdateByUserGuid)
                .Index(t => t.CompletedByUserGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Access.RegistrationAccounts",
                c => new
                    {
                        RegistrationAccountId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                        Phone = c.String(maxLength: 15),
                        Fax = c.String(maxLength: 15),
                        Address1 = c.String(maxLength: 1024),
                        Address2 = c.String(maxLength: 1024),
                        City = c.String(maxLength: 1024),
                        StateId = c.String(),
                        Zip = c.String(maxLength: 25),
                        DiscountPercentage = c.Int(),
                    })
                .PrimaryKey(t => t.RegistrationAccountId);
            
            CreateTable(
                "Access.RegistrationShops",
                c => new
                    {
                        RegistrationShopId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                        Phone = c.String(maxLength: 15),
                        Fax = c.String(maxLength: 15),
                        Address1 = c.String(maxLength: 1024),
                        Address2 = c.String(maxLength: 1024),
                        City = c.String(maxLength: 1024),
                        StateId = c.String(),
                        Zip = c.String(maxLength: 25),
                        SendToMitchellInd = c.Boolean(nullable: false),
                        DiscountPercentage = c.Int(),
                        CCCShopId = c.String(maxLength: 128),
                        AllowAllRepairAutoClose = c.Boolean(nullable: false),
                        AllowAutoRepairClose = c.Boolean(nullable: false),
                        AllowScanAnalysisAutoClose = c.Boolean(nullable: false),
                        ShopFixedPriceInd = c.Boolean(nullable: false),
                        FirstScanCost = c.Decimal(precision: 18, scale: 2),
                        AdditionalScanCost = c.Decimal(precision: 18, scale: 2),
                        AutomaticRepairCloseDays = c.Int(),
                        HideFromReports = c.Boolean(nullable: false),
                        DisableShopBillingNotification = c.Boolean(nullable: false),
                        DisableShopStatementNotification = c.Boolean(nullable: false),
                        DefaultInsuranceCompanyId = c.Int(),
                        AverageVehiclesPerMonth = c.Int(),
                        CurrencyId = c.Int(),
                        PricingPlanId = c.Int(),
                        EstimatePlanId = c.Int(),
                        BillingCycleId = c.Int(),
                        AllowedRequestTypeIds = c.String(),
                        InsuranceCompaniesIds = c.String(),
                        InsuranceCompaniesPricingPlansJson = c.String(),
                        InsuranceCompaniesEstimatePlansJson = c.String(),
                        VehicleMakesIds = c.String(),
                        VehicleMakesPricingPlansJson = c.String(),
                    })
                .PrimaryKey(t => t.RegistrationShopId)
                .ForeignKey("Billing.Cycles", t => t.BillingCycleId)
                .ForeignKey("Billing.Currencies", t => t.CurrencyId)
                .ForeignKey("Repair.InsuranceCompanies", t => t.DefaultInsuranceCompanyId)
                .ForeignKey("Billing.EstimatePlans", t => t.EstimatePlanId)
                .ForeignKey("Billing.PricingPlans", t => t.PricingPlanId)
                .Index(t => t.CCCShopId)
                .Index(t => t.DefaultInsuranceCompanyId)
                .Index(t => t.CurrencyId)
                .Index(t => t.PricingPlanId)
                .Index(t => t.EstimatePlanId)
                .Index(t => t.BillingCycleId);
            
            CreateTable(
                "Access.RegistrationUsers",
                c => new
                    {
                        RegistrationUserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 256),
                        LastName = c.String(maxLength: 256),
                        JobTitle = c.String(maxLength: 256),
                        ContactNumber = c.String(maxLength: 50),
                        PhoneNumber = c.String(maxLength: 50),
                        AccessGroupIds = c.String(),
                        ShopBillingNotification = c.Boolean(nullable: false),
                        ShopReportNotification = c.Boolean(nullable: false),
                        TimeZoneInfoId = c.String(maxLength: 128),
                        PasswordHash = c.String(),
                    })
                .PrimaryKey(t => t.RegistrationUserId);

            this.DropObjectIfExists(DropObjectType.Procedure, "Notification", "usp_GetRegistrationNotification");
            this.DropObjectIfExists(DropObjectType.Procedure, "Notification", "usp_GetRegistrationWelcomeNotification");

            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetRegistrationGrid");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetRegistrationOptions");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_CompleteRegistration");

            Sql(@"SET IDENTITY_INSERT Notification.Templates ON;

                MERGE Notification.Templates AS t
                USING
                (
	                SELECT
		                8 [NotificationTemplateId]
		                ,'RegistrationEmail' [Name]
		                ,'RegistrationId,ClientName,CreateBy,RegistrationUrl' [Options]
		                ,'Registration' [Subject]
		                ,'Hi, </br> Please, click on <a href=""{RegistrationUrl}"">link</a> to complete registration. </br> Thank You' [EmailBody]

                    UNION

                    SELECT
		                9 [NotificationTemplateId]
		                ,'RegistrationWelcomeEmail' [Name]
		                ,'Email,RegistrationId,ClientName,CreateBy,FirstName,LastName,JobTitle,AccountName,ShopName' [Options]
		                ,'Registration Welcome' [Subject]
		                ,'Hi {FirstName} {LastName}, </br>  </br> Thank You' [EmailBody]
                ) AS s
                ON (s.NotificationTemplateId = t.NotificationTemplateId)
                WHEN NOT MATCHED THEN
	                INSERT (NotificationTemplateId, Name, Options, Subject, EmailBody, CreatedDt, CreatedByUserGuid)
	                VALUES (NotificationTemplateId, Name, Options, Subject, EmailBody, GETUTCDATE(), Common.udf_GetEmptyGuid())
                OUTPUT INSERTED.*;

                SET IDENTITY_INSERT Notification.Templates OFF;");
        }
        
        public override void Down()
        {
            DropForeignKey("Access.Registrations", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Access.Registrations", "StatusUpdateByUserGuid", "Access.Users");
            DropForeignKey("Access.Registrations", "ShopGuid", "Access.Shops");
            DropForeignKey("Access.Registrations", "RegistrationUserId", "Access.RegistrationUsers");
            DropForeignKey("Access.Registrations", "RegistrationShopId", "Access.RegistrationShops");
            DropForeignKey("Access.RegistrationShops", "PricingPlanId", "Billing.PricingPlans");
            DropForeignKey("Access.RegistrationShops", "EstimatePlanId", "Billing.EstimatePlans");
            DropForeignKey("Access.RegistrationShops", "DefaultInsuranceCompanyId", "Repair.InsuranceCompanies");
            DropForeignKey("Access.RegistrationShops", "CurrencyId", "Billing.Currencies");
            DropForeignKey("Access.RegistrationShops", "BillingCycleId", "Billing.Cycles");
            DropForeignKey("Access.Registrations", "RegistrationAccountId", "Access.RegistrationAccounts");
            DropForeignKey("Access.Registrations", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Access.Registrations", "CompletedByUserGuid", "Access.Users");
            DropForeignKey("Access.Registrations", "ClientUserGuid", "Access.Users");
            DropForeignKey("Access.Registrations", "AccountGuid", "Access.Accounts");
            DropIndex("Access.RegistrationShops", new[] { "BillingCycleId" });
            DropIndex("Access.RegistrationShops", new[] { "EstimatePlanId" });
            DropIndex("Access.RegistrationShops", new[] { "PricingPlanId" });
            DropIndex("Access.RegistrationShops", new[] { "CurrencyId" });
            DropIndex("Access.RegistrationShops", new[] { "DefaultInsuranceCompanyId" });
            DropIndex("Access.RegistrationShops", new[] { "CCCShopId" });
            DropIndex("Access.Registrations", new[] { "UpdatedByUserGuid" });
            DropIndex("Access.Registrations", new[] { "CreatedByUserGuid" });
            DropIndex("Access.Registrations", new[] { "CompletedByUserGuid" });
            DropIndex("Access.Registrations", new[] { "StatusUpdateByUserGuid" });
            DropIndex("Access.Registrations", new[] { "ClientUserGuid" });
            DropIndex("Access.Registrations", new[] { "ShopGuid" });
            DropIndex("Access.Registrations", new[] { "AccountGuid" });
            DropIndex("Access.Registrations", new[] { "RegistrationShopId" });
            DropIndex("Access.Registrations", new[] { "RegistrationAccountId" });
            DropIndex("Access.Registrations", new[] { "RegistrationUserId" });
            DropTable("Access.RegistrationUsers");
            DropTable("Access.RegistrationShops");
            DropTable("Access.RegistrationAccounts");
            DropTable("Access.Registrations");

            this.DropObjectIfExists(DropObjectType.Procedure, "Notification", "usp_GetRegistrationNotification");
            this.DropObjectIfExists(DropObjectType.Procedure, "Notification", "usp_GetRegistrationWelcomeNotification");

            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetRegistrationGrid");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_CompleteRegistration");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetRegistrationOptions");

            Sql("DELETE FROM Notification.Templates WHERE NotificationTemplateId > 7;");
        }
    }
}
