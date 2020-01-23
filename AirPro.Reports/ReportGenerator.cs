using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using AirPro.Reports.DataSources.Concrete;
using AirPro.Reports.DataSources.Interface;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using Dapper;
using Microsoft.Reporting.WebForms;

namespace AirPro.Reports
{
    public class ReportGenerator
    {
        private readonly IDbConnection _connection;

        private const string ScanReport = @"ScanReport.rdlc";
        private const string InvoiceReport = @"RepairInvoice.rdlc";
        private const string StatementReport = @"Statement.rdlc";
        private const string EstimateReport = @"RepairEstimate.rdlc";

        private const string ScanReportSql = @"EXEC Reporting.usp_GetScanReportDataSource @RequestId, @Offset";
        private const string InvoiceReportSql = @"EXEC Reporting.usp_GetInvoiceReportDataSource @InvoiceId, @Offset";
        private const string StatementReportSql = @"EXEC Reporting.usp_GetStatementReportDataSource @PaymentId, @Offset";
        private const string EstimateReportSql = @"EXEC Reporting.usp_GetEstimateReportDataSource @RepairId, @Offset";

        public ReportGenerator(IDbConnection connection)
        {
            // Store Connection.
            _connection = connection;

            // Open Connection.
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        public async Task<Stream> GetScanReportPdfStreamAsync(int requestId, string offset = "-00:00")
        {
            var report = await _connection.QuerySingleOrDefaultAsync<ScanReportDataSource>(ScanReportSql, new {RequestId = requestId, Offset = offset});

            if (report == null)
                throw new NullReferenceException("Scan Report NOT Found.");

            // Load Report Body.
            var factory = new ServiceFactory((SqlConnection) _connection, new GenericIdentity("system@airprodiag.com"));
            report.ReportTechnicianNotes = (await factory.GetByIdAsync<IReportDto>(requestId.ToString()))?.GetReportDtoHtml();

            return await GetScanReportPdfStreamAsync(report);
        }

        private async Task<Stream> GetScanReportPdfStreamAsync(IScanReportDataSource dataSource)
        {
            var rds = new ReportDataSource
            {
                Name = "ScanReportDataSource",
                Value = new List<IScanReportDataSource>() { dataSource }
            };

            return await GenerateStream.ReportToPdfAsync(ScanReport, new[] { rds });
        }

        public async Task<Stream> GetInvoiceReportPdfStreamAsync(int invoiceId, string offset = "-00:00")
        {
            using (var multi = await _connection.QueryMultipleAsync(InvoiceReportSql, new { InvoiceId = invoiceId, Offset = offset }))
            {
                var invoice = multi.Read<RepairInvoiceDataSource>().SingleOrDefault();

                if (invoice == null)
                    throw new NullReferenceException("Invoice was NOT Found.");

                invoice.CustomerMemo = invoice.CustomerMemo;

                var invoiceLineItems = multi.Read<RepairInvoiceLineItemsDataSource>().ToList();
                var invoiceWorkTypes = multi.Read<RepairInvoiceLineItemsDataSource>().ToList();

                foreach (var workTypeGroup in invoiceWorkTypes.GroupBy(x => x.ReportId))
                {
                    var invoiceLineItem = invoiceLineItems.FirstOrDefault(x => x.ReportId == workTypeGroup.Key);
                    var index = invoiceLineItems.IndexOf(invoiceLineItem);

                    foreach (var item in workTypeGroup)
                    {
                        item.ReportId = null;
                    }
                    invoiceLineItems.Insert(index + 1, new RepairInvoiceLineItemsDataSource()
                    {
                        SubItems = workTypeGroup.ToList()
                    });
                    if (invoiceLineItem != null)
                        invoiceLineItem.HasSubItems = true;
                }

                var last = invoiceLineItems.LastOrDefault();
                if (last != null)
                {
                    last.LastItem = true;
                }

                return await GetInvoiceReportPdfStreamAsync(invoice, invoiceLineItems);
            }
        }

        private async Task<Stream> GetInvoiceReportPdfStreamAsync(IRepairInvoiceDataSource invoice, IEnumerable<IRepairInvoiceLineItemsDataSource> invoiceLineItems)
        {
            var rds = new ReportDataSource
            {
                Name = "RepairInvoice",
                Value = new List<IRepairInvoiceDataSource> { invoice }
            };

            if (!invoiceLineItems.Any())
            {
                invoiceLineItems = new List<IRepairInvoiceLineItemsDataSource>()
                {
                    new RepairInvoiceLineItemsDataSource()
                    {
                        WorkPerfomed = "<No Invoiced Scans>",
                        InvoiceAmount = 0
                    }
                };
            }

            var li = new ReportDataSource
            {
                Name = "LineItems",
                Value = invoiceLineItems
            };

            return await GenerateStream.ReportToPdfAsync(InvoiceReport, new[] { rds, li });
        }

        public async Task<Stream> GetStatementReportPdfStreamAsync(int paymentId, string offset = "-00:00")
        {
            using (var multi = await _connection.QueryMultipleAsync(StatementReportSql, new { PaymentId = paymentId, Offset = offset }))
            {
                var statement = multi.Read<StatementDataSource>().SingleOrDefault();

                if (statement == null)
                    throw new NullReferenceException("Payment was NOT Found.");

                var statementLineItems = multi.Read<StatementLineItemsDataSource>().ToList();

                if (!statementLineItems.Any())
                    throw new NullReferenceException("Payment is Missing Transactions.");

                return await GetStatementReportPfdStreamAsync(statement, statementLineItems);
            }
        }

        private async Task<Stream> GetStatementReportPfdStreamAsync(IStatementDataSource statement,
            IEnumerable<IStatementLineItemsDataSource> statementLineItems)
        {
            var rds = new ReportDataSource
            {
                Name = "Statement",
                Value = new List<IStatementDataSource>() { statement }
            };

            var li = new ReportDataSource
            {
                Name = "InvoiceLineItems",
                Value = statementLineItems
            };

            return await GenerateStream.ReportToPdfAsync(StatementReport, new[] { rds, li });
        }

        public async Task<Stream> GetEstimateReportPdfStreamAsync(int repairId, string offset = "-00:00")
        {
            using (var multi = await _connection.QueryMultipleAsync(EstimateReportSql, new { RepairId = repairId, Offset = offset }))
            {
                var estimate = multi.Read<RepairEstimateDataSource>().SingleOrDefault();

                if (estimate == null)
                    throw new NullReferenceException("Estimate was NOT Found.");

                var estimateLineItems = multi.Read<RepairEstimateLineItemsDataSource>().ToList();

                return await GetEstimateReportPdfStreamAsync(estimate, estimateLineItems);
            }
        }

        private async Task<Stream> GetEstimateReportPdfStreamAsync(IRepairEstimateDataSource estimate, IEnumerable<IRepairEstimateLineItemsDataSource> estimateLineItems)
        {
            var rds = new ReportDataSource
            {
                Name = "RepairEstimate",
                Value = new List<IRepairEstimateDataSource> { estimate }
            };

            if (!estimateLineItems.Any())
            {
                estimateLineItems = new List<IRepairEstimateLineItemsDataSource>()
                {
                    new RepairEstimateLineItemsDataSource()
                    {
                        TypeOfScan = "<No Estimated Scans>",
                        EstimateAmount = 0
                    }
                };
            }

            var li = new ReportDataSource
            {
                Name = "EstimateLineItems",
                Value = estimateLineItems
            };

            return await GenerateStream.ReportToPdfAsync(EstimateReport, new[] { rds, li });
        }
    }
}