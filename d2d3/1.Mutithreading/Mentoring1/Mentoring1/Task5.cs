using System;
using System.Threading;

namespace Mentoring1
{
    public static class Task5
    {
        private const string SemaphoreName = "Semaphore";

        public static void Run()
        {
            var semaphore = new Semaphore(0, 1, SemaphoreName);
            ThreadPool.SetMinThreads(10, 0);
            ThreadPool.QueueUserWorkItem(CreateThread, 10);

        }

        public static void CreateThread(object countObj)
        {
            var semaphore = Semaphore.OpenExisting(SemaphoreName);
            if (!(countObj is int)) { throw new ArgumentException(nameof(countObj)); }
            var number = (int)countObj;
            if (number <= 0)
            {
                semaphore.Release();
                return;
            }

            Console.WriteLine($"Thread #{number}");
            Console.WriteLine($"#{number} Creating inner thread");
            ThreadPool.QueueUserWorkItem(CreateThread, number - 1);

            semaphore.WaitOne();
            Console.WriteLine($"#{number} inner thread joined");
            semaphore.Release();
        }
    }
}