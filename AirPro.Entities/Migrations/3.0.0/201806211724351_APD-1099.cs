using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class APD1099 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "HideFromReports", c => c.Boolean(nullable: false));

            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Table, "Reporting", "ReportData");
            this.DropObjectIfExists(DropObjectType.View, "Reporting", "vwReportData");

            DropColumn("Access.Shops", "HideFromReports");
        }
    }
}
