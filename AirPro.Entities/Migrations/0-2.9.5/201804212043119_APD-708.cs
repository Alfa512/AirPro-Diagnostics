namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD708 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "AllowAutoRepairClose", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Access.Shops", "AllowAutoRepairClose");
        }
    }
}
