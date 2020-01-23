using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Logging;
using AirPro.Notifications.WebJob.Models;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace AirPro.Notifications.WebJob
{
    public class Functions
    {
        public static async Task ProcessQueueMessage([QueueTrigger("%NotificationQueue%")] string message, TextWriter log)
        {
            try
            {
                // Check Item.
                if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message));

                // Log Item.
                log.WriteLine($"Received Message: {message}");

                // Create Object.
                var model = JsonConvert.DeserializeObject<NotificationQueueMessage>(message);

                // Process Notification.
                using (var conn = new SqlConnection(Program.ConnectionString))
                {
                    var notify = new Notifications(conn, Program.Settings, Program.OverrideEmail);
                    switch (model.TemplateName)
                    {
                        case NotificationTemplate.BillingEmail:
                            await notify.SendBillingNotification(model.Identifier).ConfigureAwait(false);
                            break;
                        case NotificationTemplate.ScanRequestEmail:
                            await notify.SendScanRequestNotification(model.Identifier).ConfigureAwait(false);
                            break;
                        case NotificationTemplate.ShopInvoiceEmail:
                            await notify.SendShopInvoiceNotification(model.Identifier).ConfigureAwait(false);
                            break;
                        case NotificationTemplate.ShopReportEmail:
                            await notify.SendShopReportNotification(model.Identifier).ConfigureAwait(false);
                            break;
                        case NotificationTemplate.ShopStatementEmail:
                            await notify.SendShopStatementNotification(model.Identifier).ConfigureAwait(false);
                            break;
                        case NotificationTemplate.RepairFeedbackEmail:
                            await notify.SendRepairFeedbackNotification(model.Identifier).ConfigureAwait(false);
                            break;
                        case NotificationTemplate.ScanToolIssuesEmail:
                            await notify.SendScanToolIssuesNotification(model.Identifier).ConfigureAwait(false);
                            break;
                        case NotificationTemplate.RegistrationEmail:
                            await notify.SendRegistrationLinkNotification(model.Identifier).ConfigureAwait(false);
                            break;
                        case NotificationTemplate.RegistrationWelcomeEmail:
                            await notify.SendRegistrationWelcomeNotification(model.Identifier).ConfigureAwait(false);
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);

                throw new Exception(@"Unable to process message, see exception queue for details.");
            }
        }
    }
}
