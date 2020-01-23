namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1177 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Common.Notes",
                c => new
                    {
                        NoteId = c.Int(nullable: false, identity: true),
                        NoteKey = c.String(nullable: false, maxLength: 50),
                        NoteTypeId = c.Int(nullable: false),
                        NoteDescription = c.String(),
                        NoteDeletedInd = c.Boolean(nullable: false),
                        NoteDeletedByUserGuid = c.Guid(),
                        NoteDeletedDt = c.DateTimeOffset(precision: 7),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.NoteId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Access.Users", t => t.NoteDeletedByUserGuid)
                .ForeignKey("Common.NoteTypes", t => t.NoteTypeId)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.NoteTypeId)
                .Index(t => t.NoteDeletedByUserGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Common.NoteTypes",
                c => new
                    {
                        NoteTypeId = c.Int(nullable: false, identity: true),
                        NoteTypeName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.NoteTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Common.Notes", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Common.Notes", "NoteTypeId", "Common.NoteTypes");
            DropForeignKey("Common.Notes", "NoteDeletedByUserGuid", "Access.Users");
            DropForeignKey("Common.Notes", "CreatedByUserGuid", "Access.Users");
            DropIndex("Common.Notes", new[] { "UpdatedByUserGuid" });
            DropIndex("Common.Notes", new[] { "CreatedByUserGuid" });
            DropIndex("Common.Notes", new[] { "NoteDeletedByUserGuid" });
            DropIndex("Common.Notes", new[] { "NoteTypeId" });
            DropTable("Common.NoteTypes");
            DropTable("Common.Notes");
        }
    }
}
