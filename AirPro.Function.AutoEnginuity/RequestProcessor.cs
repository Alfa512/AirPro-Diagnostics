using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Logging;
using AirPro.Parser;
using AirPro.Service;
using Dapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AirPro.Function.AutoEnginuity
{
    public static class RequestProcessor
    {
        private static readonly string ConnectionString;

        static RequestProcessor()
        {
#if DEBUG
            ConnectionString = Environment.GetEnvironmentVariable("DefaultConnection");
#else
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
#endif

            Logger.Initialize(ConnectionString);
        }

        [FunctionName("ScanUpload")]
        public static async Task<HttpResponseMessage> RunProcessor([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            // Get Request Body.
            var body = await req.Content.ReadAsStringAsync();

            try
            {
                // Parse Query Parameters.
                var paramValues = string.Join("&", req.GetQueryNameValuePairs()?.Select(q => $"{q.Key}={q.Value}") ?? new string[] { });

                // Save Request.
                using (var conn = new SqlConnection(ConnectionString))
                {
                    await conn.ExecuteAsync("Service.usp_SaveAutoEnginuityUpload", new { RequestQuery = paramValues, RequestBody = body }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                // Log Error.
                Logger.LogException(e);
            }

            // Check Body.
            if (string.IsNullOrWhiteSpace(body)) return req.CreateResponse(HttpStatusCode.OK, "Request Processed.");

            try
            {
                // Parse File.
                var file = DiagnosticFileParser.ParseFile(body, DiagnosticTool.AutoEnginuity, DiagnosticFileType.JSON);

                // Save File.
                var factory = new ServiceFactory(ConnectionString, new GenericIdentity("system@airprodiag.com"));
                factory.Save(file);

                // Return OK.
                return req.CreateResponse(HttpStatusCode.OK, "Request Processed.");
            }
            catch (Exception e)
            {
                // Log Error.
                Logger.LogException(e);
            }

            // Return Error.
            return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to Process Request.");
        }
    }
}
