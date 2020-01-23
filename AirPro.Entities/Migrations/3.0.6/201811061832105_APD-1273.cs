namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;

    public partial class APD1273 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Billing.Payments", "PaymentDt", c => c.DateTime(storeType: "date"));
            Sql("UPDATE Billing.Payments SET PaymentDt = CreatedDt WHERE PaymentDt IS NULL");

            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetStatementReportDataSource");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Reporting", "usp_GetStatementReportDataSource");

            DropColumn("Billing.Payments", "PaymentDt");
        }
    }
}
