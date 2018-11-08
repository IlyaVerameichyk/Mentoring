using SystemWatcher;
using SystemWatcher.Analyzer;
using SystemWatcher.Exporters;
using SystemWatcher.LocalSystemWatcher;
using Service.QueueManager;
using Topshelf;

namespace Service
{
    public class Service : ServiceControl
    {
        private FileManager _fileManager;

        public bool Start(HostControl hostControl)
        {
            var analyzer = new BarcodeAnalyzer();
            var settingsManager = new SettingsManager(analyzer);

            _fileManager = new FileManager(new LocalSystemWatcher(new[] { "D:\\test" }), analyzer,
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