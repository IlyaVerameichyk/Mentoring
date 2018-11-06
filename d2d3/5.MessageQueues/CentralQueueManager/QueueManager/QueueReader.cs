﻿using System;
using Microsoft.ServiceBus.Messaging;
using QueueManager.Models;

namespace QueueManager
{
    public class QueueReader
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://mentoringmq.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=uwBuFNU0hZPYSigS5NFeQx2xqoWDRiQIBn6oRZdXoxA=";
        private const string QueueName = "filequeue";
        private QueueClient _client;

        public void StartListen()
        {
            _client = QueueClient.CreateFromConnectionString(ServiceBusConnectionString, QueueName);
            _client.OnMessage(ProcessMessage);
            
        }

        public void StopListen()
        {
            _client.Close();
            _client = null;
        }

        public event EventHandler<MessageEventArgs> MessageReceived;

        private void ProcessMessage(BrokeredMessage message)
        {
            try
            {
                var parser = new BrokeredMessageParser(message);
                MessageReceived?.Invoke(this, new MessageEventArgs(parser));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}