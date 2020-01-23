using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD581 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.ReportWorkTypes", "InvoicedInd", c => c.Boolean(nullable: false));
            AddColumn("Scan.ReportWorkTypes", "InvoicedByUserGuid", c => c.Guid());
            AddColumn("Scan.ReportWorkTypes", "InvoiceAmount", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("Scan.ReportWorkTypes", "InvoicedByUserGuid");
            AddForeignKey("Scan.ReportWorkTypes", "InvoicedByUserGuid", "Access.Users", "UserGuid");

            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetBillingInvoice");
            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetOutstandingInvoices");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetInvoiceReportDataSource");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetStatementReportDataSource");
            this.DropObjectIfExists(DropObjectType.Function, "Billing", "udf_GetPricingPlanIdByOrderId");
        }

        public override void Down()
        {
            DropForeignKey("Scan.ReportWorkTypes", "InvoicedByUserGuid", "Access.Users");
            DropIndex("Scan.ReportWorkTypes", new[] { "InvoicedByUserGuid" });
            DropColumn("Scan.ReportWorkTypes", "InvoiceAmount");
            DropColumn("Scan.ReportWorkTypes", "InvoicedByUserGuid");
            DropColumn("Scan.ReportWorkTypes", "InvoicedInd");

            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetBillingInvoice");
            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetOutstandingInvoices");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetInvoiceReportDataSource");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetStatementReportDataSource");
            this.DropObjectIfExists(DropObjectType.Function, "Billing", "udf_GetPricingPlanIdByOrderId");
        }
    }
}
