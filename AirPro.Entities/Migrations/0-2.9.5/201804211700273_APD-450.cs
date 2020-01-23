namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD450 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.Requests", "ToolId", c => c.Int());
            CreateIndex("Scan.Requests", "ToolId");
            AddForeignKey("Scan.Requests", "ToolId", "Inventory.AirProTools", "ToolId");
        }
        
        public override void Down()
        {
            DropForeignKey("Scan.Requests", "ToolId", "Inventory.AirProTools");
            DropIndex("Scan.Requests", new[] { "ToolId" });
            DropColumn("Scan.Requests", "ToolId");
        }
    }
}
