using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class APD1619 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.TroubleCodeRecommendations", "ActiveInd", c => c.Boolean(nullable: false, defaultValue: true));

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetTroubleCodeRecommendations");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveTroubleCodeRecommendation");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRecommendationSearch");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
        }

        public override void Down()
        {
            DropColumn("Scan.TroubleCodeRecommendations", "ActiveInd");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetTroubleCodeRecommendations");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveTroubleCodeRecommendation");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRecommendationSearch");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
        }
    }
}
