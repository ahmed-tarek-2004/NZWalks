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
        Task<RegionDTO> Get(Guid id);
        Task<IEnumerable<RegionDTO>> GetALL(string?filter=null);
        Task<Region> Add(AddRegionRequestDto region);
        Task<Region> Update(Guid id,UpdateRegionRequestDto requestDto);
        Task<Region> Delete(Guid id);
    }
}
