using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.Model.DTOs
{
    public class RegionDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? RegionImageUrl { get; set; }
    }
}
