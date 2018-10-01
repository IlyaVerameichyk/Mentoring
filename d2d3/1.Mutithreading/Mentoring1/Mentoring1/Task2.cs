using System;
using System.Linq;

namespace Mentoring1
{
    public class Task2
    {

        public 

        public int[] SortArray(int[] array)
        {
            return array.OrderBy(i => i).ToArray();
        }

        public int[] MultiplyArray(int[] array)
        {
            var factor = new Random().Next();
            return array.Select(i => i * factor).ToArray();
        }

        public int[] CreateArray()
        {
            var random = new Random();
            var result = new byte[10];
            random.NextBytes(result);
            return result.Select(i => (int) i).ToArray();
        }
    }
}