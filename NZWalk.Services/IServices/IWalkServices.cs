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
        Task<IEnumerable<WalkDto>> GetALL(string? filter = null,string? order=null,bool ?IsDescending=false,int PageNum=1,int PageSize=1000);
        Task<Walk> Add(AddWalkDto Walk);
        Task<Walk> Update(Guid id, UpdateWalkDto Dto);
        Task<Walk> Delete(Guid id);
    }
}
