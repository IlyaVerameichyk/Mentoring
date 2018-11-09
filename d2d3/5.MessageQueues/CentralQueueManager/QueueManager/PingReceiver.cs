using System;
using SystemWatcher.Models;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace QueueManager
{
    public class PingReceiver
    {

        private const string ServiceBusConnectionString = "Endpoint=sb://mqmentoring.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=UlNEqYdOZZUz3SKwcoK7knvXl/3scewYuKWSSBNEvg8=";
        private const string TopicName = "settingstopic";
        private const string SubscriptionName = "PingSubscription";

        public PingReceiver()
        {
            var nsm = NamespaceManager.CreateFromConnectionString(ServiceBusConnectionString);
            if (nsm.SubscriptionExists(TopicName, SubscriptionName))
            {
                nsm.DeleteSubscription(TopicName, SubscriptionName);
            }
            nsm.CreateSubscription(TopicName, SubscriptionName, new SqlFilter("sys.To = 'CentralQueueManager' AND ActionName = 'Ping'"));

            var client = SubscriptionClient.CreateFromConnectionString(ServiceBusConnectionString, TopicName, SubscriptionName);
            client.OnMessage(ProcessMessage);
        }

        private void ProcessMessage(BrokeredMessage obj)
        {
            var guid = (Guid)obj.Properties["Guid"];
            var status = obj.GetBody<WatcherStatus>();
            Console.WriteLine($"Status of {guid}: {status.ToString()}");
            obj.Complete();
        }
    }
}