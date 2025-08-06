﻿using AutoMapper;
using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.Services.Services
{
    public class WalkServices: IWalkServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper map;
        public WalkServices(IUnitOfWork unitOfWork, IMapper map)
        {
            this.unitOfWork = unitOfWork;
            this.map = map;

        }
        public async Task<Walk> Add(AddWalkDto WalkDto)
        {
            if (WalkDto == null)
            {
                return null;
            }
            var Walk = map.Map<Walk>(WalkDto);
            await unitOfWork.walk.Add(Walk);
            await unitOfWork.SaveChanges();
            return Walk;
        }
        public async Task<Walk> Delete(Guid id)
        {
            var Walk = await unitOfWork.walk.Get(p => p.Id == id);
            if (Walk == null)
            {
                return null;
            }
            var returned = Walk;
            await unitOfWork.walk.Remove(Walk);
            await unitOfWork.SaveChanges();
            return returned;

        }
        public async Task<WalkDto> Get(Guid id)
        {
            var Walk = await unitOfWork.walk.Get(p => p.Id == id);
            if (Walk is null) return null;
            var WalkDto = map.Map<WalkDto>(Walk);
            return WalkDto;
        }
        public async Task<IEnumerable<WalkDto>> GetALL(Expression<Func<Walk, bool>>? filter = null)
        {
            //var Walks = await unitOfWork.Repository<Walk>().GetAll(filter,IncludeProperities: "Region,Difficulty");
            
            var Walks = await unitOfWork.walk.GetAll(filter,IncludeProperities: "Region,Difficulty");
            
           
            return map.Map<IEnumerable<WalkDto>>(Walks);
        }
        public async Task<Walk> Update(Guid id, UpdateWalkDto WalkDto)
        {
            var Walk = await unitOfWork.walk.Get(r => r.Id == id,IncludeProperities:"Region,Difficulty");
            if (Walk != null)
            {
                map.Map(WalkDto,Walk);
                await unitOfWork.SaveChanges();
                return Walk;
            }
            return null;
        }
    }
}
