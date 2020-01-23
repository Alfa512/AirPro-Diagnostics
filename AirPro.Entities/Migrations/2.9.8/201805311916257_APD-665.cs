namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD665 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Access.UserPreferences",
                c => new
                    {
                        UserGuid = c.Guid(nullable: false),
                        ControlId = c.String(nullable: false, maxLength: 128),
                        SettingsJson = c.String(),
                        UpdatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserGuid, t.ControlId });
            
        }
        
        public override void Down()
        {
            DropTable("Access.UserPreferences");
        }
    }
}
