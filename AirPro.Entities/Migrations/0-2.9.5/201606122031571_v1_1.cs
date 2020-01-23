namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Scan.TestIssues", "SCAN_ID", "Scan.Results");
            DropForeignKey("Scan.Results", "SCAN_UPLOAD_ID", "Scan.Uploads");
            DropIndex("Scan.Results", new[] { "SCAN_UPLOAD_ID" });
            DropIndex("Scan.TestIssues", new[] { "SCAN_ID" });
            RenameColumn(table: "Scan.Requests", name: "ScanReport_ReportID", newName: "ReportID");
            RenameIndex(table: "Scan.Requests", name: "IX_ScanReport_ReportID", newName: "IX_ReportID");
            AddColumn("Scan.Reports", "TechnicianNotes", c => c.String());
            AlterColumn("Scan.Results", "SCAN_UPLOAD_ID", c => c.Int(nullable: false));
            AlterColumn("Scan.TestIssues", "SCAN_ID", c => c.Int(nullable: false));
            CreateIndex("Scan.Results", "SCAN_UPLOAD_ID");
            CreateIndex("Scan.TestIssues", "SCAN_ID");
            AddForeignKey("Scan.TestIssues", "SCAN_ID", "Scan.Results", "SCAN_ID", cascadeDelete: true);
            AddForeignKey("Scan.Results", "SCAN_UPLOAD_ID", "Scan.Uploads", "SCAN_UPLOAD_ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Scan.Results", "SCAN_UPLOAD_ID", "Scan.Uploads");
            DropForeignKey("Scan.TestIssues", "SCAN_ID", "Scan.Results");
            DropIndex("Scan.TestIssues", new[] { "SCAN_ID" });
            DropIndex("Scan.Results", new[] { "SCAN_UPLOAD_ID" });
            AlterColumn("Scan.TestIssues", "SCAN_ID", c => c.Int());
            AlterColumn("Scan.Results", "SCAN_UPLOAD_ID", c => c.Int());
            DropColumn("Scan.Reports", "TechnicianNotes");
            RenameIndex(table: "Scan.Requests", name: "IX_ReportID", newName: "IX_ScanReport_ReportID");
            RenameColumn(table: "Scan.Requests", name: "ReportID", newName: "ScanReport_ReportID");
            CreateIndex("Scan.TestIssues", "SCAN_ID");
            CreateIndex("Scan.Results", "SCAN_UPLOAD_ID");
            AddForeignKey("Scan.Results", "SCAN_UPLOAD_ID", "Scan.Uploads", "SCAN_UPLOAD_ID");
            AddForeignKey("Scan.TestIssues", "SCAN_ID", "Scan.Results", "SCAN_ID");
        }
    }
}
