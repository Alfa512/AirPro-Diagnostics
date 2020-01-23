namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD812 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Access.Shops", "AllowScanAnalysis", c => c.Boolean(nullable: false));
            AddColumn("Access.Shops", "DefaultInsuranceCompanyId", c => c.Int());
            CreateIndex("Access.Shops", "DefaultInsuranceCompanyId");
            AddForeignKey("Access.Shops", "DefaultInsuranceCompanyId", "Repair.InsuranceCompanies", "InsuranceCompanyId");

            Sql(@"SET IDENTITY_INSERT Scan.RequestTypes ON;
                INSERT INTO Scan.RequestTypes (RequestTypeId, TypeName, ActiveFlag, BillableFlag, SortOrder, CreatedByUserGuid, CreatedDt)
                SELECT 7, 'Scan Analysis', 1, 1, 7, '00000000-0000-0000-0000-000000000000', GETUTCDATE()
                WHERE NOT EXISTS (SELECT 1 FROM Scan.RequestTypes WHERE RequestTypeId = 7)
                SET IDENTITY_INSERT Scan.RequestTypes OFF;");

            Sql(@"INSERT INTO Scan.RequestCategoryTypes (RequestCategoryId, RequestTypeId)
                SELECT 1, 7 WHERE NOT EXISTS (SELECT 1 FROM Scan.RequestCategoryTypes WHERE RequestCategoryId = 1 AND RequestTypeId = 7)");
        }
        
        public override void Down()
        {
            DropForeignKey("Access.Shops", "DefaultInsuranceCompanyId", "Repair.InsuranceCompanies");
            DropIndex("Access.Shops", new[] { "DefaultInsuranceCompanyId" });
            DropColumn("Access.Shops", "DefaultInsuranceCompanyId");
            DropColumn("Access.Shops", "AllowScanAnalysis");
        }
    }
}
