using System;
using System.Configuration;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Common.Interfaces;
using AirPro.Library.Models.Concrete;
using AirPro.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace AirPro.Library
{
    public partial class QueueLibrary : BaseLibrary
    {
        private readonly string _connectionString;

        public QueueLibrary() : this(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString) { }

        public QueueLibrary(string storageConnectionString)
        {
            _connectionString = storageConnectionString;
        }

        private async Task<CloudQueue> GetQueue(string queueName)
        {
            // Check Queue Name.
            if (queueName == null) throw new ArgumentNullException(nameof(queueName));

            // Load Storage Account.
            if (_connectionString == null)
                throw new ArgumentNullException(nameof(_connectionString));
            var storageAccount = CloudStorageAccount.Parse(_connectionString);

            // Create Client.
            var queueClient = storageAccount.CreateCloudQueueClient();

            // Get Queue Ref.
            var queue = queueClient.GetQueueReference(queueName);
            
            // Check Queue.
            await queue.CreateIfNotExistsAsync();

            // Return Queue.
            return queue;
        }

        public async Task AddQueueMitchellReport(int requestId, Guid userGuid, string queueName = null)
        {
            try
            {
                // Create Message.
                var message = new MitchellReportQueueItem
                {
                    RequestId = requestId,
                    UserGuid = userGuid,
                    TimestampOffset = DateTimeOffset.UtcNow
                };
                var cloudQueueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(message));

                // Add to Queue.
                var queue = await GetQueue(queueName ?? ConfigurationManager.AppSettings["MitchellQueue"]);
                await queue.AddMessageAsync(cloudQueueMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public async Task AddQueueNotification(NotificationTemplates template, int id, Guid userGuid, string queueName = null)
        {
            try
            {
                // Create Message.
                var message = new QueueNotification
                {
                    TemplateName = template,
                    Identifier = id,
                    UserGuid = userGuid,
                    TimestampOffset = DateTimeOffset.UtcNow
                };
                var cloudQueueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(message));

                // Add to Queue.
                var queue = await GetQueue(queueName ?? ConfigurationManager.AppSettings["NotificationQueue"]);
                await queue.AddMessageAsync(cloudQueueMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public async Task AddQueueDebug(string message, string queueName = null)
        {
            try
            {
                // Create Message.
                var mesg = new CloudQueueMessage(message);

                // Add to Queue.
                var queue = await GetQueue(queueName ?? ConfigurationManager.AppSettings["debug"]);
                await queue.AddMessageAsync(mesg).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public async Task AddQueueTwilio(IQueueTwilio message, string queueName = null)
        {
            try
            {
                // Create Message.
                var mesg = new CloudQueueMessage(JsonConvert.SerializeObject(message));

                // Add to Queue.
                var queue = await GetQueue(queueName ?? ConfigurationManager.AppSettings["TwilioQueue"]);
                await queue.AddMessageAsync(mesg).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
