namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_7_6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.Results", "ActiveInd", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("Scan.Results", "ActiveInd");
        }
    }
}
