namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD978 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetEstimateReportDataSource') DROP PROCEDURE Reporting.usp_GetEstimateReportDataSource;");
        }

        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetEstimateReportDataSource') DROP PROCEDURE Reporting.usp_GetEstimateReportDataSource;");
        }
    }
}
