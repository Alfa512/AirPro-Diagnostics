using System;
using System.Configuration;
using System.Threading.Tasks;
using AirPro.Common.Enumerations;
using AirPro.Storage.Models.Concrete;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace AirPro.Storage
{
    public class MessageQueue : IDisposable
    {
        private readonly string _connectionString;

        public MessageQueue() : this(ConfigurationManager.ConnectionStrings?["AzureWebJobsStorage"]?.ConnectionString) { }

        public MessageQueue(string storageConnectionString)
        {
            _connectionString = storageConnectionString;
        }

        public async Task AddMitchellReportQueueMessageAsync(int requestId, Guid userGuid, string queueName = null)
        {
            // Load Default Queue Name.
            queueName = queueName ?? ConfigurationManager.AppSettings["MitchellQueue"];

            // Create Message.
            var message = new MitchellReportQueueItem
            {
                RequestId = requestId,
                UserGuid = userGuid,
                TimestampOffset = DateTimeOffset.UtcNow
            };
            var cloudQueueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(message));

            // Add to Queue.
            await AddCloudQueueMessageAsync(queueName, cloudQueueMessage);
        }

        public async Task AddNotificationQueueMessageAsync(NotificationTemplate template, int id, Guid userGuid, string queueName = null)
        {
            // Load Default Queue Name.
            queueName = queueName ?? ConfigurationManager.AppSettings["NotificationQueue"];

            // Create Message.
            var message = new NotificationQueueMessage
            {
                TemplateName = template,
                Identifier = id,
                UserGuid = userGuid,
                TimestampOffset = DateTimeOffset.UtcNow
            };
            var cloudQueueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(message));

            // Add to Queue.
            await AddCloudQueueMessageAsync(queueName, cloudQueueMessage);
        }

        private async Task AddCloudQueueMessageAsync(string queueName, CloudQueueMessage message)
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

            // Add Queue Message.
            await queue.AddMessageAsync(message);
        }

        public void Dispose()
        {
            /* Previously Implemented with Disposable Pattern. */
        }
    }
}
