using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1452 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Service.MitchellReports",
                c => new
                    {
                        MitchellReportId = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        RequestUserGuid = c.Guid(nullable: false),
                        RequestDt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.MitchellReportId)
                .ForeignKey("Scan.Requests", t => t.RequestId)
                .ForeignKey("Access.Users", t => t.RequestUserGuid)
                .Index(t => t.RequestId)
                .Index(t => t.RequestUserGuid);
            
            AddColumn("Access.Shops", "SendToMitchellInd", c => c.Boolean(nullable: false));
            AddColumn("Access.ShopsArchive", "SendToMitchellInd", c => c.Boolean(nullable: false));

            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessShopsArchive");
            this.DropObjectIfExists(DropObjectType.Procedure, "Service", "usp_GetMitchellReport");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessShopsArchive");
            this.DropObjectIfExists(DropObjectType.Procedure, "Service", "usp_GetMitchellReport");

            DropForeignKey("Service.MitchellReports", "RequestUserGuid", "Access.Users");
            DropForeignKey("Service.MitchellReports", "RequestId", "Scan.Requests");
            DropIndex("Service.MitchellReports", new[] { "RequestUserGuid" });
            DropIndex("Service.MitchellReports", new[] { "RequestId" });
            DropColumn("Access.ShopsArchive", "SendToMitchellInd");
            DropColumn("Access.Shops", "SendToMitchellInd");
            DropTable("Service.MitchellReports");
        }
    }
}
