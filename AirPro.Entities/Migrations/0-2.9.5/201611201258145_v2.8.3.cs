using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v283 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.Reports", "ResponsibleTechnicianID", c => c.String(maxLength: 128));
            AddColumn("Scan.Reports", "ResponsibleSetDt", c => c.DateTimeOffset(precision: 7));
            CreateIndex("Scan.Reports", "ResponsibleTechnicianID");
            AddForeignKey("Scan.Reports", "ResponsibleTechnicianID", "Access.Users", "UserId");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201611201258145_v2.8.3.sql");
        }

        public override void Down()
        {
            DropForeignKey("Scan.Reports", "ResponsibleTechnicianID", "Access.Users");
            DropIndex("Scan.Reports", new[] { "ResponsibleTechnicianID" });
            DropColumn("Scan.Reports", "ResponsibleSetDt");
            DropColumn("Scan.Reports", "ResponsibleTechnicianID");

            Sql(@"IF (OBJECT_ID('Scan.trgScanReportsArchive') IS NOT NULL) DROP TRIGGER Scan.trgScanReportsArchive;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ReportsArchive') DROP TABLE [Scan].[ReportsArchive];");
        }
    }
}
