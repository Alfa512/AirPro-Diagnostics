using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1112 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Inventory.AirProToolsArchive", "ShopGuid", "Access.Shops");
            DropForeignKey("Inventory.AirProTools", "ShopGuid", "Access.Shops");
            DropIndex("Inventory.AirProToolsArchive", new[] { "ShopGuid" });
            DropIndex("Inventory.AirProTools", new[] { "ShopGuid" });
            CreateTable(
                "Inventory.AirProToolAccounts",
                c => new
                    {
                        ToolId = c.Int(nullable: false),
                        AccountGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ToolId, t.AccountGuid })
                .ForeignKey("Access.Accounts", t => t.AccountGuid, cascadeDelete: true)
                .ForeignKey("Inventory.AirProTools", t => t.ToolId, cascadeDelete: true)
                .Index(t => t.ToolId)
                .Index(t => t.AccountGuid);
            
            CreateTable(
                "Inventory.AirProToolShops",
                c => new
                    {
                        ToolId = c.Int(nullable: false),
                        ShopGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ToolId, t.ShopGuid })
                .ForeignKey("Access.Shops", t => t.ShopGuid, cascadeDelete: true)
                .ForeignKey("Inventory.AirProTools", t => t.ToolId, cascadeDelete: true)
                .Index(t => t.ToolId)
                .Index(t => t.ShopGuid);
            
            AddColumn("Inventory.AirProToolsArchive", "ToolKey", c => c.Guid(nullable: false));
            AddColumn("Inventory.AirProTools", "ToolKey", c => c.Guid(nullable: false, defaultValueSql: "NEWID()"));
            CreateIndex("Inventory.AirProTools", "ToolKey");

            Sql(@"MERGE Inventory.AirProToolShops AS t
                USING (SELECT ToolId, ShopGuid FROM Inventory.AirProTools WHERE ShopGuid IS NOT NULL) AS s
	                ON t.ToolId = s.ToolId AND t.ShopGuid = s.ShopGuid
                WHEN NOT MATCHED THEN
	                INSERT (ToolId, ShopGuid)
	                VALUES (ToolId, ShopGuid)
                OUTPUT inserted.*;");

            DropColumn("Inventory.AirProToolsArchive", "ShopGuid");
            DropColumn("Inventory.AirProTools", "ShopGuid");

            this.DropObjectIfExists(type: DropObjectType.Procedure, schema: "Inventory", name: "GetAirProToolStats");
            this.DropObjectIfExists(type: DropObjectType.Procedure, schema: "Inventory", name: "usp_GetAirProToolStats");
            this.DropObjectIfExists(type: DropObjectType.Trigger, schema: "Inventory", name: "trgAirProToolArchive");
        }

        public override void Down()
        {
            AddColumn("Inventory.AirProTools", "ShopGuid", c => c.Guid());
            AddColumn("Inventory.AirProToolsArchive", "ShopGuid", c => c.Guid());

            Sql(@"UPDATE t
	                SET t.ShopGuid = s.ShopGuid
                FROM Inventory.AirProTools t
                INNER JOIN Inventory.AirProToolShops s
	                ON t.ToolId = s.ToolId");

            DropForeignKey("Inventory.AirProToolShops", "ToolId", "Inventory.AirProTools");
            DropForeignKey("Inventory.AirProToolShops", "ShopGuid", "Access.Shops");
            DropForeignKey("Inventory.AirProToolAccounts", "ToolId", "Inventory.AirProTools");
            DropForeignKey("Inventory.AirProToolAccounts", "AccountGuid", "Access.Accounts");
            DropIndex("Inventory.AirProToolShops", new[] { "ShopGuid" });
            DropIndex("Inventory.AirProToolShops", new[] { "ToolId" });
            DropIndex("Inventory.AirProTools", new[] { "ToolKey" });
            DropIndex("Inventory.AirProToolAccounts", new[] { "AccountGuid" });
            DropIndex("Inventory.AirProToolAccounts", new[] { "ToolId" });
            DropColumn("Inventory.AirProTools", "ToolKey");
            DropColumn("Inventory.AirProToolsArchive", "ToolKey");
            DropTable("Inventory.AirProToolShops");
            DropTable("Inventory.AirProToolAccounts");
            CreateIndex("Inventory.AirProTools", "ShopGuid");
            CreateIndex("Inventory.AirProToolsArchive", "ShopGuid");
            AddForeignKey("Inventory.AirProTools", "ShopGuid", "Access.Shops", "ShopGuid");
            AddForeignKey("Inventory.AirProToolsArchive", "ShopGuid", "Access.Shops", "ShopGuid");

            this.DropObjectIfExists(type: DropObjectType.Procedure, schema: "Inventory", name: "GetAirProToolStats");
            this.DropObjectIfExists(type: DropObjectType.Procedure, schema: "Inventory", name: "usp_GetAirProToolStats");
            this.DropObjectIfExists(type: DropObjectType.Trigger, schema: "Inventory", name: "trgAirProToolArchive");
        }
    }
}
