using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.IServices;
using System.Linq.Expressions;

namespace NZWalk.Services.Services
{
    public class RegionServices : IRegionServices
    {
        private readonly IUnitOfWork unitOfWork;
        public RegionServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Add(AddRegionRequestDto region)
        {
            return true;
        }
        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
        public Task<Region> Get(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Region>> GetALL(Expression<Func<Region, bool>>? filter=null)
        {
            var regions = await unitOfWork.region.GetAll();
            return regions;
        }
        public async Task<bool> Update(Guid id)
        {
            var reion = unitOfWork.region.Get(r => r.Id == id);
            if (reion != null)
            {
                return true;
            }
            return false;
        }
    }
}
