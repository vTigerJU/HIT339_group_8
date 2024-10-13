using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnyoneForTennis.Data;
using AnyoneForTennis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace AnyoneForTennis.Controllers
{
    public class CoachScheduleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CoachScheduleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: CoachSchedule
        [Authorize(Roles = "Coach, Admin")]
        public async Task<IActionResult> Index()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Today);
            var schedules = new List<NewSchedule>();
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var applicationDbContext = _context.Schedules.Include(n => n.Coach).Where(n => n.CoachId == userId && n.Date > currentDate);
                schedules = await applicationDbContext.ToListAsync();
            }
            return View(schedules);
        }

        // GET: CoachSchedule/Details/5
        [Authorize(Roles = "Coach, Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newSchedule = await _context.Schedules
                .Include(n => n.Coach)
                .Include(n => n.Members)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (newSchedule == null)
            {
                return NotFound();
            }

            return View(newSchedule);
        }
    }
}
