using System;
using Microsoft.ServiceBus.Messaging;
using QueueManager.Models;

namespace QueueManager.FileReceiving
{
    public class QueueReader
    {
        private const string QueueName = "filequeue";
        private QueueClient _client;

        public void StartListen()
        {
            _client = QueueClient.Create(QueueName);
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