namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD171 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inventory.AirProToolDeposits",
                c => new
                    {
                        ToolDepositId = c.Int(nullable: false, identity: true),
                        ToolId = c.Int(nullable: false),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        Description = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeleteInd = c.Boolean(nullable: false),
                        CreatedByUserGuid = c.Guid(nullable: false),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedByUserGuid = c.Guid(),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => new { t.ToolDepositId, t.ToolId })
                .ForeignKey("Inventory.AirProTools", t => t.ToolId, cascadeDelete: true)
                .ForeignKey("Access.Users", t => t.CreatedByUserGuid)
                .ForeignKey("Access.Users", t => t.UpdatedByUserGuid)
                .Index(t => t.ToolId)
                .Index(t => t.CreatedByUserGuid)
                .Index(t => t.UpdatedByUserGuid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Inventory.AirProToolDeposits", "UpdatedByUserGuid", "Access.Users");
            DropForeignKey("Inventory.AirProToolDeposits", "CreatedByUserGuid", "Access.Users");
            DropForeignKey("Inventory.AirProToolDeposits", "ToolId", "Inventory.AirProTools");
            DropIndex("Inventory.AirProToolDeposits", new[] { "UpdatedByUserGuid" });
            DropIndex("Inventory.AirProToolDeposits", new[] { "CreatedByUserGuid" });
            DropIndex("Inventory.AirProToolDeposits", new[] { "ToolId" });
            DropTable("Inventory.AirProToolDeposits");
        }
    }
}
