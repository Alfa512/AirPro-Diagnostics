using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class APD1258 : DbMigration
    {
        public override void Up()
        {
            // Validate Backup Tables.
            Sql(@"IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'DiagnosticControllers')
	                THROW 50000, 'Backup Table [Backups].[DiagnosticControllers] Does NOT Exist.', 1;
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'DiagnosticTroubleCodes')
	                THROW 50000, 'Backup Table [Backups].[DiagnosticTroubleCodes] Does NOT Exist.', 1;
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'DiagnosticFreezeFrames')
	                THROW 50000, 'Backup Table [Backups].[DiagnosticFreezeFrames] Does NOT Exist.', 1;
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'DiagnosticFreezeFrameSensorGroups')
	                THROW 50000, 'Backup Table [Backups].[DiagnosticFreezeFrameSensorGroups] Does NOT Exist.', 1;
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'DiagnosticFreezeFrameSensors')
	                THROW 50000, 'Backup Table [Backups].[DiagnosticFreezeFrameSensors] Does NOT Exist.', 1;");

            DropForeignKey("Support.ConnectionLogs", "UserGuid", "Access.Users");
            DropForeignKey("Diagnostic.FreezeFrameSensors", "FreezeFrameGroupId", "Diagnostic.FreezeFrameSensorGroups");
            DropForeignKey("Diagnostic.FreezeFrameSensorGroups", "FreezeFrameId", "Diagnostic.FreezeFrames");
            DropForeignKey("Diagnostic.FreezeFrames", "ControllerId", "Diagnostic.Controllers");
            DropForeignKey("Diagnostic.TroubleCodes", "ControllerId", "Diagnostic.Controllers");
            DropForeignKey("Diagnostic.Controllers", "ResultId", "Diagnostic.Results");
            DropIndex("Support.ConnectionLogs", new[] { "UserGuid" });
            DropIndex("Support.ConnectionLogs", new[] { "ConnectionEndDt" });
            DropIndex("Diagnostic.Controllers", new[] { "ResultId" });
            DropIndex("Diagnostic.FreezeFrames", new[] { "ControllerId" });
            DropIndex("Diagnostic.FreezeFrameSensorGroups", new[] { "FreezeFrameId" });
            DropIndex("Diagnostic.FreezeFrameSensors", new[] { "FreezeFrameGroupId" });
            DropIndex("Diagnostic.TroubleCodes", new[] { "ControllerId" });
            DropTable("Diagnostic.Controllers");
            DropTable("Diagnostic.TroubleCodes");
            DropTable("Diagnostic.FreezeFrameSensors");
            DropTable("Diagnostic.FreezeFrameSensorGroups");
            DropTable("Diagnostic.FreezeFrames");

            CreateTable(
                "Support.Connections",
                c => new
                    {
                        ConnectionGuid = c.Guid(nullable: false),
                        UserGuid = c.Guid(nullable: false),
                        PageUrl = c.String(),
                        ConnectionStartDt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ConnectionGuid)
                .ForeignKey("Access.Users", t => t.UserGuid)
                .Index(t => t.UserGuid);

            Sql("ALTER TABLE Support.Connections ADD PageUrlHash AS CHECKSUM(PageUrl);");
            Sql("CREATE INDEX IDX_SupportConnections_PageUrlHash ON Support.Connections (PageUrlHash);");

            CreateTable(
                "Diagnostic.ResultFreezeFrames",
                c => new
                {
                    FreezeFrameId = c.Int(nullable: false, identity: true),
                    ResultId = c.Int(nullable: false),
                    ControllerId = c.Int(nullable: false),
                    FreezeFrameTroubleCode = c.String(maxLength: 100),
                    SensorGroupsJson = c.String(),
                })
                .PrimaryKey(t => t.FreezeFrameId)
                .ForeignKey("Diagnostic.Controllers", t => t.ControllerId)
                .ForeignKey("Diagnostic.Results", t => t.ResultId)
                .Index(t => t.ResultId)
                .Index(t => t.ControllerId);

            CreateTable(
                "Diagnostic.Controllers",
                c => new
                {
                    ControllerId = c.Int(nullable: false, identity: true),
                    ControllerName = c.String(maxLength: 200),
                })
                .PrimaryKey(t => t.ControllerId);

            Sql("ALTER TABLE Diagnostic.Controllers ADD ControllerHash AS CHECKSUM(ControllerName);");
            Sql("CREATE UNIQUE INDEX UIDX_DiagnosticControllers_ControllerHash ON Diagnostic.Controllers (ControllerHash);");

            CreateTable(
                "Diagnostic.ResultTroubleCodes",
                c => new
                {
                    ResultTroubleCodeId = c.Long(nullable: false, identity: true),
                    ResultId = c.Int(nullable: false),
                    ControllerId = c.Int(nullable: false),
                    TroubleCodeId = c.Int(nullable: false),
                    TroubleCodeInformation = c.String(),
                })
                .PrimaryKey(t => t.ResultTroubleCodeId)
                .ForeignKey("Diagnostic.Controllers", t => t.ControllerId)
                .ForeignKey("Diagnostic.Results", t => t.ResultId)
                .ForeignKey("Diagnostic.TroubleCodes", t => t.TroubleCodeId)
                .Index(t => t.ResultId)
                .Index(t => t.ControllerId)
                .Index(t => t.TroubleCodeId);

            Sql("CREATE INDEX IDX_DiagnosticResultTroubleCodes_ResultId ON Diagnostic.ResultTroubleCodes (ResultId) INCLUDE (ControllerId, TroubleCodeId);");

            CreateTable(
                "Diagnostic.TroubleCodes",
                c => new
                {
                    TroubleCodeId = c.Int(nullable: false, identity: true),
                    TroubleCode = c.String(maxLength: 20),
                    TroubleCodeDescription = c.String(maxLength: 1000),
                })
                .PrimaryKey(t => t.TroubleCodeId);

            Sql("ALTER TABLE Diagnostic.TroubleCodes ADD TroubleCodeHash AS CHECKSUM(TroubleCode, TroubleCodeDescription);");
            Sql("CREATE UNIQUE INDEX UIDX_DiagnosticTroubleCodes_TroubleCodeHash ON Diagnostic.TroubleCodes (TroubleCodeHash);");

            CreateTable(
                "Scan.Decisions",
                c => new
                {
                    DecisionId = c.Int(nullable: false, identity: true),
                    DecisionText = c.String(),
                    ActiveInd = c.Boolean(nullable: false),
                    CreatedByUserGuid = c.Guid(nullable: false),
                    CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                    UpdatedByUserGuid = c.Guid(),
                    UpdatedDt = c.DateTimeOffset(precision: 7),
                })
                .PrimaryKey(t => t.DecisionId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);

            CreateTable(
                    "Scan.DecisionRequestCategories",
                    c => new
                    {
                        DecisionRequestCategoryId = c.Int(nullable: false, identity: true),
                        DecisionId = c.Int(nullable: false),
                        RequestCategoryId = c.Int(nullable: false),
                        PreSelectedInd = c.Boolean(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.DecisionRequestCategoryId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Scan.Decisions", t => t.DecisionId)
                .ForeignKey("Scan.RequestCategories", t => t.RequestCategoryId)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.DecisionId)
                .Index(t => t.RequestCategoryId)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);

            CreateTable(
                "Scan.DecisionRequestTypes",
                c => new
                {
                    DecisionRequestTypeId = c.Int(nullable: false, identity: true),
                    DecisionId = c.Int(nullable: false),
                    RequestTypeId = c.Int(nullable: false),
                    PreSelectedInd = c.Boolean(nullable: false),
                    CreatedByUserGuid = c.Guid(nullable: false),
                    CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                    UpdatedByUserGuid = c.Guid(),
                    UpdatedDt = c.DateTimeOffset(precision: 7),
                })
                .PrimaryKey(t => t.DecisionRequestTypeId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Scan.Decisions", t => t.DecisionId)
                .ForeignKey("Scan.RequestTypes", t => t.RequestTypeId)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.DecisionId)
                .Index(t => t.RequestTypeId)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);

            CreateTable(
                "Scan.DecisionVehicleMakes",
                c => new
                {
                    DecisionVehicleMakeId = c.Int(nullable: false, identity: true),
                    DecisionId = c.Int(nullable: false),
                    VehicleMakeId = c.Int(nullable: false),
                    PreSelectedInd = c.Boolean(nullable: false),
                    CreatedByUserGuid = c.Guid(nullable: false),
                    CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                    UpdatedByUserGuid = c.Guid(),
                    UpdatedDt = c.DateTimeOffset(precision: 7),
                })
                .PrimaryKey(t => t.DecisionVehicleMakeId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Scan.Decisions", t => t.DecisionId)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .ForeignKey("Repair.VehicleMakes", t => t.VehicleMakeId)
                .Index(t => t.DecisionId)
                .Index(t => t.VehicleMakeId)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Scan.ReportDecisions",
                c => new
                {
                    ReportDecisionId = c.Int(nullable: false, identity: true),
                    ReportId = c.Int(nullable: false),
                    DecisionId = c.Int(nullable: false),
                    CreatedByUserGuid = c.Guid(nullable: false),
                    CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                    UpdatedByUserGuid = c.Guid(),
                    UpdatedDt = c.DateTimeOffset(precision: 7),
                })
                .PrimaryKey(t => t.ReportDecisionId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Scan.Decisions", t => t.DecisionId)
                .ForeignKey("Scan.Reports", t => t.ReportId)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.ReportId)
                .Index(t => t.DecisionId)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Scan.ReportOrderTroubleCodes",
                c => new
                    {
                        ReportOrderTroubleCodeId = c.Long(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ControllerId = c.Int(nullable: false),
                        ControllerIdOrig = c.Int(nullable: false),
                        TroubleCodeId = c.Int(nullable: false),
                        TroubleCodeIdOrig = c.Int(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ReportOrderTroubleCodeId)
                .ForeignKey("Diagnostic.Controllers", t => t.ControllerId)
                .ForeignKey("Diagnostic.Controllers", t => t.ControllerIdOrig)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Repair.Orders", t => t.OrderId)
                .ForeignKey("Diagnostic.TroubleCodes", t => t.TroubleCodeId)
                .ForeignKey("Diagnostic.TroubleCodes", t => t.TroubleCodeIdOrig)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.OrderId)
                .Index(t => t.ControllerId)
                .Index(t => t.ControllerIdOrig)
                .Index(t => t.TroubleCodeId)
                .Index(t => t.TroubleCodeIdOrig)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Scan.ReportTroubleCodeRecommendations",
                c => new
                    {
                        ReportTroubleCodeRecommendationId = c.Long(nullable: false, identity: true),
                        ReportOrderTroubleCodeId = c.Long(nullable: false),
                        ReportId = c.Int(nullable: false),
                        ResultTroubleCodeId = c.Long(),
                        InformCustomerInd = c.Boolean(nullable: false),
                        AccidentRelatedInd = c.Boolean(),
                        ExcludeFromReportInd = c.Boolean(nullable: false),
                        TroubleCodeRecommendationId = c.Int(),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ReportTroubleCodeRecommendationId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Scan.Reports", t => t.ReportId)
                .ForeignKey("Scan.ReportOrderTroubleCodes", t => t.ReportOrderTroubleCodeId)
                .ForeignKey("Diagnostic.ResultTroubleCodes", t => t.ResultTroubleCodeId)
                .ForeignKey("Scan.TroubleCodeRecommendations", t => t.TroubleCodeRecommendationId)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.ReportOrderTroubleCodeId)
                .Index(t => t.ReportId)
                .Index(t => t.ResultTroubleCodeId)
                .Index(t => t.TroubleCodeRecommendationId)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Scan.TroubleCodeRecommendations",
                c => new
                {
                    TroubleCodeRecommendationId = c.Int(nullable: false, identity: true),
                    TroubleCodeRecommendationText = c.String(),
                    CreatedByUserGuid = c.Guid(nullable: false),
                    CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                    UpdatedByUserGuid = c.Guid(),
                    UpdatedDt = c.DateTimeOffset(precision: 7),
                })
                .PrimaryKey(t => t.TroubleCodeRecommendationId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);

            Sql("ALTER TABLE Scan.TroubleCodeRecommendations ADD TroubleCodeRecommendationHash AS CHECKSUM(TroubleCodeRecommendationText);");
            Sql("CREATE UNIQUE INDEX UIDX_TroubleCodeRecommendations_TroubleCodeRecommendationHash ON Scan.TroubleCodeRecommendations (TroubleCodeRecommendationHash);");

            Sql("ALTER TABLE Inventory.AirProTools ADD ToolName AS 'AirPro' + RIGHT('0000' + CAST(ToolId AS VARCHAR(10)), 4);");

            AddColumn("Scan.Reports", "ReportFooterHTML", c => c.String());
            AddColumn("Scan.Reports", "DiagnosticResultId", c => c.Int());

            CreateIndex("Scan.Reports", "DiagnosticResultId");
            AddForeignKey("Scan.Reports", "DiagnosticResultId", "Diagnostic.Results", "ResultId");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetDecisionsRanked");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestById");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");

            this.DropObjectIfExists(DropObjectType.Procedure, "Diagnostic", "usp_GetDiagnosticResults");
            this.DropObjectIfExists(DropObjectType.Procedure, "Diagnostic", "usp_SaveDiagnosticResult");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Diagnostic", "udt_ResultFreezeFrames");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Diagnostic", "udt_ResultTroubleCodes");

            this.DropObjectIfExists(DropObjectType.Procedure, "Diagnostic", "usp_GetControllersSearch");
            this.DropObjectIfExists(DropObjectType.Procedure, "Diagnostic", "usp_GetTroubleCodesSearch");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRecommendationSearch");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveScanReport");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_ReportDecisions");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_ReportRecommendations");

            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_SaveConnection");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_SaveConnectionLog");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_GetConnections");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_GetActiveConnections");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetDashboardConnections");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_ApplicationStart");
            this.DropObjectIfExists(DropObjectType.Procedure, "Technician", "usp_GetQueueConnections");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CloseRepairByRequestId");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetDecisionsRanked");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestById");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");

            this.DropObjectIfExists(DropObjectType.Procedure, "Diagnostic", "usp_GetDiagnosticResults");
            this.DropObjectIfExists(DropObjectType.Procedure, "Diagnostic", "usp_SaveDiagnosticResult");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Diagnostic", "udt_ResultFreezeFrames");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Diagnostic", "udt_ResultTroubleCodes");

            this.DropObjectIfExists(DropObjectType.Procedure, "Diagnostic", "usp_GetControllersSearch");
            this.DropObjectIfExists(DropObjectType.Procedure, "Diagnostic", "usp_GetTroubleCodesSearch");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRecommendationSearch");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveScanReport");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_ReportDecisions");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_ReportRecommendations");

            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_SaveConnection");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_SaveConnectionLog");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_GetConnections");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_GetActiveConnections");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetDashboardConnections");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_ApplicationStart");
            this.DropObjectIfExists(DropObjectType.Procedure, "Technician", "usp_GetQueueConnections");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CloseRepairByRequestId");

            DropForeignKey("Scan.ReportTroubleCodeRecommendations", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.ReportTroubleCodeRecommendations", "TroubleCodeRecommendationId", "Scan.TroubleCodeRecommendations");
            DropForeignKey("Scan.TroubleCodeRecommendations", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.TroubleCodeRecommendations", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.ReportTroubleCodeRecommendations", "ResultTroubleCodeId", "Diagnostic.ResultTroubleCodes");
            DropForeignKey("Scan.ReportTroubleCodeRecommendations", "ReportOrderTroubleCodeId", "Scan.ReportOrderTroubleCodes");
            DropForeignKey("Scan.ReportTroubleCodeRecommendations", "ReportId", "Scan.Reports");
            DropForeignKey("Scan.ReportTroubleCodeRecommendations", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.ReportOrderTroubleCodes", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.ReportOrderTroubleCodes", "TroubleCodeIdOrig", "Diagnostic.TroubleCodes");
            DropForeignKey("Scan.ReportOrderTroubleCodes", "TroubleCodeId", "Diagnostic.TroubleCodes");
            DropForeignKey("Scan.ReportOrderTroubleCodes", "OrderId", "Repair.Orders");
            DropForeignKey("Scan.ReportOrderTroubleCodes", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.ReportOrderTroubleCodes", "ControllerIdOrig", "Diagnostic.Controllers");
            DropForeignKey("Scan.ReportOrderTroubleCodes", "ControllerId", "Diagnostic.Controllers");
            DropForeignKey("Scan.ReportDecisions", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.ReportDecisions", "ReportId", "Scan.Reports");
            DropForeignKey("Scan.ReportDecisions", "DecisionId", "Scan.Decisions");
            DropForeignKey("Scan.ReportDecisions", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.DecisionVehicleMakes", "VehicleMakeId", "Repair.VehicleMakes");
            DropForeignKey("Scan.DecisionVehicleMakes", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.DecisionVehicleMakes", "DecisionId", "Scan.Decisions");
            DropForeignKey("Scan.DecisionVehicleMakes", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.DecisionRequestTypes", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.DecisionRequestTypes", "RequestTypeId", "Scan.RequestTypes");
            DropForeignKey("Scan.DecisionRequestTypes", "DecisionId", "Scan.Decisions");
            DropForeignKey("Scan.DecisionRequestTypes", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.DecisionRequestCategories", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.DecisionRequestCategories", "RequestCategoryId", "Scan.RequestCategories");
            DropForeignKey("Scan.DecisionRequestCategories", "DecisionId", "Scan.Decisions");
            DropForeignKey("Scan.Decisions", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.Decisions", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.DecisionRequestCategories", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Diagnostic.ResultTroubleCodes", "TroubleCodeId", "Diagnostic.TroubleCodes");
            DropForeignKey("Diagnostic.ResultTroubleCodes", "ResultId", "Diagnostic.Results");
            DropForeignKey("Diagnostic.ResultTroubleCodes", "ControllerId", "Diagnostic.Controllers");
            DropForeignKey("Diagnostic.ResultFreezeFrames", "ResultId", "Diagnostic.Results");
            DropForeignKey("Scan.Reports", "DiagnosticResultId", "Diagnostic.Results");
            DropForeignKey("Diagnostic.ResultFreezeFrames", "ControllerId", "Diagnostic.Controllers");
            DropForeignKey("Support.Connections", "UserGuid", "Access.Users");
            DropIndex("Scan.TroubleCodeRecommendations", new[] { "UpdatedByUserGuid" });
            DropIndex("Scan.TroubleCodeRecommendations", new[] { "CreatedByUserGuid" });
            DropIndex("Scan.TroubleCodeRecommendations", new[] { "TroubleCodeRecommendationHash" });
            DropIndex("Scan.ReportTroubleCodeRecommendations", new[] { "UpdatedByUserGuid" });
            DropIndex("Scan.ReportTroubleCodeRecommendations", new[] { "CreatedByUserGuid" });
            DropIndex("Scan.ReportTroubleCodeRecommendations", new[] { "TroubleCodeRecommendationId" });
            DropIndex("Scan.ReportTroubleCodeRecommendations", new[] { "ResultTroubleCodeId" });
            DropIndex("Scan.ReportTroubleCodeRecommendations", new[] { "ReportId" });
            DropIndex("Scan.ReportTroubleCodeRecommendations", new[] { "ReportOrderTroubleCodeId" });
            DropIndex("Scan.ReportOrderTroubleCodes", new[] { "UpdatedByUserGuid" });
            DropIndex("Scan.ReportOrderTroubleCodes", new[] { "CreatedByUserGuid" });
            DropIndex("Scan.ReportOrderTroubleCodes", new[] { "TroubleCodeIdOrig" });
            DropIndex("Scan.ReportOrderTroubleCodes", new[] { "TroubleCodeId" });
            DropIndex("Scan.ReportOrderTroubleCodes", new[] { "ControllerIdOrig" });
            DropIndex("Scan.ReportOrderTroubleCodes", new[] { "ControllerId" });
            DropIndex("Scan.ReportOrderTroubleCodes", new[] { "OrderId" });
            DropIndex("Scan.ReportDecisions", new[] { "UpdatedByUserGuid" });
            DropIndex("Scan.ReportDecisions", new[] { "CreatedByUserGuid" });
            DropIndex("Scan.ReportDecisions", new[] { "DecisionId" });
            DropIndex("Scan.ReportDecisions", new[] { "ReportId" });
            DropIndex("Scan.DecisionVehicleMakes", new[] { "UpdatedByUserGuid" });
            DropIndex("Scan.DecisionVehicleMakes", new[] { "CreatedByUserGuid" });
            DropIndex("Scan.DecisionVehicleMakes", new[] { "VehicleMakeId" });
            DropIndex("Scan.DecisionVehicleMakes", new[] { "DecisionId" });
            DropIndex("Scan.DecisionRequestTypes", new[] { "UpdatedByUserGuid" });
            DropIndex("Scan.DecisionRequestTypes", new[] { "CreatedByUserGuid" });
            DropIndex("Scan.DecisionRequestTypes", new[] { "RequestTypeId" });
            DropIndex("Scan.DecisionRequestTypes", new[] { "DecisionId" });
            DropIndex("Scan.Decisions", new[] { "UpdatedByUserGuid" });
            DropIndex("Scan.Decisions", new[] { "CreatedByUserGuid" });
            DropIndex("Scan.DecisionRequestCategories", new[] { "UpdatedByUserGuid" });
            DropIndex("Scan.DecisionRequestCategories", new[] { "CreatedByUserGuid" });
            DropIndex("Scan.DecisionRequestCategories", new[] { "RequestCategoryId" });
            DropIndex("Scan.DecisionRequestCategories", new[] { "DecisionId" });
            DropIndex("Diagnostic.TroubleCodes", new[] { "TroubleCodeHash" });
            DropIndex("Diagnostic.ResultTroubleCodes", new[] { "TroubleCodeId" });
            DropIndex("Diagnostic.ResultTroubleCodes", new[] { "ControllerId" });
            DropIndex("Diagnostic.ResultTroubleCodes", new[] { "ResultId" });
            DropIndex("Scan.Reports", new[] { "DiagnosticResultId" });
            DropIndex("Diagnostic.Controllers", new[] { "ControllerHash" });
            DropIndex("Diagnostic.ResultFreezeFrames", new[] { "ControllerId" });
            DropIndex("Diagnostic.ResultFreezeFrames", new[] { "ResultId" });
            DropIndex("Support.Connections", new[] { "PageUrl" });
            DropIndex("Support.Connections", new[] { "UserGuid" });
            DropColumn("Scan.Reports", "DiagnosticResultId");
            DropColumn("Scan.Reports", "ReportFooterHTML");
            DropColumn("Inventory.AirProTools", "ToolName");
            DropTable("Scan.TroubleCodeRecommendations");
            DropTable("Scan.ReportTroubleCodeRecommendations");
            DropTable("Scan.ReportOrderTroubleCodes");
            DropTable("Scan.ReportDecisions");
            DropTable("Scan.DecisionVehicleMakes");
            DropTable("Scan.DecisionRequestTypes");
            DropTable("Scan.Decisions");
            DropTable("Scan.DecisionRequestCategories");
            DropTable("Diagnostic.TroubleCodes");
            DropTable("Diagnostic.ResultTroubleCodes");
            DropTable("Diagnostic.Controllers");
            DropTable("Diagnostic.ResultFreezeFrames");
            DropTable("Support.Connections");

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
                .PrimaryKey(t => t.ToubleCodeId);

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
                .PrimaryKey(t => t.SensorId);

            CreateTable(
                "Diagnostic.FreezeFrameSensorGroups",
                c => new
                {
                    FreezeFrameGroupId = c.Int(nullable: false, identity: true),
                    FreezeFrameId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.FreezeFrameGroupId);

            CreateTable(
                "Diagnostic.FreezeFrames",
                c => new
                {
                    FreezeFrameId = c.Int(nullable: false, identity: true),
                    ControllerId = c.Int(nullable: false),
                    FreezeFrameTroubleCode = c.String(maxLength: 100),
                })
                .PrimaryKey(t => t.FreezeFrameId);

            CreateTable(
                "Diagnostic.Controllers",
                c => new
                {
                    ControllerId = c.Int(nullable: false, identity: true),
                    ResultId = c.Int(nullable: false),
                    ControllerName = c.String(maxLength: 200),
                })
                .PrimaryKey(t => t.ControllerId);

            CreateIndex("Diagnostic.TroubleCodes", "ControllerId");
            CreateIndex("Diagnostic.FreezeFrameSensors", "FreezeFrameGroupId");
            CreateIndex("Diagnostic.FreezeFrameSensorGroups", "FreezeFrameId");
            CreateIndex("Diagnostic.FreezeFrames", "ControllerId");
            CreateIndex("Diagnostic.Controllers", "ResultId");
            CreateIndex("Support.ConnectionLogs", "ConnectionEndDt");
            CreateIndex("Support.ConnectionLogs", "UserGuid");
			
            AddForeignKey("Diagnostic.Controllers", "ResultId", "Diagnostic.Results", "ResultId");
            AddForeignKey("Diagnostic.TroubleCodes", "ControllerId", "Diagnostic.Controllers", "ControllerId");
            AddForeignKey("Diagnostic.FreezeFrames", "ControllerId", "Diagnostic.Controllers", "ControllerId");
            AddForeignKey("Diagnostic.FreezeFrameSensorGroups", "FreezeFrameId", "Diagnostic.FreezeFrames", "FreezeFrameId");
            AddForeignKey("Diagnostic.FreezeFrameSensors", "FreezeFrameGroupId", "Diagnostic.FreezeFrameSensorGroups", "FreezeFrameGroupId");
            AddForeignKey("Support.ConnectionLogs", "UserGuid", "Access.Users", "UserGuid");
        }
    }
}
