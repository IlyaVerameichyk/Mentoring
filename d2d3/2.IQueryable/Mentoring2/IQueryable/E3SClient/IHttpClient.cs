using System;
using System.Threading.Tasks;

namespace Sample03.E3SClient
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(Uri requestUri);
    }
}