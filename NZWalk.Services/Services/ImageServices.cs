using AutoMapper;
using Azure.Core;
//using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;
using NZWalk.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NZWalk.Services.Services
{
    public class ImageServices : IImageServices
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;
      

        public ImageServices(ApplicationDBContext context, IMapper map)
        {
            this.context = context;
            this.mapper = map;
        }


        public async Task<ImageFile> Upload(ImageFileDTO request,string web,string path)
        {
            var Image = mapper.Map<ImageFile>(request);
            var localFile = Path.Combine(web, "Images", $"{Image.FileName}{ Image.FileExtension}");

            using(var stream = new FileStream(localFile,FileMode.Create))
            {
                await Image.File.CopyToAsync(stream);
            }

            Image.FilePath = Path.Combine(path, "Images",$"{Image.FileName}{ Image.FileExtension}");
            await context.Images.AddAsync(Image);
            await context.SaveChangesAsync();

            return Image;
        }
    }
}
