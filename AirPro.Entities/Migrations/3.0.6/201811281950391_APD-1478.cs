namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;
    
    public partial class APD1478 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Access.ShopRequestTypes",
                c => new
                    {
                        ShopGuid = c.Guid(nullable: false),
                        RequestTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShopGuid, t.RequestTypeId })
                .ForeignKey("Scan.RequestTypes", t => t.RequestTypeId, cascadeDelete: true)
                .ForeignKey("Access.Shops", t => t.ShopGuid, cascadeDelete: true)
                .Index(t => t.ShopGuid)
                .Index(t => t.RequestTypeId);

            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveShop");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetShops");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetShopDisplayList");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");

            Sql(@"INSERT INTO [Access].[ShopRequestTypes] ([ShopGuid], [RequestTypeId])
                    SELECT ShopGuid, RequestTypeId
                    FROM (SELECT ShopGuid, AllowDemoScan, AllowScanAnalysis, AllowSelfScan FROM Access.Shops) as Shop,
                        (SELECT RequestTypeId, TypeName FROM Scan.RequestTypes) as Type
                    WHERE (RequestTypeId <> 6 OR (AllowSelfScan = 1 AND RequestTypeId = 6))
                        AND (RequestTypeId <> 8 OR (AllowDemoScan = 1 AND RequestTypeId = 8))
                        AND (RequestTypeId <> 7 OR (AllowScanAnalysis = 1 AND RequestTypeId = 7))");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveShop");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetShops");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetShopDisplayList");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");

            DropForeignKey("Access.ShopRequestTypes", "ShopGuid", "Access.Shops");
            DropForeignKey("Access.ShopRequestTypes", "RequestTypeId", "Scan.RequestTypes");
            DropIndex("Access.ShopRequestTypes", new[] { "RequestTypeId" });
            DropIndex("Access.ShopRequestTypes", new[] { "ShopGuid" });
            DropTable("Access.ShopRequestTypes");
        }
    }
}
