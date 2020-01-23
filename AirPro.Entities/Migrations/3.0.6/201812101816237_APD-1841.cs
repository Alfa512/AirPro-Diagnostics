using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1841 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Support.ApplicationExceptions", "ExceptionObjectInfo", c => c.String());

            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetBillingInvoice");
            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetBillingInvoiceGrid");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_RunNightlyProcess");
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CloseRepairsByAge");
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CloseRepairByRequestId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_SaveApplicationException");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetBillingInvoice");
            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetBillingInvoiceGrid");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_GetReportByRequestId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Scan", "usp_SaveReport");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_RunNightlyProcess");
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CloseRepairsByAge");
            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CloseRepairByRequestId");
            this.DropObjectIfExists(DropObjectType.Procedure, "Support", "usp_SaveApplicationException");

            DropColumn("Support.ApplicationExceptions", "ExceptionObjectInfo");
        }
    }
}
