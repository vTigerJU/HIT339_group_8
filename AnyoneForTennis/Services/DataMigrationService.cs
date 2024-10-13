using AnyoneForTennis.Constants;
using AnyoneForTennis.Data;
using AnyoneForTennis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AnyoneForTennis.Services
{
    public class DataMigrationService : IDataMigrationService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly Hitdb1Context _hitdb1Context;
        private readonly UserManager<ApplicationUser> _userMgr;
        public DataMigrationService(ApplicationDbContext applicationDbContext, Hitdb1Context hitdb1Context, UserManager<ApplicationUser> userMgr)
        {
            _applicationDbContext = applicationDbContext;
            _hitdb1Context = hitdb1Context;
            _userMgr = userMgr;
        }
        public async Task MigrateData(UserManager<ApplicationUser> manager)
        {
         
            var coaches = await _hitdb1Context.Coaches.ToListAsync();
            var members = await _hitdb1Context.Members.ToListAsync();
            var schedules = await _hitdb1Context.Schedules.ToListAsync();
            var newSchedules = schedules.Select(old => new NewSchedule
            {
                Name = old.Name,
                Location = old.Location,
                Description = old.Description,
                Date = new DateOnly(2024, 10, 30)
            }).ToList();
            //Inserts schedules from given database into our own database context
            await _applicationDbContext.Schedules.AddRangeAsync(newSchedules);
            await _applicationDbContext.SaveChangesAsync();
            char[] charTrim = {' ', '\''};
            //Transforms coach into applicationUser with Coach Role
            foreach (var coach in coaches)
            {
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = coach.FirstName.Trim(charTrim),
                    Email = "coach@aft.com",
                    EmailConfirmed = true,
                    Firstname = coach.FirstName.Trim(charTrim),
                    Lastname = coach.LastName.Trim(charTrim),
                    Photo = coach.Photo,
                    Biography = coach.Biography,
                    Schedules = new List<NewSchedule>()

                };
                var result =  await _userMgr.CreateAsync(user, "User@123");                
                if (result.Succeeded) 
                {
                    await manager.AddToRoleAsync(user, Roles.Coach.ToString());
                }
                else
                {
                    // Handle errors
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
            //Transforms members into applicationUser with user role
            foreach (var member in members) 
            {
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = member.Email.Trim(charTrim),
                    Email = member.Email.Trim(charTrim),
                    EmailConfirmed = true,
                    Firstname = member.FirstName.Trim(charTrim),
                    Lastname = member.LastName.Trim(charTrim),
                    Active = member.Active,
                    Schedules = new List<NewSchedule>()
                };
                var result = await _userMgr.CreateAsync(user, "User@123");
                if (result.Succeeded)
                {
                    await _userMgr.AddToRoleAsync(user, Roles.User.ToString());
                }
                else
                {
                    // Handle errors
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }

        }
    }
}