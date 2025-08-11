using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.IRepository
{
    public interface IWalkRepository:IRepository<Walk>
    {
        public Task<IQueryable<Walk>> GetAll(Expression<Func<Walk, bool>> ?filter=null,string?IncludeProperities=null, string? order = null, bool? IsDescending = false);
    }
}
