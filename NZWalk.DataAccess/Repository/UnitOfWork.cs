using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Model.Domin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDBContext _context;
   
        public IRegionRepository region{ get; private set; }

        public UnitOfWork(ApplicationDBContext context) 
        {
            _context = context;
            region = new RegionRepository(_context);
        }


        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
