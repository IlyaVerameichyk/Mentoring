using SystemWatcher;
using SystemWatcher.Analyzer;
using SystemWatcher.Exporters;
using SystemWatcher.LocalSystemWatcher;
using SystemWatcher.Models.Interfaces;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Service.Interceptors;

namespace Service.AutofacConfig
{
    public static class AutofacConfiguration
    {
        public static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<TraceInterceptor>().AsSelf();

            builder.RegisterType<LocalSystemWatcher>().As<IFileSystemWatcher>()
                .WithParameters(new[]
                {
                    new NamedParameter("badFilesDirectory", "D:\\test\\bad"),
                    new NamedParameter("paths", new[] {"D:\\test"}),
                }).EnableInterfaceInterceptors().InterceptedBy(typeof(TraceInterceptor));

            builder.RegisterType<BarcodeAnalyzer>().As<IFilesAnalyzer>().EnableInterfaceInterceptors().InterceptedBy(typeof(TraceInterceptor));
            builder.RegisterType<LocalSystemFileExporter>().As<IFileExporter>()
                .WithParameter("destinationPath", "D:\\test\\dest").EnableInterfaceInterceptors().InterceptedBy(typeof(TraceInterceptor));

            builder.RegisterType<FileManager>().AsSelf();
            builder.RegisterType<Service>().AsSelf();

            return builder.Build();
        }
    }
}