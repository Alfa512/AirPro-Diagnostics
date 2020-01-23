namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD599 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetScanReportDataSource') DROP PROCEDURE Reporting.usp_GetScanReportDataSource;");
        }

        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetScanReportDataSource') DROP PROCEDURE Reporting.usp_GetScanReportDataSource;");
        }
    }
}
