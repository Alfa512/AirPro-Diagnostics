namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD888 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "Billing.ShopVehicleMakesPricing",
                    c => new
                    {
                        ShopId = c.Guid(nullable: false),
                        VehicleMakeId = c.Int(nullable: false),
                        PricingPlanId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShopId, t.VehicleMakeId })
                .ForeignKey("Billing.PricingPlans", t => t.PricingPlanId)
                .ForeignKey("Access.Shops", t => t.ShopId)
                .ForeignKey("Repair.VehicleMakes", t => t.VehicleMakeId)
                .Index(t => t.ShopId)
                .Index(t => t.VehicleMakeId)
                .Index(t => t.PricingPlanId);
        }

        public override void Down()
        {
            DropForeignKey("Billing.ShopVehicleMakesPricing", "VehicleMakeId", "Repair.VehicleMakes");
            DropForeignKey("Billing.ShopVehicleMakesPricing", "ShopId", "Access.Shops");
            DropForeignKey("Billing.ShopVehicleMakesPricing", "PricingPlanId", "Billing.PricingPlans");
            DropIndex("Billing.ShopVehicleMakesPricing", new[] { "PricingPlanId" });
            DropIndex("Billing.ShopVehicleMakesPricing", new[] { "VehicleMakeId" });
            DropIndex("Billing.ShopVehicleMakesPricing", new[] { "ShopId" });
            DropTable("Billing.ShopVehicleMakesPricing");
        }
    }
}