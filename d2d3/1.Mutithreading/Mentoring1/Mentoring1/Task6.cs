using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mentoring1
{
    public static class Task6
    {
        private static readonly IList<int> _list = new List<int>();
        private static readonly AutoResetEvent AutoResetEvent = new AutoResetEvent(false);

        public static void Run()
        {
            var addTask = new Task(() =>
            {
                foreach (var i in Enumerable.Range(0, 10))
                {
                    Console.WriteLine($"Add value {i}");
                    AddToCollection(i);
                    Console.WriteLine($"Finish add value {i}");
                    AutoResetEvent.Set();
                    AutoResetEvent.WaitOne();
                }
            });

            var printTask = new Task(() =>
            {
                AutoResetEvent.WaitOne();
                foreach (var i in Enumerable.Range(0, 10))
                {
                    Console.WriteLine("Print collection");
                    IterateCollection(Console.WriteLine);
                    Console.WriteLine("Finish print collection");
                    AutoResetEvent.Set();
                    AutoResetEvent.WaitOne();
                }
            });

            addTask.Start();
            printTask.Start();
            Task.WaitAll(addTask, printTask);
        }

        private static void AddToCollection(int value)
        {
            _list.Add(value);
        }

        private static void IterateCollection(Action<int> action)
        {
            foreach (var value in _list)
            {
                action(value);
            }
        }
    }
}