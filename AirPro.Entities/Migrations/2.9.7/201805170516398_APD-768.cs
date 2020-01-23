namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD768 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "AllowSelfScanAssessment", c => c.Boolean(nullable: false));

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_GetRepairsByUser') DROP PROCEDURE Repair.usp_GetRepairsByUser;");
        }
        
        public override void Down()
        {
            DropColumn("Access.Shops", "AllowSelfScanAssessment");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_GetRepairsByUser') DROP PROCEDURE Repair.usp_GetRepairsByUser;");
        }
    }
}
