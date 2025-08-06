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
    public interface IWalkServices
    {
        Task<WalkDto> Get(Guid id);
        Task<IEnumerable<WalkDto>> GetALL(Expression<Func<Walk, bool>>? filter = null);
        Task<Walk> Add(AddWalkDto Walk);
        Task<Walk> Update(Guid id, UpdateWalkDto Dto);
        Task<Walk> Delete(Guid id);
    }
}
