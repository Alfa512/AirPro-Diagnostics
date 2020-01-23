namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD573 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Billing.EstimatePlans",
                c => new
                    {
                        EstimatePlanId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ActiveInd = c.Boolean(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.EstimatePlanId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Billing.EstimatePlanVehicles",
                c => new
                    {
                        EstimatePlanId = c.Int(nullable: false),
                        VehicleMakeId = c.Int(nullable: false),
                        CompletionCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.EstimatePlanId, t.VehicleMakeId })
                .ForeignKey("Billing.EstimatePlans", t => t.EstimatePlanId)
                .ForeignKey("Repair.VehicleMakes", t => t.VehicleMakeId)
                .Index(t => t.EstimatePlanId)
                .Index(t => t.VehicleMakeId);
        }
        
        public override void Down()
        {
            DropForeignKey("Billing.EstimatePlans", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Billing.EstimatePlanVehicles", "VehicleMakeId", "Repair.VehicleMakes");
            DropForeignKey("Billing.EstimatePlanVehicles", "EstimatePlanId", "Billing.EstimatePlans");
            DropForeignKey("Billing.EstimatePlans", "CreatedByUserGuid", "Access.Users");
            DropIndex("Billing.EstimatePlanVehicles", new[] { "VehicleMakeId" });
            DropIndex("Billing.EstimatePlanVehicles", new[] { "EstimatePlanId" });
            DropIndex("Billing.EstimatePlans", new[] { "UpdatedByUserGuid" });
            DropIndex("Billing.EstimatePlans", new[] { "CreatedByUserGuid" });
            DropTable("Billing.EstimatePlanVehicles");
            DropTable("Billing.EstimatePlans");
        }
    }
}
