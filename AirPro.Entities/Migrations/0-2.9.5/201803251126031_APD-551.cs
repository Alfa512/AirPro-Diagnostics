namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD551 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_GetRepairsByUser') DROP PROCEDURE Repair.usp_GetRepairsByUser;");
        }

        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_GetRepairsByUser') DROP PROCEDURE Repair.usp_GetRepairsByUser;");
        }
    }
}
