using AirPro.Messaging.Interface;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AirPro.Logging;
using SendGrid;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using UniMatrix.Common.Extensions;

namespace AirPro.Messaging
{
    public class Messages
    {
        private readonly IMessagingSettings _settings;

        public Messages(IMessagingSettings settings)
        {
            _settings = settings;
        }

        public async Task SendTextMessageAsync(INotificationMessage message, string overrideEmail = null)
        {
            // Check Settings.
            if (_settings.TwilioSid == null) throw new ArgumentNullException(nameof(_settings.TwilioSid));
            if (_settings.TwilioToken == null) throw new ArgumentNullException(nameof(_settings.TwilioToken));
            if (_settings.TwilioFromPhone == null) throw new ArgumentNullException(nameof(_settings.TwilioFromPhone));

            // Create Client.
            TwilioClient.Init(_settings.TwilioSid, _settings.TwilioToken);

            // Scrub HTML from Links.
            var text = message.TextMessage.Replace("<a href='", "").Replace("'>View Report</a>", "").Replace("'>View Invoice</a>", "");

            // Check Override Email.
            if (string.IsNullOrEmpty(overrideEmail))
            {
                // Send Message(s).
                foreach (var destination in message.Destinations.Where(d => d.Text != null).Select(d => d.Text).Distinct())
                {
                    // Send Message.
                    var resource = await MessageResource.CreateAsync(
                        to: new PhoneNumber(destination),
                        from: new PhoneNumber(_settings.TwilioFromPhone),
                        body: text);
                    
                    // Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
                    Trace.TraceInformation(resource.Status.ToString());

                    // Log Message.
                    await Logger.LogTextNotification(destination, text, resource.Status.ToString());
                }
            }
            else
            {
                // Build Message Body.
                var body = GetOverrideBody(message.Destinations.Select(d => d.Text).Distinct(), HeaderType.Text, text);

                // Send Override Email.
                var mail = GetMail(overrideEmail, "Text Message Redirect", body);
                await SendMail(mail);
            }

            // Twilio doesn't currently have an async API, so return success.
            await Task.FromResult(0);
        }

        public async Task SendMailMessageAsync(INotificationMessage message, string overrideEmail = null)
        {
            // Check Settings.
            if (_settings.SendGridAccount == null) throw new ArgumentNullException(nameof(_settings.SendGridAccount));
            if (_settings.SendGridAccountKey == null) throw new ArgumentNullException(nameof(_settings.SendGridAccountKey));

            // Load Messages to Send.
            var send = new List<Task>();
            if (string.IsNullOrEmpty(overrideEmail))
            {
                // Process Addresses.
                send.AddRange(from destination in message.Destinations.Where(d => d.Email != null).Select(d => d.Email).Distinct()
                    let mail = GetMail(destination, message.EmailSubject, message.EmailBody, message.Attachments) select SendMail(mail));
            }
            else
            {
                // Build Message Body.
                var body = GetOverrideBody(message.Destinations.Select(d => d.Email).Distinct(), HeaderType.Email, message.EmailBody);

                // Send Override Email.
                var mail = GetMail(overrideEmail, message.EmailSubject, body, message.Attachments);
                send.Add(SendMail(mail));
            }

            // Send All Messages.
            await Task.WhenAll(send.ToArray());
        }

        #region Override Helpers

        private enum HeaderType
        {
            Text,
            Email
        }

        private static string GetOverrideBody(IEnumerable<string> destinations, HeaderType type, string body) => $"<hr/><h4>Testing Environment { type.ToString() } Redirect</h4><strong>{ type.ToString() } Recipient List:</strong><br/>{ string.Join(",<br/>", destinations) }<br/><br/><hr/>{ body }";

        #endregion

        #region Mail Helpers

        private async Task SendMail(SendGridMessage mail)
        {
            // Create Web Transport.
            var client = new SendGridClient(_settings.SendGridAccountKey);

            // Send/Log Message.
            var status = "Unknown";
            var response = await client.SendEmailAsync(mail);
            if (response != null && response.GetType() == typeof(Response))
                status = response.StatusCode.ToString();

            // Log Message.
            await Logger.LogEmailNotification(string.Join(",", mail.Personalizations[0].Tos.Select(t => t.Email)), mail.Subject, mail.Contents[0].Value, status).ConfigureAwait(false);
        }

        private SendGridMessage GetMail(string destination, string subject, string body, IEnumerable<INotificationMessageAttachment> attachments = null)
        {
            // Create Message.
            var from = new EmailAddress(_settings.SendGridFromAddress, _settings.SendGridFromName);
            var to = new EmailAddress(destination);
            var mail = MailHelper.CreateSingleEmail(from, to, subject, null, body);

            // Add Attachment(s).
            mail.Attachments = attachments?.Select(a => new Attachment
            {
                Filename = a.Filename,
                Type = a.MimeType,
                Content = a.ContentBase64
            }).ToList();

            // Return.
            return mail;
        }

        #endregion
    }
}
