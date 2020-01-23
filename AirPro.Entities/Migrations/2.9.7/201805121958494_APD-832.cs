namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD832 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Technician.ProfileSchedules", "BreakStart", c => c.Time(precision: 7));
            AddColumn("Technician.ProfileSchedules", "BreakEnd", c => c.Time(precision: 7));

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Technician' AND ROUTINE_NAME = 'usp_SaveProfileTimeOff') DROP PROCEDURE Technician.usp_SaveProfileTimeOff;");
            Sql(@"IF type_id('[Technician].[udt_TimeOff]') IS NOT NULL DROP TYPE [Technician].[udt_TimeOff]");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Technician' AND ROUTINE_NAME = 'usp_SaveProfileSchedules') DROP PROCEDURE Technician.usp_SaveProfileSchedules;");
            Sql(@"IF type_id('[Technician].[udt_Schedules]') IS NOT NULL DROP TYPE [Technician].[udt_Schedules]");
        }
        
        public override void Down()
        {
            DropColumn("Technician.ProfileSchedules", "BreakEnd");
            DropColumn("Technician.ProfileSchedules", "BreakStart");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Technician' AND ROUTINE_NAME = 'usp_SaveProfileTimeOff') DROP PROCEDURE Technician.usp_SaveProfileTimeOff;");
            Sql(@"IF type_id('[Technician].[udt_TimeOff]') IS NOT NULL DROP TYPE [Technician].[udt_TimeOff]");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Technician' AND ROUTINE_NAME = 'usp_SaveProfileSchedules') DROP PROCEDURE Technician.usp_SaveProfileSchedules;");
            Sql(@"IF type_id('[Technician].[udt_Schedules]') IS NOT NULL DROP TYPE [Technician].[udt_Schedules]");
        }
    }
}
