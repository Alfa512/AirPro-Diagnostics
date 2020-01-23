namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD890 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Repair.VehicleMakes", "ProgramName", c => c.String());

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_GetVehicleMakes') DROP PROCEDURE Repair.usp_GetVehicleMakes;");
        }

        public override void Down()
        {
            DropColumn("Repair.VehicleMakes", "ProgramName");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_GetVehicleMakes') DROP PROCEDURE Repair.usp_GetVehicleMakes;");
        }
    }
}
