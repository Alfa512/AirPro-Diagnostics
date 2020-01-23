namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;

    public partial class APD996 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetEstimateReportDataSource");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetEstimateReportDataSource");
        }
    }
}
