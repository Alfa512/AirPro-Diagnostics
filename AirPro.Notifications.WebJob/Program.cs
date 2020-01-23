using System.Configuration;
using AirPro.Logging;
using AirPro.Notifications.WebJob.Models;
using Microsoft.Azure.WebJobs;

namespace AirPro.Notifications.WebJob
{
    static class Program
    {
        private static string _overrideEmail;
        internal static string OverrideEmail => _overrideEmail ?? (_overrideEmail = ConfigurationManager.AppSettings["OverrideEmail"]);

        private static string _connectionString;
        internal static string ConnectionString => _connectionString ?? (_connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        private static MessagingSettingsModel _settings;
        internal static MessagingSettingsModel Settings => _settings ?? (_settings = new MessagingSettingsModel
        {
            SendGridAccount = ConfigurationManager.AppSettings["SendGridAccount"],
            SendGridAccountKey = ConfigurationManager.AppSettings["SendGridApiKey"],
            SendGridFromAddress = ConfigurationManager.AppSettings["MailFromAddress"],
            SendGridFromName = ConfigurationManager.AppSettings["MailFromName"],
            TwilioSid = ConfigurationManager.AppSettings["TwilioSid"],
            TwilioToken = ConfigurationManager.AppSettings["TwilioToken"],
            TwilioFromPhone = ConfigurationManager.AppSettings["TwilioFromPhone"]
        });

        static void Main()
        {
            // Initialize Logger.
            Logger.Initialize(ConnectionString);

            // Load Configuration.
            var config = new JobHostConfiguration { NameResolver = new QueueNameResolver() };
            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            // Start Host.
            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
