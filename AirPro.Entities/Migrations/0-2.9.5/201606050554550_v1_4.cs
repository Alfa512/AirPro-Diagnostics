namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1_4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Scan.Requests", "ScanResult_SCAN_ID", "Scan.Results");
            DropForeignKey("Scan.Requests", "Repair_RepairOrderID", "Repair.Orders");
            DropIndex("Scan.Requests", new[] { "Repair_RepairOrderID" });
            DropIndex("Scan.Requests", new[] { "ScanResult_SCAN_ID" });
            CreateTable(
                "Scan.Reports",
                c => new
                    {
                        ReportID = c.Int(nullable: false, identity: true),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                        UpdatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReportID)
                .ForeignKey("Access.Users", t => t.CreatedBy_Id, cascadeDelete: true)
                .ForeignKey("Access.Users", t => t.UpdatedBy_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);
            
            AddColumn("Repair.Photos", "ContentType", c => c.String());
            AddColumn("Scan.Uploads", "Request_RequestID", c => c.Int());
            AddColumn("Scan.Requests", "ScanReport_ReportID", c => c.Int());
            AlterColumn("Scan.Requests", "Repair_RepairOrderID", c => c.Int(nullable: false));
            CreateIndex("Scan.Requests", "Repair_RepairOrderID");
            CreateIndex("Scan.Requests", "ScanReport_ReportID");
            CreateIndex("Scan.Uploads", "Request_RequestID");
            AddForeignKey("Scan.Requests", "ScanReport_ReportID", "Scan.Reports", "ReportID");
            AddForeignKey("Scan.Uploads", "Request_RequestID", "Scan.Requests", "RequestID");
            AddForeignKey("Scan.Requests", "Repair_RepairOrderID", "Repair.Orders", "RepairOrderID", cascadeDelete: false);
            DropColumn("Scan.Requests", "ScanResult_SCAN_ID");
        }
        
        public override void Down()
        {
            AddColumn("Scan.Requests", "ScanResult_SCAN_ID", c => c.Int());
            DropForeignKey("Scan.Requests", "Repair_RepairOrderID", "Repair.Orders");
            DropForeignKey("Scan.Uploads", "Request_RequestID", "Scan.Requests");
            DropForeignKey("Scan.Requests", "ScanReport_ReportID", "Scan.Reports");
            DropForeignKey("Scan.Reports", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Scan.Reports", "CreatedBy_Id", "Access.Users");
            DropIndex("Scan.Uploads", new[] { "Request_RequestID" });
            DropIndex("Scan.Reports", new[] { "UpdatedBy_Id" });
            DropIndex("Scan.Reports", new[] { "CreatedBy_Id" });
            DropIndex("Scan.Requests", new[] { "ScanReport_ReportID" });
            DropIndex("Scan.Requests", new[] { "Repair_RepairOrderID" });
            AlterColumn("Scan.Requests", "Repair_RepairOrderID", c => c.Int());
            DropColumn("Scan.Requests", "ScanReport_ReportID");
            DropColumn("Scan.Uploads", "Request_RequestID");
            DropColumn("Repair.Photos", "ContentType");
            DropTable("Scan.Reports");
            CreateIndex("Scan.Requests", "ScanResult_SCAN_ID");
            CreateIndex("Scan.Requests", "Repair_RepairOrderID");
            AddForeignKey("Scan.Requests", "Repair_RepairOrderID", "Repair.Orders", "RepairOrderID");
            AddForeignKey("Scan.Requests", "ScanResult_SCAN_ID", "Scan.Results", "SCAN_ID");
        }
    }
}
