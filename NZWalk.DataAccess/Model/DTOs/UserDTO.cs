using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.Model.DTOs
{
    public class UserDTO
    {
        
        public List<string>roles { get; set; }
       
        public IdentityUser user { get; set; }
    }
}
