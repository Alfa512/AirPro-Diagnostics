using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class APD1380 : DbMigration
    {
        public override void Up()
        {

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveDecision");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetDecisionById");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetDecisionsByGridPage");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");

            Sql("ALTER TABLE Access.Users ADD DisplayName AS LastName + ', ' + FirstName;");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveDecision");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetDecisionById");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetDecisionsByGridPage");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");

            DropColumn("Access.Users", "DisplayName");
        }
    }
}
