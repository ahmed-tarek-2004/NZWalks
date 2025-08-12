using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.Services.IServices
{
    public interface IImageServices
    {
        public Task<ImageFile> Upload(ImageFileDTO file,string web,string path, CancellationToken cancellationToken = default);
    }
}
