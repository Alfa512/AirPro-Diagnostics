using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD294 : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TABLE Access.Shops ADD ShopNumber AS ShopId + 10000;");
            Sql(@"ALTER TABLE Access.Shops ADD DisplayName AS Name + ' (' + CAST(ShopId + 10000 AS VARCHAR(10)) + ')';");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestGridByUser");

            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetOutstandingInvoices");
            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetPaymentTransactions");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetStatementReportDataSource");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetScanReportDataSource");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetInvoiceReportDataSource");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetEstimateReportDataSource");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetShopActivityReport");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_GetRepairsByUser");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetRequestGridByUser");

            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetOutstandingInvoices");
            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetPaymentTransactions");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetStatementReportDataSource");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetScanReportDataSource");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetInvoiceReportDataSource");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetEstimateReportDataSource");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetShopActivityReport");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");

            DropColumn("Access.Shops", "ShopNumber");
            DropColumn("Access.Shops", "DisplayName");
        }
    }
}
