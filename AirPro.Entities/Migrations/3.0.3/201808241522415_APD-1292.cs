namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1292 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Billing.Cycles",
                c => new
                    {
                        CycleId = c.Int(nullable: false, identity: true),
                        CycleName = c.String(),
                    })
                .PrimaryKey(t => t.CycleId);
            
            AddColumn("Access.Shops", "BillingCycleId", c => c.Int());
            CreateIndex("Access.Shops", "BillingCycleId");
            AddForeignKey("Access.Shops", "BillingCycleId", "Billing.Cycles", "CycleId");

            Sql("INSERT INTO Billing.Cycles (CycleName) VALUES('Weekly')");
            Sql("INSERT INTO Billing.Cycles (CycleName) VALUES('Bi-Weekly')");
            Sql("INSERT INTO Billing.Cycles (CycleName) VALUES('Monthly')");
        }
        
        public override void Down()
        {
            DropForeignKey("Access.Shops", "BillingCycleId", "Billing.Cycles");
            DropIndex("Access.Shops", new[] { "BillingCycleId" });
            DropColumn("Access.Shops", "BillingCycleId");
            DropTable("Billing.Cycles");
        }
    }
}
