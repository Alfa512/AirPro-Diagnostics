namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Shops : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Scan.Infoes", newName: "Results");
            RenameTable(name: "Scan.FFInfoes", newName: "FFResults");
            CreateTable(
                "Repair.Orders",
                c => new
                    {
                        RepairOrderID = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        ShopReferenceNumber = c.String(),
                        InsuranceCompany = c.String(),
                        InsuranceReferenceNumber = c.String(),
                        Odometer = c.String(),
                        AirBagsDeployed = c.Boolean(nullable: false),
                        OtherWarningInfo = c.String(),
                        ProblemDescription = c.String(),
                        Notes = c.String(),
                        Vehicle_VIN = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RepairOrderID)
                .ForeignKey("Repair.Vehicles", t => t.Vehicle_VIN)
                .Index(t => t.Vehicle_VIN);
            
            CreateTable(
                "Repair.Photos",
                c => new
                    {
                        PhotoID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FullImage = c.Binary(),
                        ThumbnailImage = c.Binary(),
                        UploadDt = c.DateTimeOffset(nullable: false, precision: 7),
                        Repair_RepairOrderID = c.Int(),
                    })
                .PrimaryKey(t => t.PhotoID)
                .ForeignKey("Repair.Orders", t => t.Repair_RepairOrderID)
                .Index(t => t.Repair_RepairOrderID);
            
            CreateTable(
                "Repair.Vehicles",
                c => new
                    {
                        VIN = c.String(nullable: false, maxLength: 128),
                        Make = c.String(),
                        Model = c.String(),
                        Year = c.String(),
                        TransmissionType = c.String(),
                    })
                .PrimaryKey(t => t.VIN);
            
            CreateTable(
                "Scan.Requests",
                c => new
                    {
                        RequestID = c.Int(nullable: false, identity: true),
                        TypeOfScan = c.Int(nullable: false),
                        CreatedDt = c.DateTime(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                        Repair_RepairOrderID = c.Int(),
                        ScanResult_SCAN_ID = c.Int(),
                    })
                .PrimaryKey(t => t.RequestID)
                .ForeignKey("Access.Users", t => t.CreatedBy_Id)
                .ForeignKey("Repair.Orders", t => t.Repair_RepairOrderID)
                .ForeignKey("Scan.Results", t => t.ScanResult_SCAN_ID)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Repair_RepairOrderID)
                .Index(t => t.ScanResult_SCAN_ID);
            
            CreateTable(
                "Access.Shops",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Phone = c.String(),
                        Fax = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        State = c.Int(nullable: false),
                        Zip = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("Access.Users", "FirstName", c => c.String());
            AddColumn("Access.Users", "LastName", c => c.String());
            AddColumn("Access.Users", "JobTitle", c => c.String());
            AddColumn("Access.Users", "Shop_ID", c => c.Int());
            CreateIndex("Access.Users", "Shop_ID");
            AddForeignKey("Access.Users", "Shop_ID", "Access.Shops", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("Scan.Requests", "ScanResult_SCAN_ID", "Scan.Results");
            DropForeignKey("Scan.Requests", "Repair_RepairOrderID", "Repair.Orders");
            DropForeignKey("Scan.Requests", "CreatedBy_Id", "Access.Users");
            DropForeignKey("Access.Users", "Shop_ID", "Access.Shops");
            DropForeignKey("Repair.Orders", "Vehicle_VIN", "Repair.Vehicles");
            DropForeignKey("Repair.Photos", "Repair_RepairOrderID", "Repair.Orders");
            DropIndex("Access.Users", new[] { "Shop_ID" });
            DropIndex("Scan.Requests", new[] { "ScanResult_SCAN_ID" });
            DropIndex("Scan.Requests", new[] { "Repair_RepairOrderID" });
            DropIndex("Scan.Requests", new[] { "CreatedBy_Id" });
            DropIndex("Repair.Photos", new[] { "Repair_RepairOrderID" });
            DropIndex("Repair.Orders", new[] { "Vehicle_VIN" });
            DropColumn("Access.Users", "Shop_ID");
            DropColumn("Access.Users", "JobTitle");
            DropColumn("Access.Users", "LastName");
            DropColumn("Access.Users", "FirstName");
            DropTable("Access.Shops");
            DropTable("Scan.Requests");
            DropTable("Repair.Vehicles");
            DropTable("Repair.Photos");
            DropTable("Repair.Orders");
            RenameTable(name: "Scan.FFResults", newName: "FFInfoes");
            RenameTable(name: "Scan.Results", newName: "Infoes");
        }
    }
}
