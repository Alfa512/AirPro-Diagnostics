using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v285 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Billing.Payments",
                c => new
                    {
                        PaymentID = c.Int(nullable: false, identity: true),
                        PaymentReceivedFromShopID = c.Int(nullable: false),
                        PaymentTypeID = c.Int(nullable: false),
                        PaymentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentReferenceNumber = c.String(),
                        PaymentMemo = c.String(),
                        PaymentVoidInd = c.Boolean(nullable: false),
                        PaymentVoidByID = c.String(maxLength: 128),
                        PaymentVoidDt = c.DateTimeOffset(precision: 7),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                        UpdatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PaymentID)
                .ForeignKey("Access.Users", t => t.CreatedBy_Id)
                .ForeignKey("Access.Shops", t => t.PaymentReceivedFromShopID)
                .ForeignKey("Billing.PaymentTypes", t => t.PaymentTypeID)
                .ForeignKey("Access.Users", t => t.PaymentVoidByID)
                .ForeignKey("Access.Users", t => t.UpdatedBy_Id)
                .Index(t => t.PaymentReceivedFromShopID)
                .Index(t => t.PaymentTypeID)
                .Index(t => t.PaymentVoidByID)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);
            
            CreateTable(
                "Billing.PaymentTypes",
                c => new
                    {
                        PaymentTypeID = c.Int(nullable: false, identity: true),
                        PaymentTypeName = c.String(),
                        PaymentTypeSortOrder = c.Int(nullable: false),
                        PaymentTypeActiveInd = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentTypeID);
            
            CreateTable(
                "Billing.PaymentTransactions",
                c => new
                    {
                        PaymentTransactionID = c.Int(nullable: false, identity: true),
                        PaymentID = c.Int(nullable: false),
                        InvoiceID = c.Int(nullable: false),
                        InvoiceAmountApplied = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentTransactionVoidInd = c.Boolean(nullable: false),
                        PaymentTransactionVoidByID = c.String(maxLength: 128),
                        PaymentTransactionVoidDt = c.DateTimeOffset(precision: 7),
                        CreatedDt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedDt = c.DateTimeOffset(precision: 7),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                        UpdatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PaymentTransactionID)
                .ForeignKey("Access.Users", t => t.CreatedBy_Id)
                .ForeignKey("Repair.Invoices", t => t.InvoiceID)
                .ForeignKey("Billing.Payments", t => t.PaymentID)
                .ForeignKey("Access.Users", t => t.PaymentTransactionVoidByID)
                .ForeignKey("Access.Users", t => t.UpdatedBy_Id)
                .Index(t => t.PaymentID)
                .Index(t => t.InvoiceID)
                .Index(t => t.PaymentTransactionVoidByID)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201612222349234_v2.8.5_Deploy.sql");
        }

        public override void Down()
        {
            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201612222349234_v2.8.5_Rollback.sql");

            DropForeignKey("Billing.PaymentTransactions", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Billing.PaymentTransactions", "PaymentTransactionVoidByID", "Access.Users");
            DropForeignKey("Billing.PaymentTransactions", "PaymentID", "Billing.Payments");
            DropForeignKey("Billing.PaymentTransactions", "InvoiceID", "Repair.Invoices");
            DropForeignKey("Billing.PaymentTransactions", "CreatedBy_Id", "Access.Users");
            DropForeignKey("Billing.Payments", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Billing.Payments", "PaymentVoidByID", "Access.Users");
            DropForeignKey("Billing.Payments", "PaymentTypeID", "Billing.PaymentTypes");
            DropForeignKey("Billing.Payments", "PaymentReceivedFromShopID", "Access.Shops");
            DropForeignKey("Billing.Payments", "CreatedBy_Id", "Access.Users");
            DropIndex("Billing.PaymentTransactions", new[] { "UpdatedBy_Id" });
            DropIndex("Billing.PaymentTransactions", new[] { "CreatedBy_Id" });
            DropIndex("Billing.PaymentTransactions", new[] { "PaymentTransactionVoidByID" });
            DropIndex("Billing.PaymentTransactions", new[] { "InvoiceID" });
            DropIndex("Billing.PaymentTransactions", new[] { "PaymentID" });
            DropIndex("Billing.Payments", new[] { "UpdatedBy_Id" });
            DropIndex("Billing.Payments", new[] { "CreatedBy_Id" });
            DropIndex("Billing.Payments", new[] { "PaymentVoidByID" });
            DropIndex("Billing.Payments", new[] { "PaymentTypeID" });
            DropIndex("Billing.Payments", new[] { "PaymentReceivedFromShopID" });
            DropTable("Billing.PaymentTransactions");
            DropTable("Billing.PaymentTypes");
            DropTable("Billing.Payments");
        }
    }
}
