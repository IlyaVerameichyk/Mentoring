using Service.AutofacConfig;
using Topshelf;
using Topshelf.Autofac;

namespace Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = AutofacConfiguration.GetContainer();
            HostFactory.Run(x =>
            {
                x.UseAutofacContainer(container);
                x.UseNLog();
                x.Service<Service>(s =>
                {
                    s.ConstructUsingAutofacContainer();
                    s.WhenStarted((ser, cont) => ser.Start(cont));
                    s.WhenStopped((ser, cont) => ser.Stop(cont));
                });
            });
        }
    }
}
