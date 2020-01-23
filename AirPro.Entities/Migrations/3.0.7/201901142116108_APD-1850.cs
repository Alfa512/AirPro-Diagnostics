namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;

    public partial class APD1850 : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE INDEX IX_PaymentTransactions_PaymentId_InvoiceId ON Billing.PaymentTransactions (PaymentId) INCLUDE (InvoiceId);");

            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetPaymentsGrid");
        }
        
        public override void Down()
        {
            Sql(@"DROP INDEX IX_PaymentTransactions_PaymentId_InvoiceId ON Billing.PaymentTransactions;");

            this.DropObjectIfExists(DropObjectType.Procedure, "Billing", "usp_GetPaymentsGrid");
        }
    }
}
