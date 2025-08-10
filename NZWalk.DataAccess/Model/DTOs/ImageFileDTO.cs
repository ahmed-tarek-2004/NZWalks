using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace NZWalk.DataAccess.Model.DTOs
{
    public class ImageFileDTO
    {
        [Required]
        public IFormFile File { get; set; }
        public string? Description { get; set; }
        [Required]
        public string FileName { get; set; }
    }
}
