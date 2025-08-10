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
            //.ForMember(dest => dest.CreatedAt, opti 
            CreateMap<WalkDto, Walk>().ReverseMap();
            CreateMap<ImageFileDTO, ImageFile>()
                .ForMember(des => des.FileSize, opt => opt.MapFrom(b => b.File.Length))
                .ForMember(des => des.FileExtension, opt => opt.MapFrom(b => Path.GetExtension(b.File.FileName)));
            //.ReverseMap();
        }
    }
}
