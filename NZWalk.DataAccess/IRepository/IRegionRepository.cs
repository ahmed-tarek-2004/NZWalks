using Microsoft.EntityFrameworkCore;
using NZWalk.DataAccess.Model.Domin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.IRepository
{
    public interface IRegionRepository:IRepository<Region>
    {
        public Task<IQueryable<Region>> GetAll(Expression<Func<Region, bool>>? filter = null, string? order = null, bool? IsDescending = false);
    }
}
