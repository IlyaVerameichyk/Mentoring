using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueueManager;

namespace Runner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var manager = new FileManager(new QueueReader());
            Console.ReadLine();
        }
    }
}
