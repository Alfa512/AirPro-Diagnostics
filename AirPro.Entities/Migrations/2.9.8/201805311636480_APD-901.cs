namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD901 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_CloseRepairsByAge') DROP PROCEDURE Repair.usp_CloseRepairsByAge;");
            AddColumn("Access.Shops", "AutomaticRepairCloseDays", c => c.Int());
        }
        
        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_CloseRepairsByAge') DROP PROCEDURE Repair.usp_CloseRepairsByAge;");
            DropColumn("Access.Shops", "AutomaticRepairCloseDays");
        }
    }
}
