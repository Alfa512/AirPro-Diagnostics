using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v288 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Scan.Uploads", newName: "UploadXmls");
            RenameTable(name: "Support.NotificationLog", newName: "NotificationLogs");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201612282057480_v2.8.8_Deploy.sql");
        }

        public override void Down()
        {
            RenameTable(name: "Support.NotificationLogs", newName: "NotificationLog");
            RenameTable(name: "Scan.UploadXmls", newName: "Uploads");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201612282057480_v2.8.8_Rollback.sql");
        }
    }
}
