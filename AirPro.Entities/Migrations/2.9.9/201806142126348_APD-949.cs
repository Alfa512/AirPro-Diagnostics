namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD949 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Service.MitchellRequests",
                c => new
                    {
                        RequestId = c.Int(nullable: false, identity: true),
                        ShopGuid = c.Guid(),
                        MitchellRecId = c.String(maxLength: 128),
                        VehicleVIN = c.String(maxLength: 128),
                        ShopRONum = c.String(maxLength: 128),
                        InsuranceCoName = c.String(maxLength: 128),
                        Odometer = c.Int(),
                        DrivableInd = c.Boolean(nullable: false),
                        AirBagsDeployedInd = c.Boolean(nullable: false),
                        RequestBody = c.String(),
                        RequestDt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("Access.Shops", t => t.ShopGuid)
                .Index(t => t.ShopGuid)
                .Index(t => t.MitchellRecId)
                .Index(t => t.VehicleVIN);
            
            AddColumn("Repair.Orders", "MitchellRequestId", c => c.Int());
            CreateIndex("Repair.Orders", "MitchellRequestId");
            AddForeignKey("Repair.Orders", "MitchellRequestId", "Service.MitchellRequests", "RequestId");
        }
        
        public override void Down()
        {
            DropForeignKey("Repair.Orders", "MitchellRequestId", "Service.MitchellRequests");
            DropForeignKey("Service.MitchellRequests", "ShopGuid", "Access.Shops");
            DropIndex("Service.MitchellRequests", new[] { "VehicleVIN" });
            DropIndex("Service.MitchellRequests", new[] { "MitchellRecId" });
            DropIndex("Service.MitchellRequests", new[] { "ShopGuid" });
            DropIndex("Repair.Orders", new[] { "MitchellRequestId" });
            DropColumn("Repair.Orders", "MitchellRequestId");
            DropTable("Service.MitchellRequests");
        }
    }
}
