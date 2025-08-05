using AutoMapper;
using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.IServices;
using NZWalk.Services.Mapping;
using System.Linq.Expressions;

namespace NZWalk.Services.Services
{
    public class RegionServices : IRegionServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper map;
        public RegionServices(IUnitOfWork unitOfWork, IMapper map)
        {
            this.unitOfWork = unitOfWork;
            this.map = map;
        }
        public async Task<Region> Add(AddRegionRequestDto regionDto)
        {
            if (regionDto == null)
            {
                return null;
            }
            var region = map.Map<Region>(regionDto);
            await unitOfWork.region.Add(region);
            await unitOfWork.SaveChanges();
            return region;
        }
        public async Task<Region> Delete(Guid id)
        {
            var region = await unitOfWork.region.Get(p => p.Id == id);
            if (region == null)
            {
                return null;
            }
            var returned = region;
            await unitOfWork.region.Remove(region);
            return returned;
             
        }
        public async Task<RegionDTO> Get(Guid id)
        {
            var region = await unitOfWork.region.Get(p => p.Id == id);
            if (region is null) return null;
            var regionDto = map.Map<RegionDTO>(region);
            return regionDto;
        }
        public async Task<IEnumerable<RegionDTO>> GetALL(Expression<Func<Region, bool>>? filter = null)
        {
            var regions = await unitOfWork.region.GetAll(filter);
            return map.Map<IEnumerable<RegionDTO>>(regions);
        }
        public async Task<Region> Update(Guid id, UpdateRegionRequestDto regionDto)
        {
            var region = await unitOfWork.region.Get(r => r.Id == id);
            if (region != null)
            {
                region = map.Map<Region>(regionDto);
                await unitOfWork.SaveChanges();
                return region;
            }
            return null;
        }
    }
}
