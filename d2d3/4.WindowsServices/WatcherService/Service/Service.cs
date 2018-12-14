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
        private readonly FileManager _fileManager;

        public Service(FileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public bool Start(HostControl hostControl)
        {
            //var analyzer = new BarcodeAnalyzer();
            ////var settingsManager = new SettingsManager(analyzer);

            //_fileManager = new FileManager(new LocalSystemWatcher("D:\\test\\bad", new[] { "D:\\test" }), analyzer,
            //    new LocalSystemFileExporter("D:\\test\\dest"));
            //var pm = new PingManager(_fileManager);
            _fileManager.StartListen();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _fileManager.StopListen();
            return true;
        }
    }
}