using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.Model.Domin
{
    public class ImageFile
    {
        public Guid Id { get; set; }
        [NotMapped]
        public IFormFile File {  get; set; }
        public string ?Description {  get; set; }
        public string FilePath {  get; set; }
        public string FileName {  get; set; }
        public string FileExtension {  get; set; }
        [MaxLength(10485760)]
        public long FileSize {  get; set; }

    }
}
