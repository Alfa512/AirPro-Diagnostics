using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1852 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.ReportOrderTroubleCodes", "RequestId", c => c.Int());
            CreateIndex("Scan.ReportOrderTroubleCodes", "RequestId");
            AddForeignKey("Scan.ReportOrderTroubleCodes", "RequestId", "Scan.Requests", "RequestId");

            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");

            DropForeignKey("Scan.ReportOrderTroubleCodes", "RequestId", "Scan.Requests");
            DropIndex("Scan.ReportOrderTroubleCodes", new[] { "RequestId" });
            DropColumn("Scan.ReportOrderTroubleCodes", "RequestId");
        }
    }
}
