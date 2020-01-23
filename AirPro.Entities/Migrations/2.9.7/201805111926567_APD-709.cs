namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD709 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inventory.AirProToolSubscriptions",
                c => new
                    {
                        ToolSubscriptionId = c.Int(nullable: false, identity: true),
                        ToolId = c.Int(nullable: false),
                        Vendor = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => new { t.ToolSubscriptionId, t.ToolId })
                .ForeignKey("Inventory.AirProTools", t => t.ToolId)
                .Index(t => t.ToolId);

            Sql(@"INSERT INTO [Inventory].[AirProToolSubscriptions] ([ToolId], [Vendor], [Username], [Password]) 
                    SELECT ToolId, 'GM', GMUsername, GMPassword FROM Inventory.AirProTools  
                    WHERE GMUsername IS NOT NULL AND GMPassword IS NOT NULL");
            
            AddColumn("Inventory.AirProTools", "OBD2YConnector", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProTools", "AELatestCode", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProTools", "ChargerStyle", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProTools", "TabletSerialNumber", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProTools", "WifiCard", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProTools", "WifiHardwareId", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProTools", "WifiDriverDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("Inventory.AirProTools", "WifiDriverVersion", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProTools", "ImageVersion", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProTools", "HondaVersion", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProTools", "FJDSVersion", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProTools", "TechstreamVersion", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProTools", "CellularActiveInd", c => c.Boolean(nullable: false));
            AddColumn("Inventory.AirProTools", "CellularProvider", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProTools", "CellularIMEI", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProTools", "WifiMacAddress", c => c.String(maxLength: 100));
            DropColumn("Inventory.AirProToolsArchive", "GMUsername");
            DropColumn("Inventory.AirProToolsArchive", "GMPassword");
            DropColumn("Inventory.AirProTools", "GMUsername");
            DropColumn("Inventory.AirProTools", "GMPassword");

            Sql("IF EXISTS(SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Inventory].[trgAirProToolArchive]')) DROP TRIGGER [Inventory].[trgAirProToolArchive]");
        }
        
        public override void Down()
        {
            AddColumn("Inventory.AirProTools", "GMPassword", c => c.String(maxLength: 20));
            AddColumn("Inventory.AirProTools", "GMUsername", c => c.String(maxLength: 20));
            AddColumn("Inventory.AirProToolsArchive", "GMPassword", c => c.String(maxLength: 20));
            AddColumn("Inventory.AirProToolsArchive", "GMUsername", c => c.String(maxLength: 20));
            DropForeignKey("Inventory.AirProToolSubscriptions", "ToolId", "Inventory.AirProTools");
            DropIndex("Inventory.AirProToolSubscriptions", new[] { "ToolId" });
            DropColumn("Inventory.AirProTools", "WifiMacAddress");
            DropColumn("Inventory.AirProTools", "CellularIMEI");
            DropColumn("Inventory.AirProTools", "CellularProvider");
            DropColumn("Inventory.AirProTools", "CellularActiveInd");
            DropColumn("Inventory.AirProTools", "TechstreamVersion");
            DropColumn("Inventory.AirProTools", "FJDSVersion");
            DropColumn("Inventory.AirProTools", "HondaVersion");
            DropColumn("Inventory.AirProTools", "ImageVersion");
            DropColumn("Inventory.AirProTools", "WifiDriverVersion");
            DropColumn("Inventory.AirProTools", "WifiDriverDate");
            DropColumn("Inventory.AirProTools", "WifiHardwareId");
            DropColumn("Inventory.AirProTools", "WifiCard");
            DropColumn("Inventory.AirProTools", "TabletSerialNumber");
            DropColumn("Inventory.AirProTools", "ChargerStyle");
            DropColumn("Inventory.AirProTools", "AELatestCode");
            DropColumn("Inventory.AirProTools", "OBD2YConnector");
            DropTable("Inventory.AirProToolSubscriptions");

            Sql("IF EXISTS(SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Inventory].[trgAirProToolArchive]')) DROP TRIGGER [Inventory].[trgAirProToolArchive]");
        }
    }
}
