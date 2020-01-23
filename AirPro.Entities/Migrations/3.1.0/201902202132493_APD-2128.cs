using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2128 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Diagnostic", "usp_GetDiagnosticQueueByShop");
            this.DropObjectIfExists(DropObjectType.View, "Diagnostic", "vwUploadQueue");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Diagnostic", "usp_GetDiagnosticQueueByShop");
            this.DropObjectIfExists(DropObjectType.View, "Diagnostic", "vwUploadQueue");
        }
    }
}
