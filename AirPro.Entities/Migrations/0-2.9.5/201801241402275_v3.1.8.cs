namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v318 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Access' AND TABLE_NAME = 'vwUserMemberships') DROP VIEW Access.vwUserMemberships;");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_SaveVehicleLookup') DROP PROCEDURE Repair.usp_SaveVehicleLookup;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_SaveVehicle') DROP PROCEDURE Repair.usp_SaveVehicle;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_GetVehicleByVIN') DROP PROCEDURE Repair.usp_GetVehicleByVIN;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_GetRepairsByUser') DROP PROCEDURE Repair.usp_GetRepairsByUser;");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetReportDataDump') DROP PROCEDURE Reporting.usp_GetReportDataDump;");
        }

        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Access' AND TABLE_NAME = 'vwUserMemberships') DROP VIEW Access.vwUserMemberships;");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_SaveVehicleLookup') DROP PROCEDURE Repair.usp_SaveVehicleLookup;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_SaveVehicle') DROP PROCEDURE Repair.usp_SaveVehicle;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_GetVehicleByVIN') DROP PROCEDURE Repair.usp_GetVehicleByVIN;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_GetRepairsByUser') DROP PROCEDURE Repair.usp_GetRepairsByUser;");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetReportDataDump') DROP PROCEDURE Reporting.usp_GetReportDataDump;");
        }
    }
}
