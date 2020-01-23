using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v302 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Support.NotificationLogs", newName: "Logs");
            RenameTable(name: "Support.NotificationTemplates", newName: "Templates");
            MoveTable(name: "Support.Logs", newSchema: "Notification");
            MoveTable(name: "Support.Templates", newSchema: "Notification");
            CreateTable(
                "Notification.TypeRoles",
                c => new
                    {
                        TypeGuid = c.Guid(nullable: false),
                        RoleGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.TypeGuid, t.RoleGuid })
                .ForeignKey("Access.Roles", t => t.RoleGuid, cascadeDelete: true)
                .ForeignKey("Notification.Types", t => t.TypeGuid, cascadeDelete: true)
                .Index(t => t.TypeGuid)
                .Index(t => t.RoleGuid);
            
            CreateTable(
                "Notification.Types",
                c => new
                    {
                        TypeGuid = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TypeGuid);
            
            CreateTable(
                "Notification.UserOptOuts",
                c => new
                    {
                        TypeGuid = c.Guid(nullable: false),
                        UserGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.TypeGuid, t.UserGuid })
                .ForeignKey("Notification.Types", t => t.TypeGuid, cascadeDelete: true)
                .ForeignKey("Access.Users", t => t.UserGuid, cascadeDelete: true)
                .Index(t => t.TypeGuid)
                .Index(t => t.UserGuid);

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201703241625584_v3.0.2_Deploy.sql");
        }

        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Access' AND ROUTINE_NAME = 'usp_NotificationUsers') DROP PROCEDURE Access.usp_NotificationUsers");
            DropForeignKey("Notification.UserOptOuts", "UserGuid", "Access.Users");
            DropForeignKey("Notification.UserOptOuts", "TypeGuid", "Notification.Types");
            DropForeignKey("Notification.TypeRoles", "TypeGuid", "Notification.Types");
            DropForeignKey("Notification.TypeRoles", "RoleGuid", "Access.Roles");
            DropIndex("Notification.UserOptOuts", new[] { "UserGuid" });
            DropIndex("Notification.UserOptOuts", new[] { "TypeGuid" });
            DropIndex("Notification.TypeRoles", new[] { "RoleGuid" });
            DropIndex("Notification.TypeRoles", new[] { "TypeGuid" });
            DropTable("Notification.UserOptOuts");
            DropTable("Notification.Types");
            DropTable("Notification.TypeRoles");
            MoveTable(name: "Notification.Templates", newSchema: "Support");
            MoveTable(name: "Notification.Logs", newSchema: "Support");
            RenameTable(name: "Support.Templates", newName: "NotificationTemplates");
            RenameTable(name: "Support.Logs", newName: "NotificationLogs");
        }
    }
}
