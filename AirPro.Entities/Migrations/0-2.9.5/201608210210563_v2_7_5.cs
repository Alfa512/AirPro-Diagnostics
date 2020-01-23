namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_7_5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.RequestTypes", "SortOrder", c => c.Int(nullable: false));
            AddColumn("Scan.RequestTypes", "NextRequestTypeID", c => c.Int());
            CreateIndex("Scan.Requests", "TypeOfScan");
            CreateIndex("Scan.RequestTypes", "NextRequestTypeID");
            AddForeignKey("Scan.RequestTypes", "NextRequestTypeID", "Scan.RequestTypes", "RequestTypeID");
            AddForeignKey("Scan.Requests", "TypeOfScan", "Scan.RequestTypes", "RequestTypeID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Scan.Requests", "TypeOfScan", "Scan.RequestTypes");
            DropForeignKey("Scan.RequestTypes", "NextRequestTypeID", "Scan.RequestTypes");
            DropIndex("Scan.RequestTypes", new[] { "NextRequestTypeID" });
            DropIndex("Scan.Requests", new[] { "TypeOfScan" });
            DropColumn("Scan.RequestTypes", "NextRequestTypeID");
            DropColumn("Scan.RequestTypes", "SortOrder");
        }
    }
}
