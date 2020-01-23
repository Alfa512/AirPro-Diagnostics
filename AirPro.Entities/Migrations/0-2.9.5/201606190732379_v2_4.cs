namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Scan.RequestWarningIndicators",
                c => new
                    {
                        RequestWarningIndicatorID = c.Int(nullable: false, identity: true),
                        RequestID = c.Int(nullable: false),
                        WarningIndicatorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequestWarningIndicatorID)
                .ForeignKey("Scan.Requests", t => t.RequestID, cascadeDelete: true)
                .ForeignKey("Scan.WarningIndicators", t => t.WarningIndicatorID, cascadeDelete: true)
                .Index(t => t.RequestID)
                .Index(t => t.WarningIndicatorID);
            
            CreateTable(
                "Scan.WarningIndicators",
                c => new
                    {
                        WarningIndicatorID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.WarningIndicatorID);
            
            CreateTable(
                "Scan.RequestTypes",
                c => new
                    {
                        RequestTypeID = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        ActiveFlag = c.Boolean(nullable: false),
                        BillableFlag = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RequestTypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Scan.RequestWarningIndicators", "WarningIndicatorID", "Scan.WarningIndicators");
            DropForeignKey("Scan.RequestWarningIndicators", "RequestID", "Scan.Requests");
            DropIndex("Scan.RequestWarningIndicators", new[] { "WarningIndicatorID" });
            DropIndex("Scan.RequestWarningIndicators", new[] { "RequestID" });
            DropTable("Scan.RequestTypes");
            DropTable("Scan.WarningIndicators");
            DropTable("Scan.RequestWarningIndicators");
        }
    }
}
