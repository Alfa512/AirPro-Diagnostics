using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2284 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveLoginAttempt");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetReportAccess");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveLoginAttempt");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetReportAccess");
        }
    }
}
