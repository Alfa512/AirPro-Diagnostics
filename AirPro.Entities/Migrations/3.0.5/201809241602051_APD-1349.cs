namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1349 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Inventory.AirProTools", "J2534Brand", c => c.Int());
            AddColumn("Inventory.AirProTools", "J2534Model", c => c.Int());
            AddColumn("Inventory.AirProTools", "J2534Serial", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("Inventory.AirProTools", "J2534Serial");
            DropColumn("Inventory.AirProTools", "J2534Model");
            DropColumn("Inventory.AirProTools", "J2534Brand");
        }
    }
}
