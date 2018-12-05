using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Profiling
{
    public class Program
    {
        private static readonly string Password = new string('a', 1000);

        public static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var result = GeneratePasswordHashUsingSaltOriginal(Password, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });
            var timeOriginal = sw.ElapsedMilliseconds;
            sw.Restart();
            var result1 = GeneratePasswordHashUsingSaltOptimized(Password, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });
            var timeOptimized = sw.ElapsedMilliseconds;
            
        }

        public static string GeneratePasswordHashUsingSaltOriginal(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }

        public static string GeneratePasswordHashUsingSaltOptimized(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(Encoding.ASCII.GetBytes(passwordText), salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }
    }
}
