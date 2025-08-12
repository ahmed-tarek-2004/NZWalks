using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.Services.IServices
{
    public interface ICacheServices
    {
        Task SetCache<T>(string key,T value,CancellationToken cancellationToken=default);
        Task<T> GetCache<T>(string key,CancellationToken cancellationToken=default);
        Task Remove<T>(string key,CancellationToken cancellationToken=default);
    }
}
