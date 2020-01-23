namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD132 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "Repair.OrderPointOfImpacts",
                    c => new
                    {
                        OrderID = c.Int(nullable: false),
                        PointOfImpactId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderID, t.PointOfImpactId })
                .ForeignKey("Repair.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("Repair.PointOfImpacts", t => t.PointOfImpactId, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.PointOfImpactId);

            CreateTable(
                    "Repair.PointOfImpacts",
                    c => new
                    {
                        PointOfImpactId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.PointOfImpactId);

        }

        public override void Down()
        {
            DropForeignKey("Repair.OrderPointOfImpacts", "PointOfImpactId", "Repair.PointOfImpacts");
            DropForeignKey("Repair.OrderPointOfImpacts", "OrderID", "Repair.Orders");
            DropIndex("Repair.OrderPointOfImpacts", new[] { "PointOfImpactId" });
            DropIndex("Repair.OrderPointOfImpacts", new[] { "OrderID" });
            DropTable("Repair.PointOfImpacts");
            DropTable("Repair.OrderPointOfImpacts");
        }
    }
}
