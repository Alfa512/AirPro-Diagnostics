using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1257 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.Requests", "ContactUserGuid", c => c.Guid());
            AddColumn("Scan.RequestsArchive", "ContactUserGuid", c => c.Guid());
            CreateIndex("Scan.Requests", "ContactUserGuid");
            CreateIndex("Scan.RequestsArchive", "ContactUserGuid");
            AddForeignKey("Scan.Requests", "ContactUserGuid", "Access.Users", "UserGuid");
            AddForeignKey("Scan.RequestsArchive", "ContactUserGuid", "Access.Users", "UserGuid");

            this.DropObjectIfExists(DropObjectType.Trigger, "Scan", "trgScanRequestsArchive");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetTechnicianRequest");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetUsersByRepairId");
        }

        public override void Down()
        {
            DropForeignKey("Scan.RequestsArchive", "ContactUserGuid", "Access.Users");
            DropForeignKey("Scan.Requests", "ContactUserGuid", "Access.Users");
            DropIndex("Scan.RequestsArchive", new[] { "ContactUserGuid" });
            DropIndex("Scan.Requests", new[] { "ContactUserGuid" });
            DropColumn("Scan.RequestsArchive", "ContactUserGuid");
            DropColumn("Scan.Requests", "ContactUserGuid");

            this.DropObjectIfExists(DropObjectType.Trigger, "Scan", "trgScanRequestsArchive");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetTechnicianRequest");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetUsersByRepairId");
        }
    }
}
