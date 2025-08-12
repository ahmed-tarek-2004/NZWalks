using Microsoft.Extensions.Caching.Distributed;
using NZWalk.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace NZWalk.Services.Services
{
    public class CacheServices : ICacheServices
    {
        public IDistributedCache cache;
        public CacheServices(IDistributedCache cache) 
        {
            this.cache = cache;
        }
        public async Task<T> GetCache<T>(string key, CancellationToken cancellationToken = default)
        {
            if(string.IsNullOrEmpty(key))
            {
                return default!;
            }
            var data=await cache.GetStringAsync(key.ToLower());
            if (data == null)
            {
                return default!;
            }
            return JsonSerializer.Deserialize<T>(data)!;
        }

        public async Task Remove<T>(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(key))
            {
                return ;
            }
            await cache.RemoveAsync(key);
        }

        public async Task SetCache<T>(string key, T value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(key)||value==null)
            {
                return ;
            }
            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };
            await cache.SetStringAsync(key.ToLower(),JsonSerializer.Serialize<T>(value),option);
        }
    }
}
