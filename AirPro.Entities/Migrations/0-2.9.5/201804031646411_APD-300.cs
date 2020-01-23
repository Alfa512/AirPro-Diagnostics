namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD300 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inventory.AirProToolsArchive",
                c => new
                    {
                        ToolArchiveId = c.Int(nullable: false, identity: true),
                        ToolId = c.Int(nullable: false),
                        ShopGuid = c.Guid(),
                        ToolPassword = c.String(maxLength: 20),
                        AutoEnginuityNum = c.String(maxLength: 20),
                        AutoEnginuityVersion = c.String(maxLength: 20),
                        CarDaqNum = c.String(maxLength: 20),
                        DGNum = c.String(maxLength: 20),
                        TeamViewerId = c.String(maxLength: 20),
                        TeamViewerPassword = c.String(maxLength: 20),
                        GMUsername = c.String(maxLength: 20),
                        GMPassword = c.String(maxLength: 20),
                        WindowsVersion = c.String(maxLength: 100),
                        TabletModel = c.String(maxLength: 100),
                        HubModel = c.String(maxLength: 200),
                        IPV6DisabledInd = c.Boolean(nullable: false),
                        OneDriveSyncEnabledInd = c.Boolean(nullable: false),
                        UpdatesServiceInd = c.Boolean(nullable: false),
                        MeteredConnectionInd = c.Boolean(nullable: false),
                        SelfScanEnabledInd = c.Boolean(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ToolArchiveId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Access.Shops", t => t.ShopGuid)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.ToolId)
                .Index(t => t.ShopGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            CreateTable(
                "Inventory.AirProTools",
                c => new
                    {
                        ToolId = c.Int(nullable: false, identity: true),
                        ShopGuid = c.Guid(),
                        ToolPassword = c.String(maxLength: 20),
                        AutoEnginuityNum = c.String(maxLength: 20),
                        AutoEnginuityVersion = c.String(maxLength: 20),
                        CarDaqNum = c.String(maxLength: 20),
                        DGNum = c.String(maxLength: 20),
                        TeamViewerId = c.String(maxLength: 20),
                        TeamViewerPassword = c.String(maxLength: 20),
                        GMUsername = c.String(maxLength: 20),
                        GMPassword = c.String(maxLength: 20),
                        WindowsVersion = c.String(maxLength: 100),
                        TabletModel = c.String(maxLength: 100),
                        HubModel = c.String(maxLength: 200),
                        IPV6DisabledInd = c.Boolean(nullable: false),
                        OneDriveSyncEnabledInd = c.Boolean(nullable: false),
                        UpdatesServiceInd = c.Boolean(nullable: false),
                        MeteredConnectionInd = c.Boolean(nullable: false),
                        SelfScanEnabledInd = c.Boolean(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ToolId)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Access.Shops", t => t.ShopGuid)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.ShopGuid)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
            Sql(@"CREATE TRIGGER Inventory.trgAirProToolArchive
	            ON Inventory.AirProTools FOR UPDATE, DELETE
                AS
	                INSERT INTO Inventory.AirProToolsArchive
	                (
		                ToolId
		                ,ShopGuid
		                ,ToolPassword
		                ,AutoEnginuityNum
		                ,AutoEnginuityVersion
		                ,CarDaqNum
		                ,DGNum
		                ,TeamViewerId
		                ,TeamViewerPassword
		                ,GMUsername
		                ,GMPassword
		                ,WindowsVersion
		                ,TabletModel
		                ,HubModel
		                ,IPV6DisabledInd
		                ,OneDriveSyncEnabledInd
		                ,UpdatesServiceInd
		                ,MeteredConnectionInd
		                ,SelfScanEnabledInd
		                ,CreatedByUserGuid
		                ,CreatedDt
		                ,UpdatedByUserGuid
		                ,UpdatedDt
	                )
	                SELECT
		                ToolId
		                ,ShopGuid
		                ,ToolPassword
		                ,AutoEnginuityNum
		                ,AutoEnginuityVersion
		                ,CarDaqNum
		                ,DGNum
		                ,TeamViewerId
		                ,TeamViewerPassword
		                ,GMUsername
		                ,GMPassword
		                ,WindowsVersion
		                ,TabletModel
		                ,HubModel
		                ,IPV6DisabledInd
		                ,OneDriveSyncEnabledInd
		                ,UpdatesServiceInd
		                ,MeteredConnectionInd
		                ,SelfScanEnabledInd
		                ,CreatedByUserGuid
		                ,CreatedDt
		                ,UpdatedByUserGuid
		                ,UpdatedDt
	                FROM DELETED");
        }
        
        public override void Down()
        {
            DropForeignKey("Inventory.AirProTools", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Inventory.AirProTools", "ShopGuid", "Access.Shops");
            DropForeignKey("Inventory.AirProTools", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Inventory.AirProToolsArchive", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Inventory.AirProToolsArchive", "ShopGuid", "Access.Shops");
            DropForeignKey("Inventory.AirProToolsArchive", "CreatedByUserGuid", "Access.Users");
            DropIndex("Inventory.AirProTools", new[] { "UpdatedByUserGuid" });
            DropIndex("Inventory.AirProTools", new[] { "CreatedByUserGuid" });
            DropIndex("Inventory.AirProTools", new[] { "ShopGuid" });
            DropIndex("Inventory.AirProToolsArchive", new[] { "UpdatedByUserGuid" });
            DropIndex("Inventory.AirProToolsArchive", new[] { "CreatedByUserGuid" });
            DropIndex("Inventory.AirProToolsArchive", new[] { "ShopGuid" });
            DropIndex("Inventory.AirProToolsArchive", new[] { "ToolId" });
            DropTable("Inventory.AirProTools");
            DropTable("Inventory.AirProToolsArchive");
        }
    }
}
