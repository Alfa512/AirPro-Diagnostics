namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD673 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Scan.Requests", name: "RequestTypeCategoryId", newName: "RequestCategoryId");
            RenameIndex(table: "Scan.Requests", name: "IX_RequestTypeCategoryId", newName: "IX_RequestCategoryId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "Scan.Requests", name: "IX_RequestCategoryId", newName: "IX_RequestTypeCategoryId");
            RenameColumn(table: "Scan.Requests", name: "RequestCategoryId", newName: "RequestTypeCategoryId");
        }
    }
}
