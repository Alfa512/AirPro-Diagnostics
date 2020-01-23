using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v303 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.RequestTypes", "DefaultPrice", c => c.Double(nullable: false));

            //SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Views/Billing.vwInvoiceBalances.sql");
            //SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Views/Billing.vwPaymentTransactions.sql");

            //SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Procedures/Billing.usp_GetOutstandingInvoices.sql");
            //SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Procedures/Billing.usp_GetPaymentTransactions.sql");

        }

        public override void Down()
        {
            DropColumn("Scan.RequestTypes", "DefaultPrice");
        }
    }
}
