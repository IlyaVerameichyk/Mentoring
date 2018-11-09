using System;
using SystemWatcher.Models;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace QueueManager
{
    public class PingReceiver
    {
        private const string TopicName = "settingstopic";
        private const string SubscriptionName = "PingSubscription";

        public PingReceiver()
        {
            var nsm = NamespaceManager.Create();
            if (nsm.SubscriptionExists(TopicName, SubscriptionName))
            {
                nsm.DeleteSubscription(TopicName, SubscriptionName);
            }
            nsm.CreateSubscription(TopicName, SubscriptionName, new SqlFilter("sys.To = 'CentralQueueManager' AND ActionName = 'Ping'"));

            var client = SubscriptionClient.Create(TopicName, SubscriptionName);
            client.OnMessage(ProcessMessage);
        }

        private static void ProcessMessage(BrokeredMessage obj)
        {
            var guid = (Guid)obj.Properties["Guid"];
            var status = obj.GetBody<WatcherStatus>();
            Console.WriteLine($"Status of {guid}: {status.ToString()}");
            obj.Complete();
        }
    }
}