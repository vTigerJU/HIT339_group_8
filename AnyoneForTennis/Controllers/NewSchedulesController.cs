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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AnyoneForTennis.Models.ViewModels;

namespace AnyoneForTennis.Controllers
{
    public class NewSchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NewSchedulesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: NewSchedules
        [Authorize(Roles="User, Admin")]
        public async Task<IActionResult> Index()
        {
            UserScheduleViewModel usVM = new UserScheduleViewModel();
            var schedules = await _context.Schedules.Include(n => n.Coach).ToListAsync();
            usVM.Schedules = schedules;
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var currentUsers = await _context.Users.Include(n => n.Schedules).Where(u => u.Id == userId).ToListAsync();
                usVM.User = currentUsers.First();
            }
            else
            {
                var emptyUser = new ApplicationUser();
                emptyUser.Schedules = new List<NewSchedule>();
                usVM.User = emptyUser;
            }
            return View(usVM);
        }

        [Authorize(Roles="User")]
        public async Task<IActionResult> Enrol(int? id)
        {
            var schedule = _context.Schedules.Include(s => s.Members).First(s => s.ScheduleId == id);
            var user = await _userManager.GetUserAsync(User);
            var currentDate = DateOnly.FromDateTime(DateTime.Today);
            if(user != null && currentDate < schedule.Date)
            {
                if(schedule.Members == null)
                {
                    schedule.Members = new List<ApplicationUser>();
                }
                if (schedule.Members.Contains(user))
                {
                    schedule.Members.Remove(user);
                }
                else
                {
                    schedule.Members.Add(user);
                }
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: NewSchedules/Details/5
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newSchedule = await _context.Schedules
                .Include(n => n.Coach)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (newSchedule == null)
            {
                return NotFound();
            }

            return View(newSchedule);
        }

        // GET: NewSchedules/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var users = _userManager.GetUsersInRoleAsync(Roles.Coach.ToString());
            ViewData["CoachId"] = new SelectList(users.Result, "Id", "UserName");
            return View();
        }

        // POST: NewSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleId,Name,Location,Description,Date,CoachId")] NewSchedule newSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var users = _userManager.GetUsersInRoleAsync(Roles.Coach.ToString());
            ViewData["CoachId"] = new SelectList(users.Result, "Id", "UserName");
            return View(newSchedule);
        }

        // GET: NewSchedules/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newSchedule = await _context.Schedules.FindAsync(id);
            if (newSchedule == null)
            {
                return NotFound();
            }
            var users = _userManager.GetUsersInRoleAsync(Roles.Coach.ToString());
            ViewData["CoachId"] = new SelectList(users.Result, "Id", "UserName");
            return View(newSchedule);
        }

        // POST: NewSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleId,Name,Location,Description,Date,CoachId")] NewSchedule newSchedule)
        {
            if (id != newSchedule.ScheduleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewScheduleExists(newSchedule.ScheduleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var users = _userManager.GetUsersInRoleAsync(Roles.Coach.ToString());
            ViewData["CoachId"] = new SelectList(users.Result, "Id", "UserName");
            return View(newSchedule);
        }

        // GET: NewSchedules/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newSchedule = await _context.Schedules
                .Include(n => n.Coach)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (newSchedule == null)
            {
                return NotFound();
            }

            return View(newSchedule);
        }

        // POST: NewSchedules/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newSchedule = await _context.Schedules.FindAsync(id);
            if (newSchedule != null)
            {
                _context.Schedules.Remove(newSchedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.ScheduleId == id);
        }
    }
}
