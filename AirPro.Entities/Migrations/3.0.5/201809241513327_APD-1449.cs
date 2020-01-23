using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1449 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inventory.AirProToolAccountsArchive",
                c => new
                    {
                        ArchiveId = c.Int(nullable: false, identity: true),
                        ArchiveDt = c.DateTimeOffset(nullable: false, precision: 7),
                        ToolId = c.Int(nullable: false),
                        AccountGuid = c.Guid(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ArchiveId);
            
            CreateTable(
                "Inventory.AirProToolShopsArchive",
                c => new
                    {
                        ArchiveId = c.Int(nullable: false, identity: true),
                        ArchiveDt = c.DateTimeOffset(nullable: false, precision: 7),
                        ToolId = c.Int(nullable: false),
                        ShopGuid = c.Guid(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ArchiveId);
            
            AddColumn("Inventory.AirProToolShops", "CreatedByUserGuid", c => c.Guid(nullable: false));
            AddColumn("Inventory.AirProToolShops", "CreatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Inventory.AirProToolShops", "UpdatedByUserGuid", c => c.Guid());
            AddColumn("Inventory.AirProToolShops", "UpdatedDt", c => c.DateTimeOffset(precision: 7));
            AddColumn("Inventory.AirProToolAccounts", "CreatedByUserGuid", c => c.Guid(nullable: false));
            AddColumn("Inventory.AirProToolAccounts", "CreatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Inventory.AirProToolAccounts", "UpdatedByUserGuid", c => c.Guid());
            AddColumn("Inventory.AirProToolAccounts", "UpdatedDt", c => c.DateTimeOffset(precision: 7));
            CreateIndex("Inventory.AirProToolShops", "CreatedByUserGuid");
            CreateIndex("Inventory.AirProToolShops", "UpdatedByUserGuid");
            CreateIndex("Inventory.AirProToolAccounts", "CreatedByUserGuid");
            CreateIndex("Inventory.AirProToolAccounts", "UpdatedByUserGuid");
            AddForeignKey("Inventory.AirProToolShops", "CreatedByUserGuid", "Access.Users", "UserGuid");
            AddForeignKey("Inventory.AirProToolAccounts", "CreatedByUserGuid", "Access.Users", "UserGuid");
            AddForeignKey("Inventory.AirProToolAccounts", "UpdatedByUserGuid", "Access.Users", "UserGuid");
            AddForeignKey("Inventory.AirProToolShops", "UpdatedByUserGuid", "Access.Users", "UserGuid");

            this.DropObjectIfExists(DropObjectType.Trigger, "Inventory", "trgAirProToolShopsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Inventory", "trgAirProToolAccountsArchive");
        }
        
        public override void Down()
        {
            DropForeignKey("Inventory.AirProToolShops", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Inventory.AirProToolAccounts", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Inventory.AirProToolAccounts", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Inventory.AirProToolShops", "CreatedByUserGuid", "Access.Users");
            DropIndex("Inventory.AirProToolAccounts", new[] { "UpdatedByUserGuid" });
            DropIndex("Inventory.AirProToolAccounts", new[] { "CreatedByUserGuid" });
            DropIndex("Inventory.AirProToolShops", new[] { "UpdatedByUserGuid" });
            DropIndex("Inventory.AirProToolShops", new[] { "CreatedByUserGuid" });
            DropColumn("Inventory.AirProToolAccounts", "UpdatedDt");
            DropColumn("Inventory.AirProToolAccounts", "UpdatedByUserGuid");
            DropColumn("Inventory.AirProToolAccounts", "CreatedDt");
            DropColumn("Inventory.AirProToolAccounts", "CreatedByUserGuid");
            DropColumn("Inventory.AirProToolShops", "UpdatedDt");
            DropColumn("Inventory.AirProToolShops", "UpdatedByUserGuid");
            DropColumn("Inventory.AirProToolShops", "CreatedDt");
            DropColumn("Inventory.AirProToolShops", "CreatedByUserGuid");
            DropTable("Inventory.AirProToolShopsArchive");
            DropTable("Inventory.AirProToolAccountsArchive");

            this.DropObjectIfExists(DropObjectType.Trigger, "Inventory", "trgAirProToolShopsArchive");
            this.DropObjectIfExists(DropObjectType.Trigger, "Inventory", "trgAirProToolAccountsArchive");
        }
    }
}
