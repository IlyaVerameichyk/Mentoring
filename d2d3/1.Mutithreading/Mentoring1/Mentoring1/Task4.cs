using System;
using System.Threading;

namespace Mentoring1
{
    public static class Task4
    {
        public static void Run()
        {
            var thread = new Thread(CreateThreads);
            thread.Start(10);
            thread.Join();
        }

        public static void CreateThreads(object countObj)
        {
            if (!(countObj is int)) { throw new ArgumentException(nameof(countObj)); }
            var number = (int)countObj;

            Console.WriteLine($"Thread #{number}");
            if (number <= 1)
            {
                return;
            }

            Console.WriteLine($"#{number} Creating inner thread");
            var innerThread = new Thread(CreateThreads);
            innerThread.Start(number - 1);
            innerThread.Join();
            Console.WriteLine($"#{number} inner thread joined");
        }
    }
}
