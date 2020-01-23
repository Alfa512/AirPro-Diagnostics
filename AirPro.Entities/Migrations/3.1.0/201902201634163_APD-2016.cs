namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APD2016 : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('Support.RequestLogs') AND name='IX_SupportRequestLogs_Route')
                    CREATE INDEX IX_SupportRequestLogs_Route ON Support.RequestLogs (RouteArea, RouteController, RouteAction, RouteId) INCLUDE (UserGuid, ActionStartTime);");

            Sql(@"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('Notification.Logs') AND name='IX_NotificationLogs_Created')
                    CREATE INDEX IX_NotificationLogs_Created ON Notification.Logs (CreatedDt) INCLUDE (Destination);");

            Sql(@"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('Scan.ReportsArchive') AND name='IX_ScanReportsArchive_ReportId')
                    CREATE INDEX [IX_ScanReportsArchive_ReportId] ON [Scan].[ReportsArchive] ([ReportId]);");

            Sql(@"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('Scan.ReportTroubleCodeRecommendations') AND name='IX_ScanReportTroubleCodeRecommendations_TroubleCodeRecommendationId_ReportOrderTroubleCodeId')
                    CREATE INDEX [IX_ScanReportTroubleCodeRecommendations_TroubleCodeRecommendationId_ReportOrderTroubleCodeId] ON [Scan].[ReportTroubleCodeRecommendations] ([TroubleCodeRecommendationId]) INCLUDE ([ReportOrderTroubleCodeId]);");

            Sql(@"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('Repair.Invoices') AND name='IX_RepairInvoices_InvoiceInd_CurrencyId_InvoicedDt')
                    CREATE INDEX [IX_RepairInvoices_InvoiceInd_CurrencyId_InvoicedDt] ON [Repair].[Invoices] ([InvoicedInd]) INCLUDE ([CurrencyId], [InvoicedDt]);");

            Sql(@"DROP INDEX IF EXISTS [IX_InvoiceID] ON [Repair].[Invoices];");

            Sql(@"DROP INDEX IF EXISTS [IX_UserGuid] ON [Technician].[Profiles];");
        }
        
        public override void Down()
        {
            Sql(@"DROP INDEX IF EXISTS IX_SupportRequestLogs_Route ON Support.RequestLogs;");

            Sql(@"DROP INDEX IF EXISTS IX_NotificationLogs_Created ON Notification.Logs;");

            Sql(@"DROP INDEX IF EXISTS [IX_ScanReportsArchive_ReportId] ON [Scan].[ReportsArchive];");

            Sql(@"DROP INDEX IF EXISTS [IX_ScanReportTroubleCodeRecommendations_TroubleCodeRecommendationId_ReportOrderTroubleCodeId] ON [Scan].[ReportTroubleCodeRecommendations];");

            Sql(@"DROP INDEX IF EXISTS [IX_RepairInvoices_InvoiceInd_CurrencyId_InvoicedDt] ON [Repair].[Invoices];");
        }
    }
}
