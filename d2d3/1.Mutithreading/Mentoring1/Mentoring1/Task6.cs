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
        private static readonly AutoResetEvent AutoResetEventForAdd = new AutoResetEvent(true);
        private static readonly AutoResetEvent AutoResetEventForPrint = new AutoResetEvent(false);
        
        public static void Run()
        {
            var addTask = new Task(() =>
            {
                foreach (var i in Enumerable.Range(0, 10))
                {
                    AutoResetEventForAdd.WaitOne();
                    AutoResetEventForAdd.Reset();
                    Console.WriteLine($"Add value {i}");
                    AddToCollection(i);
                    Console.WriteLine($"Finish add value {i}");
                    AutoResetEventForPrint.Set();
                }
            });

            var printTask = new Task(() =>
            {
                foreach (var i in Enumerable.Range(0, 10))
                {
                    AutoResetEventForPrint.WaitOne();
                    AutoResetEventForPrint.Reset();
                    Console.WriteLine("Print collection");
                    IterateCollection(Console.WriteLine);
                    Console.WriteLine("Finish print collection");
                    AutoResetEventForAdd.Set();
                }
            });

            addTask.Start();
            printTask.Start();
            addTask.Wait();
            printTask.Wait();
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