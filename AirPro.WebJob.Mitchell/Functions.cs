using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AirPro.Logging;
using AirPro.Reports;
using AirPro.WebJob.Mitchell.Models.Concrete;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using UniMatrix.Common.Extensions;
using Dapper;

namespace AirPro.WebJob.Mitchell
{
    public class Functions
    {
        public static async Task ProcessQueueMessage([QueueTrigger("%MitchellQueue%")] string message, TextWriter log)
        {
            try
            {
                // Load Queue Item.
                var request = JsonConvert.DeserializeObject<MitchellReportQueueItem>(message);

                // Load Report Data.
                MitchellReportDto report;
                string countryCode = "US";
                using (var conn = new SqlConnection(Program.ConnectionString))
                {
                    // Load Data.
                    using (var multi = await conn.QueryMultipleAsync("Service.usp_GetMitchellReport", request, null, null, CommandType.StoredProcedure))
                    {
                        // Load Report.
                        report = multi.Read<MitchellReportDto, MitchellReportVehicleDto, string, MitchellReportDto>((r, v, c) =>
                        {
                            if (r != null) r.Vehicle = v;
                            countryCode = c;
                            return r;
                        }, splitOn: "Vin,CountryCode", buffered: true)?.FirstOrDefault();
                    }

                    // Check Report.
                    if (report == null)
                    {
                        log.WriteLine("Scan Report Not Found!");
                        return;
                    }

                    // Load PDF Report.
                    report.PDF = new MitchellReportPdfDto
                    {
                        Data =  (await (new ReportGenerator(conn)).GetScanReportPdfStreamAsync(request.RequestId)).ToByteArray()
                    };
                }

                // Create Client - Authentication.
                string bearerToken = null;
                using (var client = new HttpClient())
                {
                    // Load Authentication Values.
                    var keyValues = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("client_id", "DiagnosticScan"),
                        new KeyValuePair<string, string>("client_secret",
                            ConfigurationManager.AppSettings["MitchellClientSecret"]),
                        new KeyValuePair<string, string>("username",
                            ConfigurationManager.AppSettings["MitchellUsername"]),
                        new KeyValuePair<string, string>("password",
                            ConfigurationManager.AppSettings["MitchellPassword"]),
                        new KeyValuePair<string, string>("co_cd",
                            ConfigurationManager.AppSettings["MitchellCompanyCode"])
                    };

                    // Load Request Values.
                    var content = new FormUrlEncodedContent(keyValues);
                    var url = new Uri($"https://{ConfigurationManager.AppSettings["MitchellAuthUrl"]}/OAuth2Server/1.0/OAuth/Token");

                    // Load Response.
                    var response = await client.PostAsync(url, content);

                    // Load Token.
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var auth = JsonConvert.DeserializeObject<dynamic>(result);
                        bearerToken = auth?.access_token;
                    }
                }

                // Verify Bearer Token.
                if (bearerToken != null)
                {
                    // Create Client - Send Report.
                    using (var client = new HttpClient())
                    {
                        // Setup Client.
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                        client.DefaultRequestHeaders.Add("x-api-key", ConfigurationManager.AppSettings["MitchellApiKey"]);

                        // Send Report.
                        var url = new Uri($"https://{ConfigurationManager.AppSettings["MitchellReportUrl"]}/DiagnosticApiv1/VehicleDiagnostic/scans?country={countryCode}");
                        var content = new StringContent(JsonConvert.SerializeObject(report), Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(url, content);

                        // Check Response.
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception($"Error sending Report to Mitchell: {response.ReasonPhrase}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Record Error.
                log.WriteLine(e.Message);
                Logger.LogException(e);

                throw;
            }
        }

    }
}
