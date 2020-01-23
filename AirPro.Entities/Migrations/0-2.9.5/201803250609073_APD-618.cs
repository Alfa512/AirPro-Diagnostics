namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD618 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "EstimatePlanId", c => c.Int());
            CreateIndex("Access.Shops", "EstimatePlanId");
            AddForeignKey("Access.Shops", "EstimatePlanId", "Billing.EstimatePlans", "EstimatePlanId");
        }
        
        public override void Down()
        {
            DropForeignKey("Access.Shops", "EstimatePlanId", "Billing.EstimatePlans");
            DropIndex("Access.Shops", new[] { "EstimatePlanId" });
            DropColumn("Access.Shops", "EstimatePlanId");
        }
    }
}
