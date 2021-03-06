using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1445 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRequestTypeCountByDate");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRepairCountByStatusByDate");
            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRequestTypeCountByDate");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetRepairCountByStatusByDate");
            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");
        }
    }
}
