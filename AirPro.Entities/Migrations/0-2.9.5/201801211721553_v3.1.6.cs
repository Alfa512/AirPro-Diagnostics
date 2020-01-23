namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v316 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Repair' AND TABLE_NAME = 'vwRequestTypeCounts') DROP VIEW Repair.vwRequestTypeCounts;");
        }
        
        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Repair' AND TABLE_NAME = 'vwRequestTypeCounts') DROP VIEW Repair.vwRequestTypeCounts;");
        }
    }
}
