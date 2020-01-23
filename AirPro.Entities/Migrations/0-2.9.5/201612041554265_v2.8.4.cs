namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v284 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Repair.Orders", "DrivableInd", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("Scan.Reports", "CodesClearedInd", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("Scan.Reports", "DangerCodeCount", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("Scan.Reports", "WarningCodeCount", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("Scan.Reports", "CautionCodeCount", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("Scan.Reports", "OtherCodeCount", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("Scan.Reports", "OtherCodeCount");
            DropColumn("Scan.Reports", "CautionCodeCount");
            DropColumn("Scan.Reports", "WarningCodeCount");
            DropColumn("Scan.Reports", "DangerCodeCount");
            DropColumn("Scan.Reports", "CodesClearedInd");
            DropColumn("Repair.Orders", "DrivableInd");
        }
    }
}
