using System;
using System.IO;
using Topshelf;

namespace Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<Service>(s =>
                {
                    try
                    {
                        s.ConstructUsing(name => new Service());
                        s.WhenStarted(ts => ts.Start(null));
                        s.WhenStopped(ts => ts.Stop(null));
                    }
                    catch (Exception e)
                    {
                        System.IO.File.WriteAllText("C:\\Users\\Ilya_Verameichyk\\Desktop\\123.txt", e.ToString());
                    }
                });

                System.IO.File.WriteAllText("C:\\Users\\Ilya_Verameichyk\\Desktop\\123.txt", "123");

            });
        }
    }
}
