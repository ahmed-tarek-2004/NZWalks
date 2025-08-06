using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.Model.DTOs
{
    public class RegionDTO
    {
        public Guid Id { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Length must be 2-3"), MaxLength(3, ErrorMessage = "Length must be 2-3")]
        public string Code { get; set; } = null!;
        [Required(ErrorMessage = "Name Is Required"), MaxLength(100)]
        public string Name { get; set; } = null!;
        public string? RegionImageUrl { get; set; }
    }
}
