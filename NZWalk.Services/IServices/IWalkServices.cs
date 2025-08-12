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
        Task<WalkDto> Get(Guid id, bool ApplyCache = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<WalkDto>> GetALL(string? filter = null,string? order=null,bool ?IsDescending=false
            ,int PageNum=1,int PageSize=1000, bool ApplyCache = false, CancellationToken cancellationToken = default);
        Task<Walk> Add(AddWalkDto Walk, bool ApplyCache = false, CancellationToken cancellationToken = default);
        Task<Walk> Update(Guid id, UpdateWalkDto Dto, bool ApplyCache = false, CancellationToken cancellationToken = default);
        Task<Walk> Delete(Guid id, bool ApplyCache = false, CancellationToken cancellationToken = default);
    }
}
