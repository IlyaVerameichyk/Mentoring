using Topshelf;

namespace Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x => x.Service<Service>());
        }
    }
}
