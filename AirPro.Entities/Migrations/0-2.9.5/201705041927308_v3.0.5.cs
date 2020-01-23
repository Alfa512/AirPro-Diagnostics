namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v305 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Support.ConnectionLogs",
                c => new
                    {
                        ConnectionGuid = c.Guid(nullable: false),
                        UserGuid = c.Guid(nullable: false),
                        PageUrl = c.String(),
                        ConnectionStartDt = c.DateTimeOffset(nullable: false, precision: 7),
                        ConnectionEndDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ConnectionGuid)
                .ForeignKey("Access.Users", t => t.UserGuid, cascadeDelete: true)
                .Index(t => t.UserGuid)
                .Index(t => t.ConnectionEndDt);
            
            DropColumn("Access.Users", "ConnectionId");
            DropColumn("Access.Users", "ConnectionLastDt");
        }
        
        public override void Down()
        {
            AddColumn("Access.Users", "ConnectionLastDt", c => c.DateTimeOffset(precision: 7));
            AddColumn("Access.Users", "ConnectionId", c => c.String());
            DropForeignKey("Support.ConnectionLogs", "UserGuid", "Access.Users");
            DropIndex("Support.ConnectionLogs", new[] { "ConnectionEndDt" });
            DropIndex("Support.ConnectionLogs", new[] { "UserGuid" });
            DropTable("Support.ConnectionLogs");
        }
    }
}
