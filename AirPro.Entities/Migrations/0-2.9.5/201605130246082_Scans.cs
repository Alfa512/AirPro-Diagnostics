namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Scans : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Scan.DTCResults",
                c => new
                    {
                        DTC_RESULT_ID = c.Int(nullable: false, identity: true),
                        SCAN_ID = c.Int(nullable: false),
                        CONTROLLER_NAME = c.String(),
                        DTC_DESCRIPTION = c.String(),
                    })
                .PrimaryKey(t => t.DTC_RESULT_ID)
                .ForeignKey("Scan.Infoes", t => t.SCAN_ID, cascadeDelete: true)
                .Index(t => t.SCAN_ID);
            
            CreateTable(
                "Scan.Infoes",
                c => new
                    {
                        SCAN_ID = c.Int(nullable: false, identity: true),
                        SCAN_UPLOAD_ID = c.Int(),
                        VEHICLE_VIN = c.String(),
                        VEHICLE_MAKE = c.String(),
                        VEHICLE_MODEL = c.String(),
                        VEHICLE_YEAR = c.String(),
                        MIL_STATE = c.String(),
                        SHOP_NAME = c.String(),
                        SHOP_ADDRESS = c.String(),
                        SHOP_PHONE = c.String(),
                        SHOP_FAX = c.String(),
                        SHOP_EMAIL = c.String(),
                        SCAN_DT = c.DateTime(),
                    })
                .PrimaryKey(t => t.SCAN_ID)
                .ForeignKey("Scan.Uploads", t => t.SCAN_UPLOAD_ID)
                .Index(t => t.SCAN_UPLOAD_ID);
            
            CreateTable(
                "Scan.FFInfoes",
                c => new
                    {
                        FF_INFO_ID = c.Int(nullable: false, identity: true),
                        SCAN_ID = c.Int(nullable: false),
                        CONTROLLER_NAME = c.String(),
                        FF_DTC = c.String(),
                        FF_SENSOR_ONE = c.String(),
                        FF_VALUE_ONE = c.String(),
                        FF_UNITS_ONE = c.String(),
                        FF_SENSOR_TWO = c.String(),
                        FF_VALUE_TWO = c.String(),
                        FF_UNITS_TWO = c.String(),
                    })
                .PrimaryKey(t => t.FF_INFO_ID)
                .ForeignKey("Scan.Infoes", t => t.SCAN_ID, cascadeDelete: true)
                .Index(t => t.SCAN_ID);
            
            CreateTable(
                "Scan.TestIssues",
                c => new
                    {
                        SCAN_ISSUE_ID = c.Int(nullable: false, identity: true),
                        SCAN_ID = c.Int(),
                        TESTABILITY_ISSUE = c.String(),
                    })
                .PrimaryKey(t => t.SCAN_ISSUE_ID)
                .ForeignKey("Scan.Infoes", t => t.SCAN_ID)
                .Index(t => t.SCAN_ID);
            
            CreateTable(
                "Scan.Uploads",
                c => new
                    {
                        SCAN_UPLOAD_ID = c.Int(nullable: false, identity: true),
                        UPLOAD_XML = c.String(storeType: "xml"),
                        UPLOAD_DT = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.SCAN_UPLOAD_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Scan.Infoes", "SCAN_UPLOAD_ID", "Scan.Uploads");
            DropForeignKey("Scan.TestIssues", "SCAN_ID", "Scan.Infoes");
            DropForeignKey("Scan.FFInfoes", "SCAN_ID", "Scan.Infoes");
            DropForeignKey("Scan.DTCResults", "SCAN_ID", "Scan.Infoes");
            DropIndex("Scan.TestIssues", new[] { "SCAN_ID" });
            DropIndex("Scan.FFInfoes", new[] { "SCAN_ID" });
            DropIndex("Scan.Infoes", new[] { "SCAN_UPLOAD_ID" });
            DropIndex("Scan.DTCResults", new[] { "SCAN_ID" });
            DropTable("Scan.Uploads");
            DropTable("Scan.TestIssues");
            DropTable("Scan.FFInfoes");
            DropTable("Scan.Infoes");
            DropTable("Scan.DTCResults");
        }
    }
}
