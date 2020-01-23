using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Scan.Uploads", name: "Request_RequestID", newName: "RequestId");
            RenameIndex(table: "Scan.Uploads", name: "IX_Request_RequestID", newName: "IX_RequestId");
            AddColumn("Scan.Reports", "ReportCompleted", c => c.Boolean(nullable: true));
            Sql("UPDATE Scan.Reports SET ReportCompleted = 0;");
            AlterColumn("Scan.Reports", "ReportCompleted", c => c.Boolean(nullable: false));

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201606122234174_v2_1.sql");
        }
        
        public override void Down()
        {
            DropColumn("Scan.Reports", "ReportCompleted");
            RenameIndex(table: "Scan.Uploads", name: "IX_RequestId", newName: "IX_Request_RequestID");
            RenameColumn(table: "Scan.Uploads", name: "RequestId", newName: "Request_RequestID");
        }
    }
}
