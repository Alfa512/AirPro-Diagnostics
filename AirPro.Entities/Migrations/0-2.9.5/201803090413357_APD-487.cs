namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD487 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_BuildReportData') DROP PROCEDURE Reporting.usp_BuildReportData;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetReportDataDump') DROP PROCEDURE Reporting.usp_GetReportDataDump;");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Reporting' AND TABLE_NAME = 'ReportData') DROP TABLE Reporting.ReportData;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Reporting' AND TABLE_NAME = 'vwReportData') DROP VIEW Reporting.vwReportData;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Reporting' AND TABLE_NAME = 'ReportDataLoads') DROP TABLE Reporting.ReportDataLoads;");
        }

        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_BuildReportData') DROP PROCEDURE Reporting.usp_BuildReportData;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetReportDataDump') DROP PROCEDURE Reporting.usp_GetReportDataDump;");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Reporting' AND TABLE_NAME = 'ReportData') DROP TABLE Reporting.ReportData;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Reporting' AND TABLE_NAME = 'vwReportData') DROP VIEW Reporting.vwReportData;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Reporting' AND TABLE_NAME = 'ReportDataLoads') DROP TABLE Reporting.ReportDataLoads;");
        }
    }
}
