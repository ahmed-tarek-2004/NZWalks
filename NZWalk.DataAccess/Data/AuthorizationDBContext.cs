using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.utility;
namespace NZWalk.DataAccess.Data
{
    public class AuthorizationDBContext : IdentityDbContext
    {
        public AuthorizationDBContext(DbContextOptions<AuthorizationDBContext> option) : base(option) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = SD.readerRoleId,
                    ConcurrencyStamp = SD.readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = SD.writerRoleId,
                    ConcurrencyStamp = SD.writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

        }

      
    }

}

/*

 */