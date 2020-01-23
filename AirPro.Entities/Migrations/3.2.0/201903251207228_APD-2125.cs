using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2125 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "AutomaticInvoicesInd", c => c.Boolean(nullable: false));
            AddColumn("Access.ShopsArchive", "AutomaticInvoicesInd", c => c.Boolean(nullable: false));

            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveShop");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetShops");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessShopsArchive");

            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CloseRepairByRequestId");
        }
        
        public override void Down()
        {
            DropColumn("Access.ShopsArchive", "AutomaticInvoicesInd");
            DropColumn("Access.Shops", "AutomaticInvoicesInd");

            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveShop");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetShops");
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessShopsArchive");

            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_BuildReportData");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CloseRepairByRequestId");
        }
    }
}
