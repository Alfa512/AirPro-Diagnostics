using UniMatrix.Common.Enumerations;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD1091 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Billing.ShopInsuranceCompaniesEstimate",
                c => new
                    {
                        ShopId = c.Guid(nullable: false),
                        InsuranceCompanyId = c.Int(nullable: false),
                        EstimatePlanId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShopId, t.InsuranceCompanyId })
                .ForeignKey("Billing.EstimatePlans", t => t.EstimatePlanId, cascadeDelete: true)
                .ForeignKey("Repair.InsuranceCompanies", t => t.InsuranceCompanyId, cascadeDelete: true)
                .ForeignKey("Access.Shops", t => t.ShopId, cascadeDelete: true)
                .Index(t => t.ShopId)
                .Index(t => t.InsuranceCompanyId)
                .Index(t => t.EstimatePlanId);

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetEstimateReportDataSource");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetEstimateReportDataSource");

            DropForeignKey("Billing.ShopInsuranceCompaniesEstimate", "ShopId", "Access.Shops");
            DropForeignKey("Billing.ShopInsuranceCompaniesEstimate", "InsuranceCompanyId", "Repair.InsuranceCompanies");
            DropForeignKey("Billing.ShopInsuranceCompaniesEstimate", "EstimatePlanId", "Billing.EstimatePlans");
            DropIndex("Billing.ShopInsuranceCompaniesEstimate", new[] { "EstimatePlanId" });
            DropIndex("Billing.ShopInsuranceCompaniesEstimate", new[] { "InsuranceCompanyId" });
            DropIndex("Billing.ShopInsuranceCompaniesEstimate", new[] { "ShopId" });
            DropTable("Billing.ShopInsuranceCompaniesEstimate");
        }
    }
}
