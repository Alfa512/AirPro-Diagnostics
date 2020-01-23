using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Function.Mitchell.Models.Concrete;
using AirPro.Library;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using UniMatrix.Common.Extensions;
using AirPro.Logging;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Storage;
using Dapper;
using Newtonsoft.Json;

namespace AirPro.Function.Mitchell
{
    public static class RequestProcessor
    {
        private static readonly string Key;
        private static readonly string ConnectionString;

        static RequestProcessor()
        {
            Key = Environment.GetEnvironmentVariable("MitchellAPIKey");

#if DEBUG
            ConnectionString = Environment.GetEnvironmentVariable("DefaultConnection");
#else
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
#endif

            Logger.Initialize(ConnectionString);
        }

        [FunctionName("CreateRegistration")]
        public static async Task<HttpResponseMessage> CreateRegistration(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Registration Received.");

            try
            {
                // Load Request Values.
                var requestBody = await req.Content.ReadAsStringAsync();
                var requestUri = req.RequestUri.ToString();
                var signature = req.GetMitchellHeaderSignature();

                // Validate Signature.
                if (!ValidateSignature(signature, requestUri, requestBody, log))
                    return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Authorization Failed.");

                // Validate Registration.
                var request = JsonConvert.DeserializeObject<MitchellRegistrationDto>(requestBody);

                // Create Registration.
                int notification;
                MitchellRegistrationStatusDto status;
                using (var conn = new SqlConnection(ConnectionString))
                {
                    using (var q = conn.QueryMultipleAsync("Service.usp_SaveMitchellRegistration", param: request, commandType: CommandType.StoredProcedure))
                    {
                        status = q.Result.Read<MitchellRegistrationStatusDto>().FirstOrDefault();
                        notification = q.Result.Read<int>().FirstOrDefault();
                    }
                    log.Info("Registration Created.");
                }

                // Check Notifications.
                if (notification <= 0) return req.CreateResponse(HttpStatusCode.OK, status);

                // Send Notifications.
                var storage = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                using (var queue = new MessageQueue(storage))
                {
                    await queue.AddNotificationQueueMessageAsync(template: NotificationTemplate.RegistrationEmail, id: notification, userGuid: Guid.Empty);
                }

                // Return Response.
                return req.CreateResponse(HttpStatusCode.OK, status);
            }
            catch (Exception e)
            {
                // Record Error.
                log.Error(e.Message);
                Logger.LogException(e);

                // Return.
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Processing Error.");
            }
        }

        [FunctionName("CreateRepair")]
        public static async Task<HttpResponseMessage> CreateRepair(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Request Received.");

            try
            {
                // Load Request Values.
                var requestBody = await req.Content.ReadAsStringAsync();
                var requestUri = req.RequestUri.ToString();
                var signature = req.GetMitchellHeaderSignature();

                // Validate Signature.
                if (!ValidateSignature(signature, requestUri, requestBody, log))
                    return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Authorization Failed.");

                // Load Request.
                var request = JsonConvert.DeserializeObject<MitchellRepairDto>(requestBody);
                request.RequestBody = requestBody;

                // Lookup Vehicle.
                if (!string.IsNullOrEmpty(request?.VehicleVIN))
                {
                    var veh = await new ServiceFactory(ConnectionString, new GenericIdentity("system@airprodiag.com"))
                        .GetByIdAsync<IVehicleDto>(request.VehicleVIN);

                    if (!veh.ManualEntryInd)
                        log.Info($"Vehicle Found: {veh.VehicleVIN}");
                }

                // Open Connection.
                using (var conn = new SqlConnection(ConnectionString))
                {
                    // Save Request.
                    await conn.ExecuteAsync("Service.usp_SaveMitchellRequest", request,
                        commandType: CommandType.StoredProcedure);
                    log.Info("Request Saved.");
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
            return req.CreateResponse(HttpStatusCode.OK, "Done.");
        }

        private static string GetMitchellHeaderSignature(this HttpRequestMessage req) => req.Headers.TryGetValues("X-Mitchell-Signature", out var headerValues) ? headerValues.First() : string.Empty;

        private static bool ValidateSignature(string signature, string requestUri, string requestBody, TraceWriter log)
        {
            // Check Signature.
            if (string.IsNullOrEmpty(signature))
            {
                log.Error("Signature Header Missing.");
                return false;
            }

            // Verify Signature.
            var secretKeyBytes = new UTF8Encoding().GetBytes(Key);
            using (var hmac = new HMACSHA1(secretKeyBytes))
            {
                var encoder = new UTF8Encoding();
                var signatureBytes = encoder.GetBytes(requestUri + requestBody.SafeLeft(200));
                var signatureHash = hmac.ComputeHash(signatureBytes);
                var computedSignature = Convert.ToBase64String(signatureHash);
                if (string.Equals(computedSignature, signature)) return true;
            }

            log.Error("Invalid Signature.");
            return false;
        }
    }
}