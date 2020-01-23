using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1952 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Backups' AND TABLE_NAME = 'ScanReportsArchive')
                    OR NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Scan' AND TABLE_NAME = 'ReportsArchive' AND COLUMN_NAME = 'ArchiveId')
                        THROW 50000, 'Migration Script Required.', 1;");

            this.DropObjectIfExists(DropObjectType.Trigger, "Scan", "trgScanReportsArchive");
            
            this.DropObjectIfExists(DropObjectType.Trigger, "Scan", "trgScanRequestsArchive");
            AddColumn("Scan.RequestsArchive", "ShopContactGuid", c => c.Guid());

            this.DropObjectIfExists(DropObjectType.Trigger, "Inventory", "trgAirProToolArchive");
            AddColumn("Inventory.AirProToolsArchive", "OBD2YConnector", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "AELatestCode", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "ChargerStyle", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "TabletSerialNumber", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "WifiCard", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "WifiHardwareId", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "WifiDriverDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("Inventory.AirProToolsArchive", "WifiDriverVersion", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "ImageVersion", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "HondaVersion", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "FJDSVersion", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "TechstreamVersion", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "CellularActiveInd", c => c.Boolean(nullable: false));
            AddColumn("Inventory.AirProToolsArchive", "CellularProvider", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "CellularIMEI", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "WifiMacAddress", c => c.String(maxLength: 100));
            AddColumn("Inventory.AirProToolsArchive", "J2534Brand", c => c.Int());
            AddColumn("Inventory.AirProToolsArchive", "J2534Model", c => c.Int());
            AddColumn("Inventory.AirProToolsArchive", "Type", c => c.Int(nullable: false));
            AddColumn("Inventory.AirProToolsArchive", "J2534Serial", c => c.String(maxLength: 100));

            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessShopsArchive");
            AddColumn("Access.ShopsArchive", "AllowAllRepairAutoClose", c => c.Boolean(nullable: false));
            AddColumn("Access.ShopsArchive", "AllowScanAnalysisAutoClose", c => c.Boolean(nullable: false));
            AddColumn("Access.ShopsArchive", "BillingCycleId", c => c.Int());

            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessUsersArchive");
            AddColumn("Access.UsersArchive", "ShopStatementNotification", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessUsersArchive");
            DropColumn("Access.UsersArchive", "ShopStatementNotification");

            this.DropObjectIfExists(DropObjectType.Trigger, "Access", "trgAccessShopsArchive");
            DropColumn("Access.ShopsArchive", "BillingCycleId");
            DropColumn("Access.ShopsArchive", "AllowScanAnalysisAutoClose");
            DropColumn("Access.ShopsArchive", "AllowAllRepairAutoClose");

            this.DropObjectIfExists(DropObjectType.Trigger, "Inventory", "trgAirProToolArchive");
            DropColumn("Inventory.AirProToolsArchive", "J2534Serial");
            DropColumn("Inventory.AirProToolsArchive", "Type");
            DropColumn("Inventory.AirProToolsArchive", "J2534Model");
            DropColumn("Inventory.AirProToolsArchive", "J2534Brand");
            DropColumn("Inventory.AirProToolsArchive", "WifiMacAddress");
            DropColumn("Inventory.AirProToolsArchive", "CellularIMEI");
            DropColumn("Inventory.AirProToolsArchive", "CellularProvider");
            DropColumn("Inventory.AirProToolsArchive", "CellularActiveInd");
            DropColumn("Inventory.AirProToolsArchive", "TechstreamVersion");
            DropColumn("Inventory.AirProToolsArchive", "FJDSVersion");
            DropColumn("Inventory.AirProToolsArchive", "HondaVersion");
            DropColumn("Inventory.AirProToolsArchive", "ImageVersion");
            DropColumn("Inventory.AirProToolsArchive", "WifiDriverVersion");
            DropColumn("Inventory.AirProToolsArchive", "WifiDriverDate");
            DropColumn("Inventory.AirProToolsArchive", "WifiHardwareId");
            DropColumn("Inventory.AirProToolsArchive", "WifiCard");
            DropColumn("Inventory.AirProToolsArchive", "TabletSerialNumber");
            DropColumn("Inventory.AirProToolsArchive", "ChargerStyle");
            DropColumn("Inventory.AirProToolsArchive", "AELatestCode");
            DropColumn("Inventory.AirProToolsArchive", "OBD2YConnector");

            this.DropObjectIfExists(DropObjectType.Trigger, "Scan", "trgScanRequestsArchive");
            DropColumn("Scan.RequestsArchive", "ShopContactGuid");

            this.DropObjectIfExists(DropObjectType.Trigger, "Scan", "trgScanReportsArchive");
            DropTable("Scan.ReportsArchive");
        }
    }
}
