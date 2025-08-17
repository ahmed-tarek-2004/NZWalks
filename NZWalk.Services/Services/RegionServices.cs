using AutoMapper;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.Extension;
using NZWalk.Services.IServices;
using NZWalk.Services.Mapping;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace NZWalk.Services.Services
{
    public class RegionServices : IRegionServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper map;
        private readonly ICacheServices cacheServices;
        private readonly ILogger<RegionServices> logger;
        public RegionServices(IUnitOfWork unitOfWork, IMapper map
            ,ICacheServices cacheServices,ILogger<RegionServices>logger)
        {
            this.unitOfWork = unitOfWork;
            this.map = map;
            this.cacheServices = cacheServices;
            this.logger = logger;
        }
        public async Task<Region> Add(AddRegionRequestDto regionDto,bool ApplyCache=false,CancellationToken cancellationToken = default)
        {
            if (regionDto == null)
            {
                return null;
            }
            var region = map.Map<Region>(regionDto);
            await unitOfWork.region.Add(region);
            await unitOfWork.SaveChanges();
            if(ApplyCache)
            {
            await cacheServices.SetCache<Region>($"Region-{region.Id}",region);
                logger.LogWarning("Cache Used");
            }
            return region;
        }
        
        public async Task<RegionDTO> Get(Guid id, bool ApplyCache=false,CancellationToken cancellationToken = default)
        {
            var resultFromCache = await cacheServices.GetCache<Region>($"Region-{id}");
            var region = ApplyCache&& resultFromCache!= null
                ? resultFromCache
                : await unitOfWork.region.Get(p => p.Id == id);
            if (region is null) return null;
            var regionDto = map.Map<RegionDTO>(region);
            return regionDto;
        }
        [EnableRateLimiting("SlidingWindow")]
        public async Task<IEnumerable<RegionDTO>> GetALL(string? Properity = null, string? order = null
            , bool? IsDescending = false, bool ApplyingCache = false, CancellationToken cancellationToken = default)
        {
            IEnumerable<Region> regions ;
            if(ApplyingCache)
            {
                var RegionCached = await cacheServices.GetCache<IEnumerable<Region>>("Regions");
                if(RegionCached!=null)
                {
                    regions = RegionCached;
                    logger.LogWarning("Cache Used");
                }
                else
                {
                    regions = await unitOfWork.region.GetAll(string.IsNullOrEmpty(Properity) == true ? null : Properity.FilterByRegionName(), order, IsDescending);
                    await cacheServices.SetCache("Regions", regions.ToList());
                }
            }
            else
            {
                regions = await unitOfWork.region.GetAll(string.IsNullOrEmpty(Properity) == true ? null : Properity.FilterByRegionName(), order, IsDescending);
            }
            var regionsDto = regions.ToList();
          
            return map.Map<List<RegionDTO>>(regionsDto);
        }
        public async Task<Region> Update(Guid id, UpdateRegionRequestDto regionDto
            ,bool ApplyCahce=false, CancellationToken cancellationToken = default)
        {
            
            var region = await unitOfWork.region.Get(r => r.Id == id);
           
            if (region != null)
            {
                region = map.Map(regionDto,region);
                await unitOfWork.SaveChanges();
                if (ApplyCahce)
                {
                    await cacheServices.SetCache<Region>($"Region-{id}", region);
                    await cacheServices.Remove<IEnumerable<Region>>("Regions");
                    logger.LogInformation("Cache Used");
                }
                return region;
            }
            return null;
        }
        public async Task<Region> Delete(Guid id, bool ApplyCache = false, CancellationToken cancellationToken = default)
        {
            var region = await unitOfWork.region.Get(p => p.Id == id);
            if (region == null)
            {
                return null;
            }
            var returned = region;
            await unitOfWork.region.Remove(region);
            await unitOfWork.SaveChanges();
            if (ApplyCache)
            {
                await cacheServices.Remove<IEnumerable<Region>>("Regions");
                await cacheServices.Remove<Region>($"Region-{id}");
                logger.LogInformation("Cache Used");
            }
            return returned;

        }
    }
}
