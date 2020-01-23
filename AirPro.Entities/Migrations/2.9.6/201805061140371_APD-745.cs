namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD745 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Access.ShopVehicleMakes",
                c => new
                    {
                        ShopId = c.Guid(nullable: false),
                        VehicleMakeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShopId, t.VehicleMakeId })
                .ForeignKey("Access.Shops", t => t.ShopId)
                .ForeignKey("Repair.VehicleMakes", t => t.VehicleMakeId)
                .Index(t => t.ShopId)
                .Index(t => t.VehicleMakeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Access.ShopVehicleMakes", "VehicleMakeId", "Repair.VehicleMakes");
            DropForeignKey("Access.ShopVehicleMakes", "ShopId", "Access.Shops");
            DropIndex("Access.ShopVehicleMakes", new[] { "VehicleMakeId" });
            DropIndex("Access.ShopVehicleMakes", new[] { "ShopId" });
            DropTable("Access.ShopVehicleMakes");
        }
    }
}
