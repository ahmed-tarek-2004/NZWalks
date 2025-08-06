using NZWalk.DataAccess.Model.Domin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.Model.DTOs
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name Is Required"), MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Description Is Required"), MaxLength(1000)]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "KM Length Is Required"), Range(0, 500)]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        public Guid? DifficultyId { get; set; }
        public Guid? RegionId { get; set; }

        public Difficulty Difficulty { get; set; }
        //[JsonIgnore]
        public Region Region { get; set; }

    }
}
