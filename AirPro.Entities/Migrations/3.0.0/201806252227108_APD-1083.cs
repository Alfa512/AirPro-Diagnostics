using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1083 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetInvoiceReportDataSource");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetEstimateReportDataSource");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetInvoiceReportDataSource");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetEstimateReportDataSource");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
        }
    }
}
