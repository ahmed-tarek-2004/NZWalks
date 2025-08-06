using NZWalk.DataAccess.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.IRepository
{
    public interface IUnitOfWork
    {
       // public IRepository<T>Repository<T>() where T : class;
        Task SaveChanges();
        public IRegionRepository region { get; }
       // public ConcurrentDictionary<Type, Object> dic { get; }
        public IWalkRepository walk { get; }
    }
}
