namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v320 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Billing.PricingPlans",
                c => new
                    {
                        PricingPlanId = c.Int(nullable: false, identity: true),
                        PricingPlanName = c.String(),
                        PricingPlanDescription = c.String(),
                        PricingPlanActiveInd = c.Boolean(nullable: false, defaultValueSql: "1"),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.PricingPlanId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid, cascadeDelete: true)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Billing.PricingPlanRequestTypes",
                c => new
                    {
                        PricingPlanRequestTypeId = c.Int(nullable: false, identity: true),
                        PricingPlanId = c.Int(nullable: false),
                        RequestTypeId = c.Int(nullable: false),
                        VehicleMakeTypeId = c.Int(nullable: false),
                        LineItemCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PricingPlanRequestTypeId)
                .ForeignKey("Billing.PricingPlans", t => t.PricingPlanId, cascadeDelete: true)
                .ForeignKey("Scan.RequestTypes", t => t.RequestTypeId, cascadeDelete: true)
                .ForeignKey("Repair.VehicleMakeTypes", t => t.VehicleMakeTypeId, cascadeDelete: true)
                .Index(t => t.PricingPlanId)
                .Index(t => t.RequestTypeId)
                .Index(t => t.VehicleMakeTypeId);
            
            CreateTable(
                "Billing.PricingPlanWorkTypes",
                c => new
                    {
                        PricingPlanWorkTypeId = c.Int(nullable: false, identity: true),
                        PricingPlanId = c.Int(nullable: false),
                        WorkTypeId = c.Int(nullable: false),
                        VehicleMakeTypeId = c.Int(nullable: false),
                        LineItemCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PricingPlanWorkTypeId)
                .ForeignKey("Billing.PricingPlans", t => t.PricingPlanId, cascadeDelete: true)
                .ForeignKey("Repair.VehicleMakeTypes", t => t.VehicleMakeTypeId, cascadeDelete: true)
                .ForeignKey("Scan.WorkTypes", t => t.WorkTypeId, cascadeDelete: true)
                .Index(t => t.PricingPlanId)
                .Index(t => t.WorkTypeId)
                .Index(t => t.VehicleMakeTypeId);

            AddColumn("Access.Shops", "PricingPlanId", c => c.Int());
            CreateIndex("Access.Shops", "PricingPlanId");
            AddForeignKey("Access.Shops", "PricingPlanId", "Billing.PricingPlans", "PricingPlanId");

            Sql(@"INSERT INTO Billing.PricingPlans
                (
	                PricingPlanName
	                ,PricingPlanDescription
	                ,PricingPlanActiveInd
	                ,CreatedByUserGuid
	                ,CreatedDt
                )
                SELECT
	                'Default Plan' [PricingPlanName]
	                ,'Default Pricing Plan' [PricingPlanDescription]
	                ,1 [PricingPlanActiveInd]
	                ,'00000000-0000-0000-0000-000000000000' [CreatedByUserGuid]
	                ,GETUTCDATE() [CreatedDt]
                WHERE NOT EXISTS (SELECT 1 FROM Billing.PricingPlans)

                DECLARE @PricingPlanId INT = SCOPE_IDENTITY()

                UPDATE s
	                SET s.PricingPlanId = @PricingPlanId
                FROM Access.Shops s
                WHERE @PricingPlanId IS NOT NULL
	                AND s.PricingPlanId IS NULL");
        }
        
        public override void Down()
        {
            DropForeignKey("Access.Shops", "PricingPlanId", "Billing.PricingPlans");
            DropForeignKey("Billing.PricingPlanWorkTypes", "WorkTypeId", "Scan.WorkTypes");
            DropForeignKey("Billing.PricingPlanWorkTypes", "VehicleMakeTypeId", "Repair.VehicleMakeTypes");
            DropForeignKey("Billing.PricingPlanWorkTypes", "PricingPlanId", "Billing.PricingPlans");
            DropForeignKey("Billing.PricingPlans", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Billing.PricingPlanRequestTypes", "VehicleMakeTypeId", "Repair.VehicleMakeTypes");
            DropForeignKey("Billing.PricingPlanRequestTypes", "RequestTypeId", "Scan.RequestTypes");
            DropForeignKey("Billing.PricingPlanRequestTypes", "PricingPlanId", "Billing.PricingPlans");
            DropForeignKey("Billing.PricingPlans", "CreatedByUserGuid", "Access.Users");
            DropIndex("Billing.PricingPlanWorkTypes", new[] { "VehicleMakeTypeId" });
            DropIndex("Billing.PricingPlanWorkTypes", new[] { "WorkTypeId" });
            DropIndex("Billing.PricingPlanWorkTypes", new[] { "PricingPlanId" });
            DropIndex("Billing.PricingPlanRequestTypes", new[] { "VehicleMakeTypeId" });
            DropIndex("Billing.PricingPlanRequestTypes", new[] { "RequestTypeId" });
            DropIndex("Billing.PricingPlanRequestTypes", new[] { "PricingPlanId" });
            DropIndex("Billing.PricingPlans", new[] { "UpdatedByUserGuid" });
            DropIndex("Billing.PricingPlans", new[] { "CreatedByUserGuid" });
            DropIndex("Access.Shops", new[] { "PricingPlanId" });
            DropColumn("Access.Shops", "PricingPlanId");
            DropTable("Billing.PricingPlanWorkTypes");
            DropTable("Billing.PricingPlanRequestTypes");
            DropTable("Billing.PricingPlans");
        }
    }
}
