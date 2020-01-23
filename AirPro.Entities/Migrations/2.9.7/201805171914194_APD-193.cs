namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD193 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Scan.RequestsArchive",
                c => new
                    {
                        RequestArchiveId = c.Int(nullable: false, identity: true),
                        ArchiveDt = c.DateTimeOffset(nullable: false, precision: 7),
                        RequestId = c.Int(nullable: false),
                        RequestTypeId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        ReportId = c.Int(),
                        OtherWarningInfo = c.String(),
                        ProblemDescription = c.String(),
                        Notes = c.String(),
                        Contact = c.String(),
                        RequestCategoryId = c.Int(),
                        SeatRemovedInd = c.Boolean(nullable: false),
                        ToolId = c.Int(),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.RequestArchiveId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Scan.Requests", t => t.RequestId)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.RequestId)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Scan.RequestsArchive", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Scan.RequestsArchive", "RequestId", "Scan.Requests");
            DropForeignKey("Scan.RequestsArchive", "CreatedByUserGuid", "Access.Users");
            DropIndex("Scan.RequestsArchive", new[] { "UpdatedByUserGuid" });
            DropIndex("Scan.RequestsArchive", new[] { "CreatedByUserGuid" });
            DropIndex("Scan.RequestsArchive", new[] { "RequestId" });
            DropTable("Scan.RequestsArchive");
        }
    }
}
