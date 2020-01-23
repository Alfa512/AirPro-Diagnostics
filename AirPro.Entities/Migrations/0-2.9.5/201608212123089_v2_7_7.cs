using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_7_7 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Scan.Reports", name: "InvoicedBy_Id", newName: "CompletedByID");
            RenameIndex(table: "Scan.Reports", name: "IX_InvoicedBy_Id", newName: "IX_CompletedByID");

            RenameColumn(table: "Repair.Invoices", name: "CustomerVisible", newName: "InvoicedInd");
            //AddColumn("Repair.Invoices", "InvoicedInd", c => c.Boolean(nullable: false));
            //DropColumn("Repair.Invoices", "CustomerVisible");

            RenameColumn(table: "Scan.Reports", name: "ReportCompleted", newName: "CompletedInd");
            //AddColumn("Scan.Reports", "CompletedInd", c => c.Boolean(nullable: false));
            //DropColumn("Scan.Reports", "ReportCompleted");


            RenameColumn(table: "Scan.Reports", name: "Invoiced", newName: "InvoicedInd");
            //AddColumn("Scan.Reports", "InvoicedInd", c => c.Boolean(nullable: false));
            //DropColumn("Scan.Reports", "Invoiced");

            AddColumn("Repair.Invoices", "InvoicedDt", c => c.DateTimeOffset(precision: 7));
            AddColumn("Repair.Invoices", "InvoicedByID", c => c.String(maxLength: 128));
            CreateIndex("Repair.Invoices", "InvoicedByID");
            AddForeignKey("Repair.Invoices", "InvoicedByID", "Access.Users", "UserId");

            AddColumn("Scan.Reports", "CompletedDt", c => c.DateTimeOffset(precision: 7));
            AddColumn("Scan.Reports", "InvoicedByID", c => c.String(maxLength: 128));
            CreateIndex("Scan.Reports", "InvoicedByID");
            AddForeignKey("Scan.Reports", "InvoicedByID", "Access.Users", "UserId");

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201608212123089_v2_7_7.sql");
        }

        public override void Down()
        {
            DropForeignKey("Scan.Reports", "InvoicedByID", "Access.Users");
            DropIndex("Scan.Reports", new[] { "InvoicedByID" });

            DropForeignKey("Repair.Invoices", "InvoicedByID", "Access.Users");
            DropIndex("Repair.Invoices", new[] { "InvoicedByID" });

            RenameIndex(table: "Scan.Reports", name: "IX_CompletedByID", newName: "IX_InvoicedBy_Id");
            RenameColumn(table: "Scan.Reports", name: "CompletedByID", newName: "InvoicedBy_Id");

            RenameColumn(table: "Scan.Reports", name: "InvoicedInd", newName: "Invoiced");
            //AddColumn("Scan.Reports", "Invoiced", c => c.Boolean(nullable: false));
            //DropColumn("Scan.Reports", "InvoicedInd");

            RenameColumn(table: "Scan.Reports", name: "CompletedInd", newName: "ReportCompleted");
            //AddColumn("Scan.Reports", "ReportCompleted", c => c.Boolean(nullable: false));
            //DropColumn("Scan.Reports", "CompletedInd");

            RenameColumn(table: "Repair.Invoices", name: "InvoicedInd", newName: "CustomerVisible");
            //AddColumn("Repair.Invoices", "CustomerVisible", c => c.Boolean(nullable: false));
            //DropColumn("Repair.Invoices", "InvoicedInd");

            DropColumn("Scan.Reports", "InvoicedByID");
            DropColumn("Scan.Reports", "CompletedDt");
            DropColumn("Repair.Invoices", "InvoicedByID");
            DropColumn("Repair.Invoices", "InvoicedDt");
        }
    }
}
