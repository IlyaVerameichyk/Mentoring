using System;
using SystemWatcher;
using SystemWatcher.Analyzer;
using SystemWatcher.Exporters;
using SystemWatcher.LocalSystemWatcher;

namespace Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var a = new FileManager(new LocalSystemWatcher(new[] {"D:\\test"}), new BarcodeAnalyzer(),
                new LocalSystemFileExporter("D:\\test\\dest"));

            Console.ReadKey();
        }
    }
}
