namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD698 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "ShopFixedPriceInd", c => c.Boolean(nullable: false));
            AddColumn("Access.Shops", "FirstScanCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Access.Shops", "AdditionalScanCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Access.Shops", "AdditionalScanCost");
            DropColumn("Access.Shops", "FirstScanCost");
            DropColumn("Access.Shops", "ShopFixedPriceInd");
        }
    }
}
