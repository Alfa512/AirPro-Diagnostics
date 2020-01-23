namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1310 : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM Access.UserPreferences WHERE ControlId = 'airprotools-grid'");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Access.UserPreferences WHERE ControlId = 'airprotools-grid'");
        }
    }
}
