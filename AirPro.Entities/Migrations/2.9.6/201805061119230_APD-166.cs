namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD166 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Repair.InsuranceCompanies", "ProgramName", c => c.String(maxLength: 128));
            MoveTable(name: "Access.ShopInsuranceCompanies", newSchema: "Billing");
            RenameTable(name: "Billing.ShopInsuranceCompanies", newName: "ShopInsuranceCompaniesPricing");

            CreateTable(
                    "Access.ShopInsuranceCompanies",
                    c => new
                    {
                        ShopId = c.Guid(nullable: false),
                        InsuranceCompanyId = c.Int(nullable: false)
                    })
                .PrimaryKey(t => new { t.ShopId, t.InsuranceCompanyId })
                .ForeignKey("Repair.InsuranceCompanies", t => t.InsuranceCompanyId)
                .ForeignKey("Access.Shops", t => t.ShopId)
                .Index(t => t.ShopId)
                .Index(t => t.InsuranceCompanyId);

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_GetInsuranceCompanies') DROP PROCEDURE Repair.usp_GetInsuranceCompanies;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_SaveInsuranceCompany') DROP PROCEDURE Repair.usp_SaveInsuranceCompany;");
        }

        public override void Down()
        {
            DropForeignKey("Access.ShopInsuranceCompanies", "ShopId", "Access.Shops");
            DropForeignKey("Access.ShopInsuranceCompanies", "InsuranceCompanyId", "Repair.InsuranceCompanies");
            DropIndex("Access.ShopInsuranceCompanies", new[] { "InsuranceCompanyId" });
            DropIndex("Access.ShopInsuranceCompanies", new[] { "ShopId" });
            DropTable("Access.ShopInsuranceCompanies");

            DropColumn("Repair.InsuranceCompanies", "ProgramName");
            RenameTable(name: "Billing.ShopInsuranceCompaniesPricing", newName: "ShopInsuranceCompanies");
            MoveTable(name: "Billing.ShopInsuranceCompanies", newSchema: "Access");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_GetInsuranceCompanies') DROP PROCEDURE Repair.usp_GetInsuranceCompanies;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Repair' AND ROUTINE_NAME = 'usp_SaveInsuranceCompany') DROP PROCEDURE Repair.usp_SaveInsuranceCompany;");
        }
    }
}
