using System;
using System.Threading;
using System.Threading.Tasks;

namespace Async.SumCalculate
{
    public class Task1
    {
        public static void Run()
        {
            var calculator = new SumCalculator();
            Console.WriteLine("Press Ctrl+C to exit");
            var cts = new CancellationTokenSource();
            while (true)
            {
                Console.WriteLine("Calculate sum of count:");
                long count;
                if(!long.TryParse(Console.ReadLine(), out count))
                {
                    Console.WriteLine("Please, input value corresponding to long.");
                    continue;
                }
                cts.Cancel();
                cts = new CancellationTokenSource();
                calculator.CalculateSumAsync(0, count, cts.Token).ConfigureAwait(true);
            }
        }
    }
}