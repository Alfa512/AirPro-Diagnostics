namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD604 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Billing.Currencies",
                c => new
                    {
                        CurrencyId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CurrencyId);
            
            AddColumn("Billing.PricingPlans", "CurrencyId", c => c.Int());
            AddColumn("Billing.Payments", "CurrencyId", c => c.Int());
            AddColumn("Repair.Invoices", "CurrencyId", c => c.Int());
            CreateIndex("Billing.PricingPlans", "CurrencyId");
            CreateIndex("Billing.Payments", "CurrencyId");
            CreateIndex("Repair.Invoices", "CurrencyId");
            AddForeignKey("Billing.PricingPlans", "CurrencyId", "Billing.Currencies", "CurrencyId");
            AddForeignKey("Billing.Payments", "CurrencyId", "Billing.Currencies", "CurrencyId");
            AddForeignKey("Repair.Invoices", "CurrencyId", "Billing.Currencies", "CurrencyId");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Billing' AND ROUTINE_NAME = 'usp_GetPricingPlans') DROP PROCEDURE Billing.usp_GetPricingPlans;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Billing' AND ROUTINE_NAME = 'usp_SavePricingPlan') DROP PROCEDURE Billing.usp_SavePricingPlan;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Billing' AND ROUTINE_NAME = 'usp_GetOutstandingInvoices') DROP PROCEDURE Billing.usp_GetOutstandingInvoices;");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetInvoiceReportDataSource') DROP PROCEDURE Reporting.usp_GetInvoiceReportDataSource;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetStatementReportDataSource') DROP PROCEDURE Reporting.usp_GetStatementReportDataSource;");
        }

        public override void Down()
        {
            DropForeignKey("Repair.Invoices", "CurrencyId", "Billing.Currencies");
            DropForeignKey("Billing.Payments", "CurrencyId", "Billing.Currencies");
            DropForeignKey("Billing.PricingPlans", "CurrencyId", "Billing.Currencies");
            DropIndex("Repair.Invoices", new[] { "CurrencyId" });
            DropIndex("Billing.Payments", new[] { "CurrencyId" });
            DropIndex("Billing.PricingPlans", new[] { "CurrencyId" });
            DropColumn("Repair.Invoices", "CurrencyId");
            DropColumn("Billing.Payments", "CurrencyId");
            DropColumn("Billing.PricingPlans", "CurrencyId");
            DropTable("Billing.Currencies");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Billing' AND ROUTINE_NAME = 'usp_GetPricingPlans') DROP PROCEDURE Billing.usp_GetPricingPlans;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Billing' AND ROUTINE_NAME = 'usp_SavePricingPlan') DROP PROCEDURE Billing.usp_SavePricingPlan;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Billing' AND ROUTINE_NAME = 'usp_GetOutstandingInvoices') DROP PROCEDURE Billing.usp_GetOutstandingInvoices;");

            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetInvoiceReportDataSource') DROP PROCEDURE Reporting.usp_GetInvoiceReportDataSource;");
            Sql(@"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'Reporting' AND ROUTINE_NAME = 'usp_GetStatementReportDataSource') DROP PROCEDURE Reporting.usp_GetStatementReportDataSource;");
        }
    }
}
