using NZWalk.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(Expression<Func<T, bool>> filter, string? IncludeProperities = null);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> ?filter=null,string?IncludeProperities=null, string? order = null, bool? IsDescending = false);
        Task Add(T entity);
        Task Update(T entity);
        Task Remove (T entity);
        Task RemoveRange(IEnumerable<T> entity);
    }
}
