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
    public class RegionRepository:Repository<Region>,IRegionRepository
    {
        private readonly ApplicationDBContext _context;
        public RegionRepository (ApplicationDBContext context):base(context) 
        {
            _context=context;
        }
    }
}
