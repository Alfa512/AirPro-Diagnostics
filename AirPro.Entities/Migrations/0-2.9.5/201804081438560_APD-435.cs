namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD435 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Access.ShopInsuranceCompanies",
                c => new
                    {
                        ShopId = c.Guid(nullable: false),
                        InsuranceCompanyId = c.Int(nullable: false),
                        PricingPlanId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShopId, t.InsuranceCompanyId })
                .ForeignKey("Repair.InsuranceCompanies", t => t.InsuranceCompanyId)
                .ForeignKey("Billing.PricingPlans", t => t.PricingPlanId)
                .ForeignKey("Access.Shops", t => t.ShopId)
                .Index(t => t.ShopId)
                .Index(t => t.InsuranceCompanyId)
                .Index(t => t.PricingPlanId);
        }
        
        public override void Down()
        {
            DropForeignKey("Access.ShopInsuranceCompanies", "ShopId", "Access.Shops");
            DropForeignKey("Access.ShopInsuranceCompanies", "PricingPlanId", "Billing.PricingPlans");
            DropForeignKey("Access.ShopInsuranceCompanies", "InsuranceCompanyId", "Repair.InsuranceCompanies");
            DropIndex("Access.ShopInsuranceCompanies", new[] { "PricingPlanId" });
            DropIndex("Access.ShopInsuranceCompanies", new[] { "InsuranceCompanyId" });
            DropIndex("Access.ShopInsuranceCompanies", new[] { "ShopId" });
            DropTable("Access.ShopInsuranceCompanies");
        }
    }
}
