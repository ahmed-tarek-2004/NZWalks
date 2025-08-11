using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Model.Domin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.Repository
{
    public class RegionRepository:Repository<Region>,IRegionRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<RegionRepository> logger;
        public RegionRepository (ApplicationDBContext context,ILogger<RegionRepository>logger):base(context) 
        {
            _context=context;
            this.logger = logger;
        }

        public new async Task<IQueryable<Region>> GetAll(Expression<Func<Region, bool>>? filter = null, string? order = null, bool? IsDescending = false)
        {
            var query=await base.GetAll(filter);
            var allowedSortFields = new List<String>() { "Name", "Code", "RegionImageUrl" };
            if (!string.IsNullOrEmpty(order) && allowedSortFields.Contains(order, StringComparer.OrdinalIgnoreCase))
            {
                var orderProperty = allowedSortFields.FirstOrDefault(b => b.Equals(order,StringComparison.OrdinalIgnoreCase));

                query = IsDescending == true ? query.OrderByDescending(x => EF.Property<Region>(x, orderProperty))
                    : query.OrderBy(x => EF.Property<Region>(x, orderProperty));
            }
            var list = await query.FirstOrDefaultAsync(b=>b.Name!=null);

            logger.LogCritical($"Name = {list.Name}");
            return query;
        }
    }
}
