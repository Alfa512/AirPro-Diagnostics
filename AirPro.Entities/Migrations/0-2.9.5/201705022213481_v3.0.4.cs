namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v304 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Support.RequestLogs",
                c => new
                    {
                        RequestLogId = c.Long(nullable: false, identity: true),
                        UserGuid = c.Guid(),
                        SessionId = c.String(maxLength: 24),
                        UserAgentId = c.Int(nullable: false),
                        UserAddress = c.String(maxLength: 45),
                        RawUrl = c.String(),
                        RouteUrl = c.String(maxLength: 100),
                        RouteArea = c.String(maxLength: 100),
                        RouteController = c.String(maxLength: 100),
                        RouteAction = c.String(maxLength: 100),
                        RouteId = c.String(maxLength: 100),
                        RequestMethod = c.String(maxLength: 10),
                        ActionStartTime = c.DateTimeOffset(precision: 7),
                        ActionEndTime = c.DateTimeOffset(precision: 7),
                        ResultStartTime = c.DateTimeOffset(precision: 7),
                        ResultEndTime = c.DateTimeOffset(precision: 7),
                        ActionExceptionId = c.Int(),
                        ResultExceptionId = c.Int(),
                    })
                .PrimaryKey(t => t.RequestLogId)
                .ForeignKey("Support.RequestLogExceptions", t => t.ActionExceptionId)
                .ForeignKey("Support.RequestLogExceptions", t => t.ResultExceptionId)
                .ForeignKey("Support.RequestLogUserAgents", t => t.UserAgentId)
                .ForeignKey("Access.Users", t => t.UserGuid)
                .Index(t => t.UserGuid)
                .Index(t => t.SessionId)
                .Index(t => t.UserAgentId)
                .Index(t => t.ActionExceptionId)
                .Index(t => t.ResultExceptionId);
            
            CreateTable(
                    "Support.RequestLogExceptions",
                    c => new
                    {
                        ExcptionId = c.Int(nullable: false, identity: true),
                        ExceptionMessage = c.String(),
                        ExceptionStackTrace = c.String()
                    })
                .PrimaryKey(t => t.ExcptionId);

            Sql("ALTER TABLE Support.RequestLogExceptions ADD ExceptionHash AS CHECKSUM(ExceptionMessage, ExceptionStackTrace);");
            Sql("CREATE UNIQUE INDEX UIDX_RequestLogExceptions_ExceptionHash ON Support.RequestLogExceptions (ExceptionHash);");

            CreateTable(
                    "Support.RequestLogUserAgents",
                    c => new
                    {
                        UserAgentId = c.Int(nullable: false, identity: true),
                        UserAgentString = c.String()
                    })
                .PrimaryKey(t => t.UserAgentId);

            Sql("ALTER TABLE Support.RequestLogUserAgents ADD UserAgentHash AS CHECKSUM(UserAgentString);");
            Sql("CREATE UNIQUE INDEX UIDX_RequestLogUserAgents_UserAgentHash ON Support.RequestLogUserAgents (UserAgentHash);");

            AddColumn("Access.Users", "SessionId", c => c.String(maxLength: 24));
            CreateIndex("Access.Users", "SessionId");
        }

        
        public override void Down()
        {
            DropForeignKey("Support.RequestLogs", "UserAgentId", "Support.RequestLogUserAgents");
            DropForeignKey("Support.RequestLogs", "UserGuid", "Access.Users");
            DropForeignKey("Support.RequestLogs", "ResultExceptionId", "Support.RequestLogExceptions");
            DropForeignKey("Support.RequestLogs", "ActionExceptionId", "Support.RequestLogExceptions");
            DropIndex("Support.RequestLogUserAgents", new[] { "UserAgentHash" });
            DropIndex("Support.RequestLogExceptions", new[] { "ExceptionHash" });
            DropIndex("Support.RequestLogs", new[] { "ResultExceptionId" });
            DropIndex("Support.RequestLogs", new[] { "ActionExceptionId" });
            DropIndex("Support.RequestLogs", new[] { "UserAgentId" });
            DropIndex("Support.RequestLogs", new[] { "SessionId" });
            DropIndex("Support.RequestLogs", new[] { "UserGuid" });
            DropIndex("Access.Users", new[] { "SessionId" });
            DropColumn("Access.Users", "SessionId");
            DropTable("Support.RequestLogUserAgents");
            DropTable("Support.RequestLogExceptions");
            DropTable("Support.RequestLogs");
        }
    }
}
