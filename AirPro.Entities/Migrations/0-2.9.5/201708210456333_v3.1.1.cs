namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v311 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Accounts", "DiscountPercentage", c => c.Int(nullable: false));
            AddColumn("Access.Shops", "DiscountPercentage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Access.Shops", "DiscountPercentage");
            DropColumn("Access.Accounts", "DiscountPercentage");
        }
    }
}
