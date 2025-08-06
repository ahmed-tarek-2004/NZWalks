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
    public class WalkRepository : Repository<Walk>, IWalkRepository
    {
        private readonly ApplicationDBContext context;
        public WalkRepository(ApplicationDBContext context) : base(context)
        {
            this.context = context;
        }
    }
}
