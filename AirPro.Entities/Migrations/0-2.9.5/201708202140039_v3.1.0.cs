namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v310 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Billing.Payments", "DiscountPercentage", c => c.Int(nullable: false));
            AddColumn("Billing.PaymentTransactions", "DiscountAmountApplied", c => c.Decimal(nullable: false, precision: 18, scale: 2));

            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vwInvoiceBalances') DROP VIEW [Billing].[vwInvoiceBalances];");
            Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'Billing' AND ROUTINE_NAME = 'usp_GetOutstandingInvoices') DROP PROCEDURE [Billing].[usp_GetOutstandingInvoices];");
        }

        public override void Down()
        {
            DropColumn("Billing.PaymentTransactions", "DiscountAmountApplied");
            DropColumn("Billing.Payments", "DiscountPercentage");
        }
    }
}
