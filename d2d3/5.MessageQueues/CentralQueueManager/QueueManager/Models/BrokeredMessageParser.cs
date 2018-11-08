using System;
using System.IO;
using Microsoft.ServiceBus.Messaging;

namespace QueueManager.Models
{
    public class BrokeredMessageParser
    {
        private readonly Stream _stream;

        public BrokeredMessageParser(BrokeredMessage message)
        {
            if (!message.Properties.ContainsKey("Size") || !(message.Properties["Size"] is int) ||
                !message.Properties.ContainsKey("Position") || !(message.Properties["Position"] is int) ||
                string.IsNullOrWhiteSpace(message.Label))
            {
                message.DeadLetter();
                throw new ArgumentException("Invalid message format");
            }
            Size = (int)message.Properties["Size"];
            Position = (int)message.Properties["Position"];
            FileName = message.Label;
            var bytes = message.GetBody<byte[]>();
            var ms = new MemoryStream(bytes);
            ms.Seek(0, SeekOrigin.Begin);
            _stream = ms;
            message.Complete();
        }

        public int Size { get; }

        public int Position { get; }

        public string FileName { get; }

        public Stream GetStream()
        {
            return _stream;
        }
    }
}
