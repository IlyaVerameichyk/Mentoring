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
                        return result;
                    }
                });
                Console.WriteLine($"Result {task}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}