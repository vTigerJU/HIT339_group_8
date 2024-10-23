using Microsoft.AspNetCore.Identity;
using AnyoneForTennis.Constants;
using System;
using AnyoneForTennis.Models;
using Microsoft.EntityFrameworkCore;
using AnyoneForTennis.Services;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace AnyoneForTennis.Data
{
    public class DbSedder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            var context = service.GetRequiredService<ApplicationDbContext>();
            var migrationService = service.GetService<DataMigrationService>();

            //adding some roles to db
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.Coach.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

          

            // create admin user

            var admin = new ApplicationUser
            {
                Firstname = "Admin",
                Lastname = "Adminsson",
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Schedules = new List<NewSchedule>()
            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
            // coach
            var coach = new ApplicationUser
            {
                Firstname = "Coach",
                Lastname = "Coachsson",
                UserName = "coach@gmail.com",
                Email = "coach@gmail.com",
                EmailConfirmed = true,
                Schedules = new List<NewSchedule>()
            };

            userInDb = await userMgr.FindByEmailAsync(coach.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(coach, "Coach@123");
                await userMgr.AddToRoleAsync(coach, Roles.Coach.ToString());
            }

            var user = new ApplicationUser
            {
                Firstname = "Jon",
                Lastname = "Doe",
                UserName = "user@gmail.com",
                Email = "user@gmail.com",
                EmailConfirmed = true,
                Schedules = new List<NewSchedule>()

            };

            userInDb = await userMgr.FindByEmailAsync(user.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(user, "User@123");
                await userMgr.AddToRoleAsync(user, Roles.User.ToString());
            }
            context.SaveChanges();
            //Migrate old Data
            if (!context.Schedules.Any())
            {
                await migrationService.MigrateData(userMgr);
            }
        }
    }
}
