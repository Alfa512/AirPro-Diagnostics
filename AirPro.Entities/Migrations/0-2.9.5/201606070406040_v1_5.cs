namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.Requests", "OtherWarningInfo", c => c.String());
            AddColumn("Scan.Requests", "ProblemDescription", c => c.String());
            AddColumn("Scan.Requests", "Notes", c => c.String());
            AlterColumn("Repair.Orders", "UpdatedDt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("Repair.Photos", "UpdatedDt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("Scan.Requests", "UpdatedDt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("Scan.Reports", "UpdatedDt", c => c.DateTimeOffset(precision: 7));
            DropColumn("Repair.Orders", "OtherWarningInfo");
            DropColumn("Repair.Orders", "ProblemDescription");
            DropColumn("Repair.Orders", "Notes");
        }
        
        public override void Down()
        {
            AddColumn("Repair.Orders", "Notes", c => c.String());
            AddColumn("Repair.Orders", "ProblemDescription", c => c.String());
            AddColumn("Repair.Orders", "OtherWarningInfo", c => c.String());
            AlterColumn("Scan.Reports", "UpdatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("Scan.Requests", "UpdatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("Repair.Photos", "UpdatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("Repair.Orders", "UpdatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("Scan.Requests", "Notes");
            DropColumn("Scan.Requests", "ProblemDescription");
            DropColumn("Scan.Requests", "OtherWarningInfo");
        }
    }
}
