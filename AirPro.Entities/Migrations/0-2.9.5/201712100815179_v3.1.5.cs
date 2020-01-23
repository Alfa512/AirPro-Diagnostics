namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v315 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'Reporting') EXEC('CREATE SCHEMA [Reporting] AUTHORIZATION [dbo]');");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Scan' AND ROUTINE_NAME = 'usp_GetChartingData') DROP PROCEDURE Scan.usp_GetChartingData;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Support' AND ROUTINE_NAME = 'usp_GetReportDataDump') DROP PROCEDURE Support.usp_GetReportDataDump;");
        }

        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetChartingData') ALTER SCHEMA Scan TRANSFER Reporting.usp_GetChartingData;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetReportDataDump') ALTER SCHEMA Support TRANSFER Reporting.usp_GetReportDataDump;");
        }
    }
}
