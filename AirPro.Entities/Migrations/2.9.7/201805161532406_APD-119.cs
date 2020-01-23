namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD119 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Common.Uploads",
                c => new
                    {
                        UploadId = c.Int(nullable: false, identity: true),
                        UploadKey = c.String(nullable: false, maxLength: 50),
                        UploadTypeId = c.Int(nullable: false),
                        UploadFileName = c.String(maxLength: 150),
                        UploadFileExtension = c.String(maxLength: 10),
                        UploadFileSizeBytes = c.Long(nullable: false),
                        UploadStorageName = c.String(maxLength: 50),
                        UploadMimeType = c.String(maxLength: 100),
                        UploadDeletedInd = c.Boolean(nullable: false),
                        UploadDeletedByUserGuid = c.Guid(),
                        UploadDeletedDt = c.DateTimeOffset(precision: 7),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.UploadId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .ForeignKey("Access.Users", t => t.UploadDeletedByUserGuid)
                .ForeignKey("Common.UploadTypes", t => t.UploadTypeId)
                .Index(t => t.UploadTypeId)
                .Index(t => t.UploadDeletedByUserGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Common.UploadTypes",
                c => new
                    {
                        UploadTypeId = c.Int(nullable: false, identity: true),
                        UploadTypeName = c.String(maxLength: 50),
                        UploadTypeSchema = c.String(maxLength: 50),
                        UploadTypeTable = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.UploadTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Common.Uploads", "UploadTypeId", "Common.UploadTypes");
            DropForeignKey("Common.Uploads", "UploadDeletedByUserGuid", "Access.Users");
            DropForeignKey("Common.Uploads", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Common.Uploads", "CreatedByUserGuid", "Access.Users");
            DropIndex("Common.Uploads", new[] { "UpdatedByUserGuid" });
            DropIndex("Common.Uploads", new[] { "CreatedByUserGuid" });
            DropIndex("Common.Uploads", new[] { "UploadDeletedByUserGuid" });
            DropIndex("Common.Uploads", new[] { "UploadTypeId" });
            DropTable("Common.UploadTypes");
            DropTable("Common.Uploads");
        }
    }
}
