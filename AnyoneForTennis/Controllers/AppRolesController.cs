using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AnyoneForTennis.Models.ViewModels;
using AnyoneForTennis.Models;
using AnyoneForTennis.Constants;

namespace AnyoneForTennis.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AppRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppRolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // List all roles created by users
        [Authorize(Roles = "Admin" )]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            if (!await _roleManager.RoleExistsAsync(model.Name))
            {
                await _roleManager.CreateAsync(new IdentityRole(model.Name));
            }
            return RedirectToAction("Index");
        }

        // List all users and their roles
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUserRoles()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = roles
                });
            }

            return View(userRoles);
        }

        // GET: Edit roles for a specific user
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var model = new EditUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList(),
                UserRoles = await _userManager.GetRolesAsync(user)
            };

            return View(model);
        }

        // POST: Update roles for a user
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserRoles(EditUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = model.UserRoles ?? new List<string>();

            // Add new roles
            await _userManager.AddToRolesAsync(user, selectedRoles.Except(currentRoles));

            // Remove old roles
            await _userManager.RemoveFromRolesAsync(user, currentRoles.Except(selectedRoles));

            return RedirectToAction("ManageUserRoles");
        }
    }
}
