using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.utility;
namespace NZWalk.DataAccess.Data
{
    public class AuthorizationDBContext : IdentityDbContext<IdentityUser>
    {
        public AuthorizationDBContext(DbContextOptions<AuthorizationDBContext> option) : base(option) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}

/*

 */