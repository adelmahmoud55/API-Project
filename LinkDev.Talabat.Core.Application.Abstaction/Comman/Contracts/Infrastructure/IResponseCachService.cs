using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstaction.Comman.Contracts.Infrastructure
{
    public interface IResponseCachService
    {
        //key means id respose
        Task CachResponseAsync(string key, object response, TimeSpan timeToLive);

        // must be nullable to handle cases where the key does not exist, bm3na el cach b2a expired
        Task<string?> GetCachedResponseAsync(string key);
    }
}
