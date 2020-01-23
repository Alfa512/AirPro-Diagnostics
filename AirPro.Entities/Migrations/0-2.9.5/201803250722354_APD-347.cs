namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD347 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Scan.RequestCategoryTypes",
                c => new
                    {
                        RequestCategoryId = c.Int(nullable: false),
                        RequestTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RequestCategoryId, t.RequestTypeId })
                .ForeignKey("Scan.RequestCategories", t => t.RequestCategoryId)
                .ForeignKey("Scan.RequestTypes", t => t.RequestTypeId)
                .Index(t => t.RequestCategoryId)
                .Index(t => t.RequestTypeId);
            
            CreateTable(
                "Scan.RequestCategories",
                c => new
                    {
                        RequestCategoryId = c.Int(nullable: false, identity: true),
                        RequestCategoryName = c.String(),
                        Order = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RequestCategoryId);
            
            AddColumn("Scan.Requests", "RequestTypeCategoryId", c => c.Int());
            CreateIndex("Scan.Requests", "RequestTypeCategoryId");
            AddForeignKey("Scan.Requests", "RequestTypeCategoryId", "Scan.RequestCategories", "RequestCategoryId");
        }
        
        public override void Down()
        {
            DropForeignKey("Scan.Requests", "RequestTypeCategoryId", "Scan.RequestCategories");
            DropForeignKey("Scan.RequestCategoryTypes", "RequestTypeId", "Scan.RequestTypes");
            DropForeignKey("Scan.RequestCategoryTypes", "RequestCategoryId", "Scan.RequestCategories");
            DropIndex("Scan.Requests", new[] { "RequestTypeCategoryId" });
            DropIndex("Scan.RequestCategoryTypes", new[] { "RequestTypeId" });
            DropIndex("Scan.RequestCategoryTypes", new[] { "RequestCategoryId" });
            DropColumn("Scan.Requests", "RequestTypeCategoryId");
            DropTable("Scan.RequestCategories");
            DropTable("Scan.RequestCategoryTypes");
        }
    }
}
