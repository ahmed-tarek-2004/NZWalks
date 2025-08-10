using Microsoft.EntityFrameworkCore;
using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Model;
using NZWalk.DataAccess.Model.Domin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext context;
        private readonly DbSet<T> db;
        public Repository(ApplicationDBContext context)
        {
            this.context = context;
            db = context.Set<T>();
        }
        public async Task Add(T entity)
        {
            await db.AddAsync(entity);

        }

        public async Task<T> Get(Expression<Func<T, bool>> filter, string? IncludeProperities = null)
        {
            IQueryable<T> query = db.Where(filter);

            if (!string.IsNullOrEmpty(IncludeProperities))
            {
                foreach (var include in IncludeProperities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) 
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null,string ?IncludeProperities=null, string? order = null, bool? IsDescending = false)
        {
            IQueryable<T> query = db;

            if (filter != null)
                query = query.Where(filter);
            if (!string.IsNullOrEmpty(IncludeProperities))
            {
                foreach (var include in IncludeProperities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include);
                }
            }
            var allowedSortFields = new List<String>() { "Name", "LengthInKm", "RegionId", "DifficultyId" };
            if (!string.IsNullOrEmpty(order) && allowedSortFields.Contains(order, StringComparer.OrdinalIgnoreCase))
            {
                var orderProperty = allowedSortFields.FirstOrDefault(b=>b.ToLower()==order.ToLower());
                query = IsDescending == true ? query.OrderByDescending(x => EF.Property<T>(x, orderProperty))
                    : query.OrderBy(x => EF.Property<T>(x, orderProperty));
            }
            return await query.ToListAsync();
        }


        public Task Update(T entity)
        {
            db.Update(entity);
            return Task.CompletedTask;
        }
        public Task Remove(T entity)
        {
            db.Remove(entity);
            return Task.CompletedTask;
        }

        public Task RemoveRange(IEnumerable<T> entity)
        {
            db.RemoveRange(entity);
            return Task.CompletedTask;
        }

    }
}
