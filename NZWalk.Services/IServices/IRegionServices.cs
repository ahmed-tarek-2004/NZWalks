using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.Services.IServices
{
    public interface IRegionServices
    {
        Task<Region> Get(Guid id);
        Task<IEnumerable<Region>> GetALL(Expression<Func<Region,bool>>?filter);
        Task <bool> Add(AddRegionRequestDto region);
        Task <bool> Update(Guid id);
        Task <bool> Delete(Guid id);
    }
}
