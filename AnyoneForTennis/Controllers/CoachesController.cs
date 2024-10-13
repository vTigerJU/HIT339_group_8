using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnyoneForTennis.Data;
using AnyoneForTennis.Models;
using AnyoneForTennis.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace AnyoneForTennis.Controllers
{
    public class CoachesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoachesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Coaches
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.GetUsersInRoleAsync(Roles.Coach.ToString()));
        }

        // GET: Coaches/Details/5
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }
    }
}
