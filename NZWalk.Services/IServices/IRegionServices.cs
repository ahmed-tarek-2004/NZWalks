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
        Task<RegionDTO> Get(Guid id, bool ApplyCache = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<RegionDTO>> GetALL(string? filter = null, string? order = null, bool? IsDescending = false, bool ApplyCache = false, CancellationToken cancellationToken = default);
        Task<Region> Add(AddRegionRequestDto region, bool ApplyCache = false, CancellationToken cancellationToken = default);
        Task<Region> Update(Guid id, UpdateRegionRequestDto requestDto, bool ApplyCache = false, CancellationToken cancellationToken = default);
        Task<Region> Delete(Guid id, bool ApplyCache = false, CancellationToken cancellationToken = default);
    }
}
