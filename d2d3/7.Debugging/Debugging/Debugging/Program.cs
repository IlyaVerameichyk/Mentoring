using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace Debugging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
            if (networkInterface == null)
            {
                throw new ApplicationException("Network interface required");
            }
            var addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes().Select(b => (int)b).ToArray();
            var dateBytes = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());
            var result = addressBytes.Zip(dateBytes, (i, b) => i ^ b).Select(b => b * 10);
            Console.WriteLine("Key:");
            Console.WriteLine(string.Join("-", result));
            Console.ReadKey();
        }
    }
}
