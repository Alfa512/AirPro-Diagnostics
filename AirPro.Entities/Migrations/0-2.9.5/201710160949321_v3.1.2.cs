namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v312 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Service.CCCEstimates",
                c => new
                    {
                        EstimateId = c.Int(nullable: false, identity: true),
                        RequestGuid = c.Guid(),
                        AppId = c.Int(nullable: false),
                        Trigger = c.String(),
                        DocumentGuid = c.Guid(),
                        DocumentVersion = c.Int(),
                        DocumentStatus = c.String(),
                        ShopId = c.String(maxLength: 128),
                        ShopName = c.String(),
                        ShopRoNumber = c.String(),
                        VehicleVin = c.String(maxLength: 128),
                        VehicleYear = c.String(),
                        VehicleMake = c.String(),
                        VehicleModel = c.String(),
                        VehicleOdometer = c.String(),
                        VehicleDrivable = c.Boolean(),
                        InsuranceCompanyId = c.String(maxLength: 128),
                        InsuranceCompanyName = c.String(),
                        RawXml = c.String(nullable: false, storeType: "xml"),
                        ReceivedDt = c.DateTimeOffset(nullable: false, precision: 7, defaultValueSql: "GETUTCDATE()"),
                    })
                .PrimaryKey(t => t.EstimateId)
                .Index(t => t.RequestGuid)
                .Index(t => t.DocumentGuid)
                .Index(t => t.ShopId)
                .Index(t => t.VehicleVin)
                .Index(t => t.InsuranceCompanyId);
            
            AddColumn("Access.Shops", "CCCShopId", c => c.String(maxLength: 128));
            AddColumn("Repair.InsuranceCompanies", "CCCInsuranceCompanyId", c => c.String(maxLength: 128));
            AddColumn("Repair.Orders", "CCCDocumentGuid", c => c.Guid());
            CreateIndex("Access.Shops", "CCCShopId");
            CreateIndex("Repair.InsuranceCompanies", "CCCInsuranceCompanyId");
            CreateIndex("Repair.Orders", "CCCDocumentGuid");
        }
        
        public override void Down()
        {
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_CreateFromCCCEstimates') DROP PROCEDURE [Repair].[usp_CreateFromCCCEstimates];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Service' AND ROUTINE_NAME = 'usp_SaveCCCEstimate') DROP PROCEDURE [Service].[usp_SaveCCCEstimate];");
            DropIndex("Repair.Orders", new[] { "CCCDocumentGuid" });
            DropIndex("Repair.InsuranceCompanies", new[] { "CCCInsuranceCompanyId" });
            DropIndex("Service.CCCEstimates", new[] { "InsuranceCompanyId" });
            DropIndex("Service.CCCEstimates", new[] { "VehicleVin" });
            DropIndex("Service.CCCEstimates", new[] { "ShopId" });
            DropIndex("Service.CCCEstimates", new[] { "DocumentGuid" });
            DropIndex("Service.CCCEstimates", new[] { "RequestGuid" });
            DropIndex("Access.Shops", new[] { "CCCShopId" });
            DropColumn("Repair.Orders", "CCCDocumentGuid");
            DropColumn("Repair.InsuranceCompanies", "CCCInsuranceCompanyId");
            DropColumn("Access.Shops", "CCCShopId");
            DropTable("Service.CCCEstimates");
        }
    }
}
