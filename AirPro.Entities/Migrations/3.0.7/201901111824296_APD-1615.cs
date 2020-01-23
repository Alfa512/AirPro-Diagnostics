namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1615 : DbMigration
    {
        public override void Up()
        {
            Sql("DBCC CHECKIDENT ('Inventory.AirProTools', RESEED, 13600);");
            Sql("ALTER TABLE Inventory.AirProTools DROP COLUMN ToolName;");
            Sql("ALTER TABLE Inventory.AirProTools ADD ToolName AS CASE Type WHEN 2 THEN 'FP' WHEN 3 THEN 'EP' ELSE 'AP' END + RIGHT('00000' + CAST(ToolId AS VARCHAR(10)), 5);");
        }
        
        public override void Down()
        {
            Sql("ALTER TABLE Inventory.AirProTools DROP COLUMN ToolName");
            Sql("ALTER TABLE Inventory.AirProTools ADD ToolName AS CASE Type WHEN 2 THEN 'FieldPro' WHEN 3 THEN 'EuroPro' ELSE 'AirPro' END + RIGHT('0000' + CAST(ToolId AS VARCHAR(10)), 4);");
        }
    }
}
