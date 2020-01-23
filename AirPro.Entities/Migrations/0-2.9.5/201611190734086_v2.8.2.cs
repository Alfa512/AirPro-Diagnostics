namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v282 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("Scan.Reports", "TechnicianNotes", "ReportNotes");
            AddColumn("Scan.Reports", "TechnicianNotes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Scan.Reports", "TechnicianNotes");
            RenameColumn("Scan.Reports", "ReportNotes", "TechnicianNotes");
        }
    }
}
