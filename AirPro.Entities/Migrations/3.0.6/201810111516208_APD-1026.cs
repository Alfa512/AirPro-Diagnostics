namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1026 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Common.ReleaseNoteRoles",
                c => new
                    {
                        ReleaseNoteId = c.Int(nullable: false),
                        RoleGuid = c.Guid(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => new { t.ReleaseNoteId, t.RoleGuid })
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid, cascadeDelete: true)
                .ForeignKey("Common.ReleaseNotes", t => t.ReleaseNoteId, cascadeDelete: true)
                .ForeignKey("Access.Roles", t => t.RoleGuid, cascadeDelete: true)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.ReleaseNoteId)
                .Index(t => t.RoleGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Common.ReleaseNotes",
                c => new
                    {
                        ReleaseNoteId = c.Int(nullable: false, identity: true),
                        Version = c.String(),
                        Summary = c.String(),
                        DevelopmentId = c.String(),
                        ReleaseNote = c.String(),
                        DeletedInd = c.Boolean(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ReleaseNoteId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid, cascadeDelete: false)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Common.ReleaseNoteRoles", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Common.ReleaseNoteRoles", "RoleGuid", "Access.Roles");
            DropForeignKey("Common.ReleaseNoteRoles", "ReleaseNoteId", "Common.ReleaseNotes");
            DropForeignKey("Common.ReleaseNotes", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Common.ReleaseNotes", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Common.ReleaseNoteRoles", "CreatedByUserGuid", "Access.Users");
            DropIndex("Common.ReleaseNotes", new[] { "UpdatedByUserGuid" });
            DropIndex("Common.ReleaseNotes", new[] { "CreatedByUserGuid" });
            DropIndex("Common.ReleaseNoteRoles", new[] { "UpdatedByUserGuid" });
            DropIndex("Common.ReleaseNoteRoles", new[] { "CreatedByUserGuid" });
            DropIndex("Common.ReleaseNoteRoles", new[] { "RoleGuid" });
            DropIndex("Common.ReleaseNoteRoles", new[] { "ReleaseNoteId" });
            DropTable("Common.ReleaseNotes");
            DropTable("Common.ReleaseNoteRoles");
        }
    }
}
