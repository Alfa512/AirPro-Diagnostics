using System;
using System.Configuration;
using Microsoft.Azure.WebJobs;

namespace AirPro.Notifications.WebJob
{
    class QueueNameResolver : INameResolver
    {
        public string Resolve(string name)
        {
            var queueName = ConfigurationManager.AppSettings[name];
            if (queueName == null) throw new ArgumentNullException(nameof(queueName));

            return queueName;
        }
    }
}