using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1681 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.Decisions", "DefaultTextSeverity", c => c.Int(nullable: false));
            AddColumn("Scan.ReportDecisions", "TextSeverity", c => c.Int(nullable: false));
            AddColumn("Scan.ReportTroubleCodeRecommendations", "TextSeverity", c => c.Int(nullable: false));

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveDecision");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetDecisionById");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetDecisionsRanked");

            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_ReportDecisions");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_ReportRecommendations");
        }
        
        public override void Down()
        {
            DropColumn("Scan.ReportTroubleCodeRecommendations", "TextSeverity");
            DropColumn("Scan.ReportDecisions", "TextSeverity");
            DropColumn("Scan.Decisions", "DefaultTextSeverity");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveDecision");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetDecisionById");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetDecisionsRanked");

            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_ReportDecisions");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_ReportRecommendations");
        }
    }
}
