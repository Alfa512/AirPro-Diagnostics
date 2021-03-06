using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1938 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetScanReportDataSource");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetScanReportDataSource");
        }
    }
}
