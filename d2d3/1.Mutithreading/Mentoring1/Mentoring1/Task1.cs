﻿using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mentoring1
{
    public class Task1
    {
        public static void Run()
        {
            var taskArray = Enumerable.Range(1, 100)
                .Select(i => Task.Factory.StartNew(() => IterateAndPrint(i, 1000, Console.WriteLine))).ToArray();
            Task.WaitAll(taskArray);
        }

        private static void IterateAndPrint(int taskNumber, int count, Action<string> output)
        {
            foreach (var i in Enumerable.Range(1, count))
            {
                output($"Task #{taskNumber} - {i}");
            }
        }
    }
}