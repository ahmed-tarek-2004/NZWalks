using AutoMapper;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.Services.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegionDTO, Region>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
            CreateMap<AddWalkDto, Walk>().ReverseMap();
            CreateMap<UpdateWalkDto, Walk>()
            .ReverseMap();
    //        CreateMap<UserDto, User>()
    //.ForMember(dest => dest.CreatedAt, opt
            CreateMap<WalkDto, Walk>().ReverseMap();
        }
    }
}
