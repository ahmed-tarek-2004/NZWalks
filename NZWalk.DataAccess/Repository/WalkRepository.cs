using Microsoft.EntityFrameworkCore;
using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Model.Domin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NZWalk.DataAccess.Repository
{
    public class WalkRepository : Repository<Walk>, IWalkRepository
    {
        private readonly ApplicationDBContext context;
        public WalkRepository(ApplicationDBContext context) : base(context)
        {
            this.context = context;
        }

        public new async Task<IQueryable<Walk>> GetAll(Expression<Func<Walk, bool>>? filter = null, string? IncludeProperities = null, string? order = null, bool? IsDescending = false)
        {
            var query = await base.GetAll( filter ,IncludeProperities);
            var allowedSortFields = new List<String>() { "Name", "LengthInKm", "RegionId", "DifficultyId" };
            if (!string.IsNullOrEmpty(order) && allowedSortFields.Contains(order, StringComparer.OrdinalIgnoreCase))
            {
                var orderProperty = allowedSortFields.FirstOrDefault(b => b.Equals(order,StringComparison.OrdinalIgnoreCase));

                query = IsDescending == true ? query.OrderByDescending(x => EF.Property<Walk>(x, orderProperty))
                    : query.OrderBy(x => EF.Property<Walk>(x, orderProperty));
            }
            return  query;
        }
    }
}
