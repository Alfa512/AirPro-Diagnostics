using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1613 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.ReportTroubleCodeRecommendations", "TroubleCodeNoteText", c => c.String());

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");

            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_ReportRecommendations");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CloseRepairByRequestId");
        }

        public override void Down()
        {
            DropColumn("Scan.ReportTroubleCodeRecommendations", "TroubleCodeNoteText");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");

            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Scan", "udt_ReportRecommendations");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CloseRepairByRequestId");
        }
    }
}
