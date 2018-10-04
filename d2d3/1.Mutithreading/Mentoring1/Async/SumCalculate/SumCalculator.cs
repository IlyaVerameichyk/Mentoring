using System;
using System.Threading;
using System.Threading.Tasks;

namespace Async.SumCalculate
{
    public class SumCalculator
    {
        public async Task CalculateSumAsync(long start, long tailInclusive, CancellationToken? token = null)
        {
            try
            {
                var task = await Task.Factory.StartNew(() =>
                {
                    checked
                    {
                        long result = 0;
                        for (var i = start; i <= tailInclusive; i++)
                        {
                            result += i;
                            token?.ThrowIfCancellationRequested();
                        }
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                        return result;
                    }
                });
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine($"Result {task}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}