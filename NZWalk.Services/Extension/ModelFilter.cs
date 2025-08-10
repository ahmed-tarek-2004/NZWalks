using NZWalk.DataAccess.Model.Domin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.Services.Extension
{
   
    public static class ModelFilter
    {

        public static Expression<Func<Region, bool>> FilterByRegionName(this string name)
        {
            Expression<Func<Region, bool>>? filter = query => query.Name.Contains(name);
            return filter;
        }
        public static Expression<Func<Walk, bool>> FilterWalkByName(this string name)
        {
            Expression<Func<Walk, bool>>? filter = query => query.Name.Contains(name);
            return filter;
        }
        public static Expression<Func<Walk, bool>> FilterWalkByDesc(this string name)
        {
            Expression<Func<Walk, bool>>? filter = query => query.Name.Contains(name);
            return filter;
        }
        public static IQueryable<Walk> Sorting(this IQueryable<Walk> walks)
        {

            return walks;
        }

    }
}
