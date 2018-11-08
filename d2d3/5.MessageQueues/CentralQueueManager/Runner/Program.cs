using System;
using QueueManager;

namespace Runner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var manager = new FileManager(new QueueReader());
            var fileReadersSettingsManager = new FileReadersSettingsManager();

            Console.WriteLine("Type 'ping' to ping services now, otherwise - stop word");
            while (true)
            {
                var input = Console.ReadLine();
                if ("ping".Equals(input, StringComparison.InvariantCultureIgnoreCase))
                {
                    fileReadersSettingsManager.PingNow();
                }
                else if(!string.IsNullOrWhiteSpace(input))
                {
                    fileReadersSettingsManager.SetQrTerminateWord(input);
                }
            }

        }
    }
}
