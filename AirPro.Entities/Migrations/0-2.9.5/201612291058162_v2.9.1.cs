namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v291 : DbMigration
    {
        public override void Up()
        {
            DropIndex("Billing.Payments", new[] { "PaymentReceivedFromShopID" });
            DropIndex("Billing.Payments", new[] { "PaymentTypeID" });

            DropIndex("Billing.PaymentTransactions", new[] { "PaymentID" });
            DropIndex("Billing.PaymentTransactions", new[] { "InvoiceID" });

            RenameColumn(table: "Billing.Payments", name: "PaymentTypeID", newName: "PaymentTypeId");
            RenameColumn(table: "Billing.Payments", name: "PaymentReceivedFromShopID", newName: "PaymentReceivedFromShopId");

            RenameColumn(table: "Billing.PaymentTypes", name: "PaymentTypeID", newName: "PaymentTypeId");

            RenameColumn(table: "Billing.PaymentTransactions", name: "PaymentTransactionID", newName: "PaymentTransactionId");
            RenameColumn(table: "Billing.PaymentTransactions", name: "PaymentID", newName: "PaymentId");
            RenameColumn(table: "Billing.PaymentTransactions", name: "InvoiceID", newName: "InvoiceId");

            RenameColumn(table: "Scan.Reports", name: "ReportID", newName: "ReportId");
            RenameColumn(table: "Scan.ReportsArchive", name: "ReportArchiveID", newName: "ReportArchiveId");
            RenameColumn(table: "Scan.ReportsArchive", name: "ReportID", newName: "ReportId");

            RenameColumn(table: "Support.ApplicationExceptions", name: "ExceptionID", newName: "ExceptionId");
            RenameColumn(table: "Support.NotificationLogs", name: "NotificationLogID", newName: "NotificationLogId");
            RenameColumn(table: "Support.NotificationTemplates", name: "NotificationTemplateID", newName: "NotificationTemplateId");

            RenameColumn(table: "Billing.Payments", name: "PaymentVoidByID", newName: "PaymentVoidByUserId");
            RenameIndex(table: "Billing.Payments", name: "IX_PaymentVoidByID", newName: "IX_PaymentVoidByUserId");

            RenameColumn(table: "Billing.PaymentTransactions", name: "PaymentTransactionVoidByID", newName: "PaymentTransactionVoidByUserId");
            RenameIndex(table: "Billing.PaymentTransactions", name: "IX_PaymentTransactionVoidByID", newName: "IX_PaymentTransactionVoidByUserId");

            CreateIndex("Billing.Payments", "PaymentReceivedFromShopId");
            CreateIndex("Billing.Payments", "PaymentTypeId");

            CreateIndex("Billing.PaymentTransactions", "PaymentId");
            CreateIndex("Billing.PaymentTransactions", "InvoiceId");
        }
        
        public override void Down()
        {
            DropIndex("Billing.Payments", new[] { "PaymentReceivedFromShopId" });
            DropIndex("Billing.Payments", new[] { "PaymentTypeId" });

            DropIndex("Billing.PaymentTransactions", new[] { "PaymentId" });
            DropIndex("Billing.PaymentTransactions", new[] { "InvoiceId" });

            RenameColumn(table: "Billing.Payments", name: "PaymentTypeId", newName: "PaymentTypeID");
            RenameColumn(table: "Billing.Payments", name: "PaymentReceivedFromShopId", newName: "PaymentReceivedFromShopID");

            RenameColumn(table: "Billing.PaymentTypes", name: "PaymentTypeId", newName: "PaymentTypeID");

            RenameColumn(table: "Billing.PaymentTransactions", name: "PaymentTransactionId", newName: "PaymentTransactionID");
            RenameColumn(table: "Billing.PaymentTransactions", name: "PaymentId", newName: "PaymentID");
            RenameColumn(table: "Billing.PaymentTransactions", name: "InvoiceId", newName: "InvoiceID");

            RenameColumn(table: "Scan.Reports", name: "ReportId", newName: "ReportID");
            RenameColumn(table: "Scan.ReportsArchive", name: "ReportArchiveId", newName: "ReportArchiveID");
            RenameColumn(table: "Scan.ReportsArchive", name: "ReportId", newName: "ReportID");

            RenameColumn(table: "Support.ApplicationExceptions", name: "ExceptionId", newName: "ExceptionID");
            RenameColumn(table: "Support.NotificationLogs", name: "NotificationLogId", newName: "NotificationLogID");
            RenameColumn(table: "Support.NotificationTemplates", name: "NotificationTemplateId", newName: "NotificationTemplateID");

            RenameColumn(table: "Billing.Payments", name: "PaymentVoidByUserId", newName: "PaymentVoidByID");
            RenameIndex(table: "Billing.Payments", name: "IX_PaymentVoidByUserId", newName: "IX_PaymentVoidByID");

            RenameColumn(table: "Billing.PaymentTransactions", name: "PaymentTransactionVoidByUserId", newName: "PaymentTransactionVoidByID");
            RenameIndex(table: "Billing.PaymentTransactions", name: "IX_PaymentTransactionVoidByUserId", newName: "IX_PaymentTransactionVoidByID");

            CreateIndex("Billing.Payments", "PaymentReceivedFromShopID");
            CreateIndex("Billing.Payments", "PaymentTypeID");

            CreateIndex("Billing.PaymentTransactions", "PaymentID");
            CreateIndex("Billing.PaymentTransactions", "InvoiceID");
        }
    }
}
