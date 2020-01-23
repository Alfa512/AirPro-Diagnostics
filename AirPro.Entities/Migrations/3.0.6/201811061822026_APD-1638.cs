namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;

    public partial class APD1638 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
        }
    }
}
