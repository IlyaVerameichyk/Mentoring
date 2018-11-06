using SystemWatcher;
using SystemWatcher.Analyzer;
using SystemWatcher.Exporters;
using SystemWatcher.LocalSystemWatcher;
using Topshelf;

namespace Service
{
    public class Service : ServiceControl
    {
        private FileManager _fileManager;

        public bool Start(HostControl hostControl)
        {
            _fileManager = new FileManager(new LocalSystemWatcher(new[] { "D:\\test" }), new BarcodeAnalyzer(),
                new ServiceBusExporter());

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _fileManager.StopListen();
            return true;
        }
    }
}