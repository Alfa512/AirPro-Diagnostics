using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Messaging;
using AirPro.Messaging.Interface;
using AirPro.Notifications.WebJob.Models;
using AirPro.Reports;
using Dapper;
using Microsoft.WindowsAzure.Storage;
using UniMatrix.Common.Extensions;

namespace AirPro.Notifications.WebJob
{
    internal class Notifications
    {
        private readonly Contacts _contacts;
        private readonly string _overrideEmail;
        private readonly IDbConnection _connection;
        private readonly ReportGenerator _reportGenerator;
        private readonly MessagingSettingsModel _settings;

        private const string TemplateSql = @"SELECT Subject, EmailBody, TextMessage FROM Notification.Templates WHERE Name = @TemplateName;";

        private string StorageAccount { get; } = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;

        internal Notifications(IDbConnection connection, MessagingSettingsModel settings, string overrideEmail)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _connection?.Open();

            _settings = settings;
            _overrideEmail = overrideEmail;
            _contacts = new Contacts(_connection);
            _reportGenerator = new ReportGenerator(_connection);
        }

        // Scan Request Notification Template.
        internal async Task SendScanRequestNotification(int requestId)
        {
            // Load Technician Distribution.
            var newRepairNotification = ConfigurationManager.AppSettings["NewRepairNotifications"];
            if (newRepairNotification == null) throw new ArgumentNullException(nameof(newRepairNotification));

            // Load Scan Request.
            var request = await _connection.QuerySingleAsync<RequestNotificationModel>("EXEC Notification.usp_GetRequestNotification @RequestId", new { RequestId = requestId });
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Load Template.
            var template = await _connection.QuerySingleAsync<NotificationTemplateModel>(TemplateSql, new { TemplateName = "ScanRequestEmail" });

            // Populate Template.
            var message = new NotificationMessageModel
            {
                Destinations = new[] { new NotificationDestinationModel { Email = newRepairNotification } },
                EmailSubject = Templates.ScanRequestEmailTemplate(template.Subject, request),
                EmailBody = Templates.ScanRequestEmailTemplate(template.EmailBody, request),
                TextMessage = Templates.ScanRequestEmailTemplate(template.TextMessage, request)
            };

            // Send Message.

            await SendNotifications(message);
        }

        // Shop Report Notification Template.
        internal async Task SendShopReportNotification(int requestId)
        {
            // Load Repair to Populate Template.
            var request = await _connection.QuerySingleAsync<RequestNotificationModel>("EXEC Notification.usp_GetRequestNotification @RequestId", new { RequestId = requestId });
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Check Complete Report.
            if (request.ReportCompletedInd)
            {
                // Load Template.
                var template = await _connection.QuerySingleAsync<NotificationTemplateModel>(TemplateSql, new { TemplateName = "ShopReportEmail" });

                // Load Reporting Contacts.
                var reportingContacts = (await _contacts.GetReportingContacts(request.ShopGuid)).ToList();

                // Load Distinct Timezones.
                var timezones = reportingContacts.Select(r => r.TimeZoneInfoId).Distinct().ToArray();

                // Load Upload Attachments.
                var uploads = new List<INotificationMessageAttachment>();
                uploads.AddRange(await LoadAttachments(UploadType.ScanRequests, request.RequestId.ToString()));
                uploads.AddRange(await LoadAttachments(UploadType.VehicleMakes, request.VehicleMakeId.ToString()));

                // Process Report per Timezone.
                foreach (string timezone in timezones)
                {
                    // Load Time Zone Offset.
                    var offset = TimeZoneInfo.FindSystemTimeZoneById(timezone).GetUtcOffset(DateTimeOffset.UtcNow).ToString();

                    // Load Report.
                    var report = (await _reportGenerator.GetScanReportPdfStreamAsync(request.RequestId, offset)).ToBase64String();

                    // Create Attachment.
                    var attachments = new List<INotificationMessageAttachment>
                    {
                        new NotificationMessageAttachment
                        {
                            Filename = $"AirProScan-{request.RequestId}.pdf",
                            MimeType = "application/pdf",
                            ContentBase64 = report
                        }
                    };

                    // Check Estimate Plan.
                    if (request.EstimatePlanInd)
                    {
                        var estimateReport = (await _reportGenerator.GetEstimateReportPdfStreamAsync(request.RepairId, offset)).ToBase64String();
                        attachments.Add(new NotificationMessageAttachment
                        {
                            Filename = $"AirProAssessment-{request.RequestId}.pdf",
                            MimeType = "application/pdf",
                            ContentBase64 = estimateReport
                        });
                    }

                    // Add Upload Attachments.
                    attachments.AddRange(uploads);

                    // Populate Template.
                    var message = new NotificationMessageModel
                    {
                        Destinations = reportingContacts.Where(r => r.TimeZoneInfoId == timezone).ToArray(),
                        EmailSubject = Templates.ShopReportEmailTemplate(template.Subject, request),
                        EmailBody = Templates.ShopReportEmailTemplate(template.EmailBody, request),
                        TextMessage = Templates.ShopReportEmailTemplate(template.TextMessage, request),
                        Attachments = attachments
                    };

                    // Send Message.
                    await SendNotifications(message);
                }
            }
        }

        internal async Task SendShopStatementNotification(int paymentId)
        {
            // Load Repair to Populate Template.
            var request = await _connection.QuerySingleAsync<StatementNotificationModel>("EXEC Notification.usp_GetStatementNotification @PaymentId", new { PaymentId = paymentId });
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Load Template.
            var template = await _connection.QuerySingleAsync<NotificationTemplateModel>(TemplateSql, new { TemplateName = "ShopStatementEmail" });

            // Load Reporting Contacts.
            var statementContacts = (await _contacts.GetStatementContacts(request.ShopGuid)).ToList();

            // Load Distinct Timezones.
            var timezones = statementContacts.Select(r => r.TimeZoneInfoId).Distinct().ToArray();

            // Process Report per Timezone.
            foreach (string timezone in timezones)
            {
                // Load Time Zone Offset.
                var offset = TimeZoneInfo.FindSystemTimeZoneById(timezone).GetUtcOffset(DateTimeOffset.UtcNow).ToString();

                // Load Report.
                var report = (await _reportGenerator.GetStatementReportPdfStreamAsync(paymentId, offset)).ToBase64String();

                // Create Attachment.
                var attachments = new List<INotificationMessageAttachment>
                    {
                        new NotificationMessageAttachment
                        {
                            Filename = $"AirProStatement-{request.PaymentId}.pdf",
                            MimeType = "application/pdf",
                            ContentBase64 = report
                        }
                    };

                // Populate Template.
                var message = new NotificationMessageModel
                {
                    Destinations = statementContacts.Where(r => r.TimeZoneInfoId == timezone).ToArray(),
                    EmailSubject = Templates.ShopStatementEmailTemplate(template.Subject, request),
                    EmailBody = Templates.ShopStatementEmailTemplate(template.EmailBody, request),
                    TextMessage = Templates.ShopStatementEmailTemplate(template.TextMessage, request),
                    Attachments = attachments
                };

                // Send Message.
                await SendNotifications(message);
            }
        }

        // Billing Notification Template.
        internal async Task SendBillingNotification(int repairId)
        {
            // Load Settings.
            var billingNotificationEmail = ConfigurationManager.AppSettings["NewBillingNotifications"];
            if (billingNotificationEmail == null) throw new ArgumentNullException(nameof(billingNotificationEmail));

            // Load Repair to Populate Template.
            var repair = await _connection.QuerySingleAsync<RepairNotificationModel>("EXEC Notification.usp_GetRepairNotification @RepairId", new { RepairId = repairId });
            if (repair == null) throw new ArgumentNullException(nameof(repair));

            // Send Notification.
            if (repair.InvoicedInd && repair.InvoiceTotal > 0)
            {
                // Load Template.
                var template = await _connection.QuerySingleAsync<NotificationTemplateModel>(TemplateSql, new { TemplateName = "BillingEmail" });

                // Load Report.
                var report = (await _reportGenerator.GetInvoiceReportPdfStreamAsync(repair.RepairId)).ToBase64String();

                // Populate Template.
                var message = new NotificationMessageModel
                {
                    Destinations = new[] { new NotificationDestinationModel { Email = billingNotificationEmail } },
                    EmailSubject = Templates.BillingEmailTemplate(template.Subject, repair),
                    EmailBody = Templates.BillingEmailTemplate(template.EmailBody, repair),
                    TextMessage = Templates.BillingEmailTemplate(template.TextMessage, repair),
                    Attachments = new List<INotificationMessageAttachment> {
                        new NotificationMessageAttachment
                        {
                            Filename = $"AirProInvoice-{repair.RepairId}.pdf",
                            MimeType = "application/pdf",
                            ContentBase64 = report
                        }
                    }
                };

                // Send Message.
                await SendNotifications(message);
            }
        }

        // Shop Invoice Notification Template.
        internal async Task SendShopInvoiceNotification(int repairId)
        {
            // Load Repair to Populate Template.
            var repair = await _connection.QuerySingleAsync<RepairNotificationModel>("EXEC Notification.usp_GetRepairNotification @RepairId", new { RepairId = repairId });
            if (repair == null) throw new ArgumentNullException(nameof(repair));

            // Check Customer Visible.
            if (repair.InvoicedInd)
            {
                // Load Template.
                var template = await _connection.QuerySingleAsync<NotificationTemplateModel>(TemplateSql, new { TemplateName = "ShopInvoiceEmail" });

                // Load Report.
                var attachments = new List<INotificationMessageAttachment>
                {
                    new NotificationMessageAttachment
                    {
                        Filename = $"AirProInvoice-{repair.RepairId}.pdf",
                        MimeType = "application/pdf",
                        ContentBase64 = (await _reportGenerator.GetInvoiceReportPdfStreamAsync(repair.RepairId)).ToBase64String()
                    }
                };

                // Load Position Statements.
                attachments.AddRange(await LoadAttachments(UploadType.VehicleMakes, repair.VehicleMakeId.ToString()));

                // Populate Template.
                var message = new NotificationMessageModel
                {
                    Destinations = await _contacts.GetBillingContacts(repair.ShopGuid),
                    EmailSubject = Templates.ShopInvoiceEmailTemplate(template.Subject, repair),
                    EmailBody = Templates.ShopInvoiceEmailTemplate(template.EmailBody, repair),
                    TextMessage = Templates.ShopInvoiceEmailTemplate(template.TextMessage, repair),
                    Attachments = attachments
                };

                // Send Message.
                await SendNotifications(message);
            }
        }

        internal async Task SendRepairFeedbackNotification(int repairId)
        {
            var feedbackNotifications = ConfigurationManager.AppSettings["FeedbackNotifications"];
            if (!string.IsNullOrWhiteSpace(feedbackNotifications))
            {
                // Load Repair Feedback to Populate Template.
                var feedback = await _connection.QuerySingleAsync<RepairFeedbackNotificationModel>("EXEC Reporting.usp_GetFeedbackReport @RepairId", new { RepairId = repairId });
                if (feedback == null) throw new ArgumentNullException(nameof(feedback));


                // Load Template.
                var template = await _connection.QuerySingleAsync<NotificationTemplateModel>(TemplateSql, new { TemplateName = "RepairFeedbackEmail" });
                // Populate Template.
                var message = new NotificationMessageModel
                {
                    Destinations = new[] { new NotificationDestinationModel() { Email = feedbackNotifications } },
                    EmailSubject = Templates.RepairFeedbackTemplate(template.Subject, feedback),
                    EmailBody = Templates.RepairFeedbackTemplate(template.EmailBody, feedback),
                    TextMessage = Templates.RepairFeedbackTemplate(template.TextMessage, feedback)
                };

                // Send Message.
                await SendNotifications(message);
            }
        }

        internal async Task SendRegistrationLinkNotification(int registrationShopId)
        {
            var registration = await _connection.QuerySingleAsync<RegistrationNotificationModel>("EXEC Notification.usp_GetRegistrationNotification @RegistrationShopId", new { RegistrationShopId = registrationShopId });
            if (registration == null) throw new ArgumentNullException(nameof(registration));

            // Load Template.
            var template = await _connection.QuerySingleAsync<NotificationTemplateModel>(TemplateSql, new { TemplateName = "RegistrationEmail" });
            // Populate Template.
            var message = new NotificationMessageModel
            {
                Destinations = new[] { new NotificationDestinationModel() { Email = registration.Email } },
                EmailSubject = Templates.RegistrationTemplate(template.Subject, registration),
                EmailBody = Templates.RegistrationTemplate(template.EmailBody, registration),
                TextMessage = Templates.RegistrationTemplate(template.TextMessage, registration)
            };

            // Send Message.
            await SendNotifications(message);
        }

        internal async Task SendRegistrationWelcomeNotification(int registrationShopId)
        {
            var welcome = await _connection.QuerySingleAsync<RegistrationWelcomeNotificationModel>("EXEC Notification.usp_GetRegistrationWelcomeNotification @RegistrationShopId", new { RegistrationShopId = registrationShopId });
            if (welcome == null) throw new ArgumentNullException(nameof(welcome));

            // Load Template.
            var template = await _connection.QuerySingleAsync<NotificationTemplateModel>(TemplateSql, new { TemplateName = "RegistrationWelcomeEmail" });
            // Populate Template.
            var message = new NotificationMessageModel
            {
                Destinations = new[] { new NotificationDestinationModel() { Email = welcome.Email } },
                EmailSubject = Templates.RegistrationWelcomeTemplate(template.Subject, welcome),
                EmailBody = Templates.RegistrationWelcomeTemplate(template.EmailBody, welcome),
                TextMessage = Templates.RegistrationWelcomeTemplate(template.TextMessage, welcome)
            };

            // Send Message.
            await SendNotifications(message);
        }

        internal async Task SendScanToolIssuesNotification(int requestId)
        {
            var scanToolIssueNotificationEmail = ConfigurationManager.AppSettings["ScanToolIssueNotificationEmail"];
            if (!string.IsNullOrWhiteSpace(scanToolIssueNotificationEmail))
            {
                // Load Scan Tool Issues Notification to Populate Template.
                var notification = await _connection.QuerySingleAsync<ScanToolIssuesNotificationModel>("EXEC Notification.usp_GetScanToolIssuesNotification @RequestId", new { RequestId = requestId });
                if (notification == null) throw new ArgumentNullException(nameof(notification));


                // Load Template.
                var template = await _connection.QuerySingleAsync<NotificationTemplateModel>(TemplateSql, new { TemplateName = "ScanToolIssuesEmail" });
                // Populate Template.
                var message = new NotificationMessageModel
                {
                    Destinations = new[] { new NotificationDestinationModel() { Email = scanToolIssueNotificationEmail } },
                    EmailSubject = Templates.ScanToolIssueTemplate(template.Subject, notification),
                    EmailBody = Templates.ScanToolIssueTemplate(template.EmailBody, notification),
                    TextMessage = Templates.ScanToolIssueTemplate(template.TextMessage, notification)
                };

                // Send Message.
                await SendNotifications(message);
            }
        }

        private async Task SendNotifications(INotificationMessage message)
        {
            // Load Messaging.
            var messaging = new Messages(_settings);

            // Queue Tasks.
            var send = new List<Task>();
            if (message.Destinations.Any(d => !string.IsNullOrEmpty(d.Email)))
                send.Add(messaging.SendMailMessageAsync(message, _overrideEmail));

            if (message.Destinations.Any(d => !string.IsNullOrEmpty(d.Text)) && message.TextMessage.Length > 0)
                send.Add(messaging.SendTextMessageAsync(message, _overrideEmail));

            // Send All Messages.
            await Task.WhenAll(send.ToArray());
        }

        #region Attachments

        private async Task<IEnumerable<INotificationMessageAttachment>> LoadAttachments(UploadType type, string key)
        {
            var result = new List<INotificationMessageAttachment>();

            var sql = @"SELECT UploadFileName + '.' + UploadFileExtension [Filename], UploadMimeType [MimeType], UploadStorageName [StorageName]
                        FROM Common.Uploads WHERE UploadDeletedInd = 0 AND UploadTypeId = @UploadTypeId AND UploadKey = @UploadKey";

            if (string.IsNullOrEmpty(key) || type == 0) return result;

            var q = await _connection.QueryAsync(sql, new { UploadTypeId = (int)type, UploadKey = key });

            result.AddRange(q.Select(u => new NotificationMessageAttachment
            {
                Filename = u.Filename,
                MimeType = u.MimeType,
                ContentBase64 = LoadFileBase64(u.StorageName)
            }).Where(u => !string.IsNullOrEmpty(u.ContentBase64)).ToList());

            return result;
        }

        private string LoadFileBase64(string storageName)
        {
            // Check Connection String.
            if (!CloudStorageAccount.TryParse(StorageAccount, out var storageAccount)) return null;

            // Create Client.
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();

            // Create Upload Container.
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("uploads");
            cloudBlobContainer.CreateIfNotExists();

            // Download File Stream.
            using (var file = new MemoryStream())
            {
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(storageName);
                if (cloudBlockBlob.Exists()) cloudBlockBlob.DownloadToStream(file);

                // Check Stream.
                if (file.Length <= 0) return null;

                file.Seek(0, SeekOrigin.Begin);
                return file.ToBase64String();
            }
        }

        #endregion

    }
}