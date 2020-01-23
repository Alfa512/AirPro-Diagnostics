using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2215 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetBillingInvoice");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetInvoiceReportDataSource");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetStatementReportDataSource");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetBillingInvoice");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetInvoiceReportDataSource");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetStatementReportDataSource");
        }
    }
}
