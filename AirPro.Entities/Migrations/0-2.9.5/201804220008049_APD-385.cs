namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD385 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Diagnostic.Results",
                c => new
                    {
                        ResultId = c.Int(nullable: false, identity: true),
                        DiagnosticToolId = c.Int(nullable: false),
                        RequestId = c.Int(),
                        ScanDateTime = c.DateTime(),
                        CustomerFirstName = c.String(maxLength: 50),
                        CustomerLastName = c.String(maxLength: 50),
                        CustomerRo = c.String(maxLength: 50),
                        ShopName = c.String(maxLength: 150),
                        ShopAddress = c.String(maxLength: 150),
                        ShopEmail = c.String(maxLength: 150),
                        ShopFax = c.String(maxLength: 50),
                        ShopPhone = c.String(maxLength: 50),
                        VehicleVin = c.String(maxLength: 50),
                        VehicleMake = c.String(maxLength: 50),
                        VehicleModel = c.String(maxLength: 100),
                        VehicleYear = c.String(maxLength: 10),
                        TestabilityIssues = c.String(),
                        DeletedInd = c.Boolean(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ResultId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Diagnostic.Tools", t => t.DiagnosticToolId)
                .ForeignKey("Scan.Requests", t => t.RequestId)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.DiagnosticToolId)
                .Index(t => t.RequestId)
                .Index(t => t.VehicleVin)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Diagnostic.Controllers",
                c => new
                    {
                        ControllerId = c.Int(nullable: false, identity: true),
                        ResultId = c.Int(nullable: false),
                        ControllerName = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ControllerId)
                .ForeignKey("Diagnostic.Results", t => t.ResultId, cascadeDelete: true)
                .Index(t => t.ResultId);
            
            CreateTable(
                "Diagnostic.FreezeFrames",
                c => new
                    {
                        FreezeFrameId = c.Int(nullable: false, identity: true),
                        ControllerId = c.Int(nullable: false),
                        FreezeFrameTroubleCode = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.FreezeFrameId)
                .ForeignKey("Diagnostic.Controllers", t => t.ControllerId, cascadeDelete: true)
                .Index(t => t.ControllerId);
            
            CreateTable(
                "Diagnostic.FreezeFrameSensorGroups",
                c => new
                    {
                        FreezeFrameGroupId = c.Int(nullable: false, identity: true),
                        FreezeFrameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FreezeFrameGroupId)
                .ForeignKey("Diagnostic.FreezeFrames", t => t.FreezeFrameId, cascadeDelete: true)
                .Index(t => t.FreezeFrameId);
            
            CreateTable(
                "Diagnostic.FreezeFrameSensors",
                c => new
                    {
                        SensorId = c.Int(nullable: false, identity: true),
                        FreezeFrameGroupId = c.Int(nullable: false),
                        SensorName = c.String(maxLength: 100),
                        SensorUnit = c.String(maxLength: 20),
                        SensorValue = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.SensorId)
                .ForeignKey("Diagnostic.FreezeFrameSensorGroups", t => t.FreezeFrameGroupId, cascadeDelete: true)
                .Index(t => t.FreezeFrameGroupId);
            
            CreateTable(
                "Diagnostic.TroubleCodes",
                c => new
                    {
                        ToubleCodeId = c.Int(nullable: false, identity: true),
                        ControllerId = c.Int(nullable: false),
                        TroubleCode = c.String(maxLength: 20),
                        TroubleCodeDescription = c.String(maxLength: 1000),
                        TroubleCodeInformation = c.String(),
                    })
                .PrimaryKey(t => t.ToubleCodeId)
                .ForeignKey("Diagnostic.Controllers", t => t.ControllerId, cascadeDelete: true)
                .Index(t => t.ControllerId);
            
            CreateTable(
                "Diagnostic.Tools",
                c => new
                    {
                        DiagnosticToolId = c.Int(nullable: false, identity: true),
                        DiagnosticToolName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DiagnosticToolId);
            
            CreateTable(
                "Diagnostic.UploadFileTypes",
                c => new
                    {
                        UploadFileTypeId = c.Int(nullable: false, identity: true),
                        UploadFileTypeName = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.UploadFileTypeId);
            
            CreateTable(
                "Diagnostic.Uploads",
                c => new
                    {
                        UploadId = c.Int(nullable: false, identity: true),
                        ResultId = c.Int(),
                        UploadFileTypeId = c.Int(nullable: false),
                        UploadText = c.String(),
                    })
                .PrimaryKey(t => t.UploadId)
                .ForeignKey("Diagnostic.Results", t => t.ResultId)
                .ForeignKey("Diagnostic.UploadFileTypes", t => t.UploadFileTypeId)
                .Index(t => t.ResultId)
                .Index(t => t.UploadFileTypeId);

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Repair' AND TABLE_NAME = 'vwScanResultCounts') DROP VIEW Repair.vwScanResultCounts;

                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Scan' AND ROUTINE_NAME = 'f_DTCResults') DROP FUNCTION Scan.f_DTCResults;
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Scan' AND ROUTINE_NAME = 'f_FFResults') DROP FUNCTION Scan.f_FFResults;
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Scan' AND ROUTINE_NAME = 'f_ScanResult') DROP FUNCTION Scan.f_ScanResult;
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Scan' AND ROUTINE_NAME = 'f_TestIssues') DROP FUNCTION Scan.f_TestIssues;

                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'ScanDTCResults')
                        AND EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Scan' AND TABLE_NAME = 'DTCResults') DROP TABLE Backups.ScanDTCResults;
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'ScanFFInfoes') DROP TABLE Backups.ScanFFInfoes;
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'ScanInfoes') DROP TABLE Backups.ScanInfoes;
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'ScanTestIssues')
                        AND EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Scan' AND TABLE_NAME = 'TestIssues') DROP TABLE Backups.ScanTestIssues;
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'ScanUploads') DROP TABLE Backups.ScanUploads;

                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_ProcessScan') DROP PROCEDURE Scan.usp_ProcessScan;
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_GetRequestGridByUser') DROP PROCEDURE Scan.usp_GetRequestGridByUser;

                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_GetRepairsByUser') DROP PROCEDURE Repair.usp_GetRepairsByUser;

                    IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Backups') EXEC sp_sqlexec 'CREATE SCHEMA Backups AUTHORIZATION dbo;';

                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Scan' AND TABLE_NAME = 'DTCResults') ALTER SCHEMA Backups TRANSFER Scan.DTCResults;
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'DTCResults') EXEC sp_rename 'Backups.DTCResults', 'ScanDTCResults';

                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Scan' AND TABLE_NAME = 'FFResults') ALTER SCHEMA Backups TRANSFER Scan.FFResults;
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'FFResults') EXEC sp_rename 'Backups.FFResults', 'ScanFFResults';

                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Scan' AND TABLE_NAME = 'Results') ALTER SCHEMA Backups TRANSFER Scan.Results;
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'Results') EXEC sp_rename 'Backups.Results', 'ScanResults';

                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Scan' AND TABLE_NAME = 'TestIssues') ALTER SCHEMA Backups TRANSFER Scan.TestIssues;
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'TestIssues') EXEC sp_rename 'Backups.TestIssues', 'ScanTestIssues';

                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Scan' AND TABLE_NAME = 'UploadXmls') ALTER SCHEMA Backups TRANSFER Scan.UploadXmls;
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'UploadXmls') EXEC sp_rename 'Backups.UploadXmls', 'ScanUploadXmls';");

            Sql(@"GRANT SELECT, INSERT ON SCHEMA::Diagnostic TO AirProJobs");
            Sql(@"GRANT EXECUTE ON Access.usp_GetServiceUser TO AirProJobs;");
        }
        
        public override void Down()
        {
            DropForeignKey("Diagnostic.Uploads", "UploadFileTypeId", "Diagnostic.UploadFileTypes");
            DropForeignKey("Diagnostic.Uploads", "ResultId", "Diagnostic.Results");
            DropForeignKey("Diagnostic.Results", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Diagnostic.Results", "RequestId", "Scan.Requests");
            DropForeignKey("Diagnostic.Results", "DiagnosticToolId", "Diagnostic.Tools");
            DropForeignKey("Diagnostic.Results", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Diagnostic.Controllers", "ResultId", "Diagnostic.Results");
            DropForeignKey("Diagnostic.TroubleCodes", "ControllerId", "Diagnostic.Controllers");
            DropForeignKey("Diagnostic.FreezeFrames", "ControllerId", "Diagnostic.Controllers");
            DropForeignKey("Diagnostic.FreezeFrameSensorGroups", "FreezeFrameId", "Diagnostic.FreezeFrames");
            DropForeignKey("Diagnostic.FreezeFrameSensors", "FreezeFrameGroupId", "Diagnostic.FreezeFrameSensorGroups");
            DropIndex("Diagnostic.Uploads", new[] { "UploadFileTypeId" });
            DropIndex("Diagnostic.Uploads", new[] { "ResultId" });
            DropIndex("Diagnostic.TroubleCodes", new[] { "ControllerId" });
            DropIndex("Diagnostic.FreezeFrameSensors", new[] { "FreezeFrameGroupId" });
            DropIndex("Diagnostic.FreezeFrameSensorGroups", new[] { "FreezeFrameId" });
            DropIndex("Diagnostic.FreezeFrames", new[] { "ControllerId" });
            DropIndex("Diagnostic.Controllers", new[] { "ResultId" });
            DropIndex("Diagnostic.Results", new[] { "UpdatedByUserGuid" });
            DropIndex("Diagnostic.Results", new[] { "CreatedByUserGuid" });
            DropIndex("Diagnostic.Results", new[] { "VehicleVin" });
            DropIndex("Diagnostic.Results", new[] { "RequestId" });
            DropIndex("Diagnostic.Results", new[] { "DiagnosticToolId" });
            DropTable("Diagnostic.Uploads");
            DropTable("Diagnostic.UploadFileTypes");
            DropTable("Diagnostic.Tools");
            DropTable("Diagnostic.TroubleCodes");
            DropTable("Diagnostic.FreezeFrameSensors");
            DropTable("Diagnostic.FreezeFrameSensorGroups");
            DropTable("Diagnostic.FreezeFrames");
            DropTable("Diagnostic.Controllers");
            DropTable("Diagnostic.Results");
        }
    }
}
