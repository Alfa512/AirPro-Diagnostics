using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AirPro.Logging;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using Dapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using UniMatrix.Common.Extensions;

namespace AirPro.Function.SecureShare
{
    public static class RequestProcessor
    {
        private static readonly string APIKey;
        private static readonly string APIToken;

        private static readonly string ConnectionString;

        static RequestProcessor()
        {
#if DEBUG
            ConnectionString = Environment.GetEnvironmentVariable("DefaultConnection");
#else
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
#endif

            APIKey = Environment.GetEnvironmentVariable("CCCAPIKey");
            APIToken = Environment.GetEnvironmentVariable("CCCAPIToken");

            Logger.Initialize(ConnectionString);
        }

        [FunctionName("estimate")]
        public static async Task<HttpResponseMessage> RunEstimate([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Estimate Received.");

            try
            {
                // Load Parameters.
                var appId = req.GetQueryNameValuePairs()
                    .FirstOrDefault(q => String.Compare(q.Key, "appId", StringComparison.OrdinalIgnoreCase) == 0)
                    .Value;
                var trigger = req.GetQueryNameValuePairs()
                    .FirstOrDefault(q => String.Compare(q.Key, "trigger", StringComparison.OrdinalIgnoreCase) == 0)
                    .Value;

                // Load Headers.
                if (!req.Headers.TryGetValues("X-SecureShare-Signature", out var headerValues))
                {
                    log.Error("Signature Header Missing.");
                    return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Signature Header Missing.");
                }

                // Load Request Body.
                var requestBody = await req.Content.ReadAsStringAsync();
                var requestUri = req.RequestUri.ToString();
                var secretKeyBytes = new UTF8Encoding().GetBytes(APIKey);

                // Verify Signature.
                using (var hmac = new HMACSHA1(secretKeyBytes))
                {
                    var encoder = new UTF8Encoding();
                    var signatureBytes = encoder.GetBytes(requestUri + requestBody.SafeLeft(200));
                    var signatureHash = hmac.ComputeHash(signatureBytes);
                    var computedSignature = Convert.ToBase64String(signatureHash);
                    var signatureHeaderValue = headerValues.First();
                    if (!string.Equals(computedSignature, signatureHeaderValue))
                    {
                        //return BadRequest(computedSignature);
                        log.Error("Invalid Signature.");
                        return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Signature.");
                    }
                }

                try
                {
                    // Load VIN.
                    var bmsDocument = new XmlDocument();
                    bmsDocument.Load(new XmlTextReader(new StringReader(requestBody)) {Namespaces = false});
                    var vin = bmsDocument.SelectSingleNode("/VehicleDamageEstimateAddRq/VehicleInfo/VINInfo/VIN/VINNum")
                        ?.InnerText;

                    // Lookup Vehicle.
                    if (!string.IsNullOrEmpty(vin))
                    {
                        var veh = await new ServiceFactory(ConnectionString, new GenericIdentity("system@airprodiag.com"))
                            .GetByIdAsync<IVehicleDto>(vin);

                        if (!veh.ManualEntryInd)
                            log.Info($"Vehicle Found: {veh.VehicleVIN}");
                    }
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    Logger.LogException(e);
                }

                // Open Connection.
                using (var conn = new SqlConnection(ConnectionString))
                {
                    // Save Estimate.
                    await conn.ExecuteAsync("Service.usp_SaveCCCEstimate",
                        new
                        {
                            AppId = appId,
                            Trigger = trigger,
                            EstimateXml = requestBody
                        }, commandType: CommandType.StoredProcedure);
                    log.Info("Estimate Saved.");

                    try
                    {
                        // Create Repairs.
                        await conn.ExecuteAsync("Repair.usp_CreateFromCCCEstimates",
                            commandType: CommandType.StoredProcedure);
                        log.Info("Repair Creation Run.");
                    }
                    catch (Exception e)
                    {
                        log.Error(e.Message);
                        Logger.LogException(e);
                    }
                }
            }
            catch (Exception e)
            {
                // Record Error.
                log.Error(e.Message);
                Logger.LogException(e);

                // Return.
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Processing Error.");
            }

            // Return.
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [FunctionName("ping")]
        public static async Task<HttpResponseMessage> RunPing([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            var dateString = DateTime.UtcNow.ToString("o");
            log.Info($"Ping: {dateString}");
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(dateString, System.Text.Encoding.UTF8, "text/plain")
            };
        }
    }
}
