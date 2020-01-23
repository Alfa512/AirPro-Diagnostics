namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1285 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Inventory.AirProTools", "Type", c => c.Int(nullable: false, defaultValue: 1));

            DropColumn("Inventory.AirProTools", "ToolName");
            Sql("ALTER TABLE Inventory.AirProTools ADD ToolName AS CASE [Type] WHEN 2 THEN 'FieldPro' WHEN 3 THEN 'EuroPro' ELSE 'AirPro' END + RIGHT('0000' + CAST(ToolId AS VARCHAR(10)), 4);");
        }
        
        public override void Down()
        {
            DropColumn("Inventory.AirProTools", "ToolName");
            Sql("ALTER TABLE Inventory.AirProTools ADD ToolName AS 'AirPro' + RIGHT('0000' + CAST(ToolId AS VARCHAR(10)), 4);");

            DropColumn("Inventory.AirProTools", "Type");
        }
    }
}
