using Microsoft.AspNetCore.Identity;
using AnyoneForTennis.Constants;
using System;

using AnyoneForTennis.Models;
using Microsoft.EntityFrameworkCore;

namespace AnyoneForTennis.Data
{
    public class DbSedder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            var context = service.GetRequiredService<ApplicationDbContext>();

            //adding some roles to db
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.Coach.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));


            // create admin user

            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
            // coach
            var coach = new IdentityUser
            {
                UserName = "coach@gmail.com",
                Email = "coach@gmail.com",
                EmailConfirmed = true
            };

            userInDb = await userMgr.FindByEmailAsync(coach.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(coach, "Admin@123");
                await userMgr.AddToRoleAsync(coach, Roles.Coach.ToString());
            }

            var user = new IdentityUser
            {
                UserName = "user@gmail.com",
                Email = "user@gmail.com",
                EmailConfirmed = true
            };

            userInDb = await userMgr.FindByEmailAsync(user.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(user, "User@123");
                await userMgr.AddToRoleAsync(user, Roles.User.ToString());
            }
        }
    }
}
