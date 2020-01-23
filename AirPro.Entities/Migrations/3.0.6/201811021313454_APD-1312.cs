namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;

    public partial class APD1312 : DbMigration
    {
        public override void Up()
        {
            DropIndex("Repair.Orders", new[] { "CCCDocumentGuid" });
            CreateIndex("Repair.Orders", "CCCDocumentGuid");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CreateFromCCCEstimates");
        }
        
        public override void Down()
        {
            DropIndex("Repair.Orders", new[] { "CCCDocumentGuid" });
            CreateIndex("Repair.Orders", "CCCDocumentGuid");

            this.DropObjectIfExists(DropObjectType.Procedure, "Repair", "usp_CreateFromCCCEstimates");
        }
    }
}
