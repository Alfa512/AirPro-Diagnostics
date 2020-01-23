using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1678 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.ReportTroubleCodeRecommendations", "CodeClearedInd", c => c.Boolean(nullable: false));

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_ReportRecommendations");

            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_ReportRecommendations");

            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");

            DropColumn("Scan.ReportTroubleCodeRecommendations", "CodeClearedInd");
        }
    }
}
