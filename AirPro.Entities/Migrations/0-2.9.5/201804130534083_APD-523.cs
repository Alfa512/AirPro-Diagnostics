namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD523 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Technician.ProfileSchedules",
                c => new
                {
                    ScheduleId = c.Int(nullable: false, identity: true),
                    UserGuid = c.Guid(nullable: false),
                    DayOfWeek = c.Int(nullable: false),
                    StartTime = c.Time(precision: 7),
                    EndTime = c.Time(precision: 7),
                })
                .PrimaryKey(t => t.ScheduleId)
                .ForeignKey("Access.Users", t => t.UserGuid)
                .ForeignKey("Technician.Profiles", t => t.UserGuid)
                .Index(t => t.UserGuid);

            CreateTable(
                "Technician.ProfileTimeOffEntries",
                c => new
                {
                    TimeOffEntryId = c.Int(nullable: false, identity: true),
                    UserGuid = c.Guid(nullable: false),
                    StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                    EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                    Reason = c.String(),
                    DeleteInd = c.Boolean(nullable: false),
                    CreatedByUserGuid = c.Guid(nullable: false),
                    CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                    UpdatedByUserGuid = c.Guid(),
                    UpdatedDt = c.DateTimeOffset(precision: 7),
                })
                .PrimaryKey(t => t.TimeOffEntryId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .ForeignKey("Access.Users", t => t.UserGuid)
                .ForeignKey("Technician.Profiles", t => t.UserGuid)
                .Index(t => t.UserGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Technician' AND ROUTINE_NAME = 'usp_SaveProfileTimeOff') DROP PROCEDURE Technician.usp_SaveProfileTimeOff;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Technician' AND ROUTINE_NAME = 'usp_SaveProfileSchedules') DROP PROCEDURE Technician.usp_SaveProfileSchedules;");

            Sql(@"IF type_id('[Technician].[udt_TimeOff]') IS NOT NULL DROP TYPE [Technician].[udt_TimeOff]");
            Sql(@"IF type_id('[Technician].[udt_Schedules]') IS NOT NULL DROP TYPE [Technician].[udt_Schedules]");
        }
        
        public override void Down()
        {
            DropForeignKey("Technician.ProfileTimeOffEntries", "UserGuid", "Technician.Profiles");
            DropForeignKey("Technician.ProfileTimeOffEntries", "UserGuid", "Access.Users");
            DropForeignKey("Technician.ProfileTimeOffEntries", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Technician.ProfileTimeOffEntries", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Technician.ProfileSchedules", "UserGuid", "Technician.Profiles");
            DropForeignKey("Technician.ProfileSchedules", "UserGuid", "Access.Users");
            DropIndex("Technician.ProfileTimeOffEntries", new[] { "UpdatedByUserGuid" });
            DropIndex("Technician.ProfileTimeOffEntries", new[] { "CreatedByUserGuid" });
            DropIndex("Technician.ProfileTimeOffEntries", new[] { "UserGuid" });
            DropIndex("Technician.ProfileSchedules", new[] { "UserGuid" });
            DropTable("Technician.ProfileTimeOffEntries");
            DropTable("Technician.ProfileSchedules");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Technician' AND ROUTINE_NAME = 'usp_SaveProfileTimeOff') DROP PROCEDURE Technician.usp_SaveProfileTimeOff;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Technician' AND ROUTINE_NAME = 'usp_SaveProfileSchedules') DROP PROCEDURE Technician.usp_SaveProfileSchedules;");

            Sql(@"IF type_id('[Technician].[udt_TimeOff]') IS NOT NULL DROP TYPE [Technician].[udt_TimeOff]");
            Sql(@"IF type_id('[Technician].[udt_Schedules]') IS NOT NULL DROP TYPE [Technician].[udt_Schedules]");
        }
    }
}
