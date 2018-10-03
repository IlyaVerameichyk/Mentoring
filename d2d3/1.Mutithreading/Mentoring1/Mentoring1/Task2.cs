using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mentoring1
{
    public class Task2
    {
        public static void Run()
        {
            var task = Task.Factory.StartNew(CreateArray)
                .ContinueWith(prev => MultiplyArray(prev.Result))
                .ContinueWith(prev => SortArray(prev.Result))
                .ContinueWith(prev => GetAverage(prev.Result));
            Console.WriteLine($"Average value: {task.Result}");
        }

        public static double GetAverage(int[] array)
        {
            var result = array.Average();
            return result;
        }

        public static int[] SortArray(int[] array)
        {
            var result =  array.OrderBy(i => i).ToArray();
            PrintToConsole("Sorted array", result);
            return result;
        }

        public static int[] MultiplyArray(int[] array)
        {
            var factor = new Random().Next(10);
            var result = array.Select(i => i * factor).ToArray();
            PrintToConsole("Multiplied array", result);
            return result;
        }

        public static int[] CreateArray()
        {
            var random = new Random();
            var randomsArray = new byte[10];
            random.NextBytes(randomsArray);
            var result = randomsArray.Select(i => (int)i).ToArray();
            PrintToConsole("Random array:", result);
            return result;
        }

        public static Task PrintToConsole(string message, int[] value)
        {
            return Task.Factory.StartNew(() =>
            {
                Console.WriteLine(message);
                Console.WriteLine(string.Join(Environment.NewLine, value));
            });
        }
    }
}