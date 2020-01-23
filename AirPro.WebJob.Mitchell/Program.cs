using System.Configuration;
using AirPro.Logging;
using Microsoft.Azure.WebJobs;

namespace AirPro.WebJob.Mitchell
{
    static class Program
    {
        private static string _connectionString;
        internal static string ConnectionString => _connectionString ?? (_connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

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
