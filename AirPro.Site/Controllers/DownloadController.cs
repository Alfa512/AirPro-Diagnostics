using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AirPro.Logging;
using AirPro.Reports;
using AirPro.Service.DTOs.Interface;
using Dapper;
using Microsoft.WindowsAzure.Storage;

namespace AirPro.Site.Controllers
{
    [Authorize]
    public class DownloadController : BaseController
    {
        private string StorageAccount { get; } = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;

        private async Task<bool> ValidateReportAccess(SqlConnection conn, int? repairId = null, int? requestId = null, int? paymentId = null) =>
            await conn.QueryFirstOrDefaultAsync<int>(sql: "Reporting.usp_GetReportAccess", param: new
            {
                Factory.User.UserGuid,
                RepairId = repairId,
                RequestId = requestId,
                PaymentId = paymentId
            }, commandType: CommandType.StoredProcedure) == 1;

        public async Task<ActionResult> ScanReport(int id)
        {
            try
            {
                // Check Id.
                if (id == 0) goto NotFound;

                using (var conn = new SqlConnection(MvcApplication.ConnectionString))
                {
                    // Verify Access.
                    if (!await ValidateReportAccess(conn, requestId: id)) goto NotFound;

                    // Generate Report.
                    var reportStream = await new ReportGenerator(conn).GetScanReportPdfStreamAsync(id, Factory.User.UserUtcOffset);

                    // Send Report.
                    return File(reportStream, "application/pdf", $"AirProScan-{id}.pdf");
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            NotFound:
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        public async Task<ActionResult> Invoice(int id)
        {
            try
            {
                // Check Id.
                if (id == 0) goto NotFound;

                using (var conn = new SqlConnection(MvcApplication.ConnectionString))
                {
                    // Verify Access.
                    if (!await ValidateReportAccess(conn, repairId: id)) goto NotFound;

                    // Generate Report.
                    var reportStream = await new ReportGenerator(conn).GetInvoiceReportPdfStreamAsync(id, Factory.User.UserUtcOffset);

                    // Send Report.
                    return File(reportStream, "application/pdf", $"AirProInvoice-{id}.pdf");
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            NotFound:
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        public async Task<ActionResult> Statement(int id)
        {
            try
            {
                // Check Id.
                if (id == 0) goto NotFound;

                using (var conn = new SqlConnection(MvcApplication.ConnectionString))
                {
                    // Verify Access.
                    if (!await ValidateReportAccess(conn, paymentId: id)) goto NotFound;

                    // Generate Report.
                    var reportStream = await new ReportGenerator(conn).GetStatementReportPdfStreamAsync(id, Factory.User.UserUtcOffset);

                    // Send Report.
                    return File(reportStream, "application/pdf", $"AirProStatement-{id}.pdf");
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            NotFound:
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        public async Task<ActionResult> Estimate(int id)
        {
            try
            {
                // Check Id.
                if (id == 0) goto NotFound;

                using (var conn = new SqlConnection(MvcApplication.ConnectionString))
                {
                    // Verify Access.
                    if (!await ValidateReportAccess(conn, repairId: id)) goto NotFound;

                    // Generate Report.
                    var reportStream = await new ReportGenerator(conn).GetEstimateReportPdfStreamAsync(id, Factory.User.UserUtcOffset);

                    // Send Report.
                    return File(reportStream, "application/pdf", $"AirProAssessment-{id}.pdf");
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            NotFound:
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        public async Task<ActionResult> File(string id)
        {
            try
            {
                // Check Id.
                if (id == null) goto NotFound;

                // Lookup Id.
                var upload = Factory.GetById<IUploadDto>(id);
                if (upload == null) goto NotFound;

                // Check Connection String.
                if (CloudStorageAccount.TryParse(StorageAccount, out var storageAccount))
                {
                    // Create Client.
                    var cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    // Create Upload Container.
                    var cloudBlobContainer = cloudBlobClient.GetContainerReference("uploads");
                    await cloudBlobContainer.CreateIfNotExistsAsync();

                    // Download File Stream.
                    Stream file = new MemoryStream();
                    var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(upload.UploadStorageName);
                    await cloudBlockBlob.DownloadToStreamAsync(file);

                    // Check Stream.
                    if (file.Length > 0)
                    {
                        file.Seek(0, SeekOrigin.Begin);
                        return File(file, upload.UploadMimeType,
                            $"{upload.UploadFileName}.{upload.UploadFileExtension}");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            NotFound:
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }
    }
}