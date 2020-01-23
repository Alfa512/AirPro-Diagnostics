namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_7_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Users", "ShopBillingNotification", c => c.Boolean(nullable: false));
            AddColumn("Access.Users", "ShopReportNotification", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Access.Users", "ShopReportNotification");
            DropColumn("Access.Users", "ShopBillingNotification");
        }
    }
}
