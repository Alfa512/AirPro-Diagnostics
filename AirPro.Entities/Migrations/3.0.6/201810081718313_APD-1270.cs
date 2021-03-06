using UniMatrix.Common.Enumerations;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Extensions;

    public partial class APD1270 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetBillingInvoice");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetBillingInvoice");
        }
    }
}
