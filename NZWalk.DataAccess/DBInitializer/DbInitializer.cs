using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NZWalk.DataAccess.Data;
using NZWalk.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.DBInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthorizationDBContext context;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AuthorizationDBContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            this.context = context;
        }

        public async Task Initialize()
        {
            //migrations if they are not applied
            try
            {
                if(context.Database.GetPendingMigrations().Count()>0)
                {
                    context.Database.Migrate();
                }
            }
            catch (Exception ex) { }

            //create roles if they are not created
            if (!await _roleManager.RoleExistsAsync(SD.readerRoleId))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.writerRoleId));
                await _roleManager.CreateAsync(new IdentityRole(SD.readerRoleId));

                IdentityUser user = new IdentityUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "112233445566"
                };
                await _userManager.CreateAsync(user, "Admin123*");
                await _userManager.AddToRoleAsync(user, SD.writerRoleId);
            }

            return;
        }
    }
}
