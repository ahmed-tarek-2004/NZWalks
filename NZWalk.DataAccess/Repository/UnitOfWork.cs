using Microsoft.Extensions.Logging;
using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Model.Domin;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDBContext _context;
        public IRegionRepository region { get; private set; }
        public IWalkRepository walk { get; private set; }
        private readonly ILogger<RegionRepository> log;

        private readonly ConcurrentDictionary<Type, object> dic;

        public UnitOfWork(ApplicationDBContext context,ILogger<RegionRepository>log) 
        {
            _context = context;
            this.log=log;
            region = new RegionRepository(_context,log);
            walk = new WalkRepository(_context);
            dic = new();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        //public async Task<IRepository<T>> GetRepository<T>() where T : class
        //{
        //    return (IRepository<T>)dic.GetOrAdd(typeof(T), _ =>
        //    {
        //        return new Repository<T>(_context);
        //    });
        //}
    }
}
