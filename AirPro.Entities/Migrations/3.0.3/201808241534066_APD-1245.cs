namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1245 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "AllowScanAnalysisAutoClose", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Access.Shops", "AllowScanAnalysisAutoClose");
        }
    }
}
