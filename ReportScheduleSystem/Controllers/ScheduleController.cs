using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReportScheduleSystem.Models;

namespace ReportScheduleSystem.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ReportDbContext _Context;

        public ScheduleController(ReportDbContext context)
        {
            _Context = context;
        }

        // ✅ LIST ALL SCHEDULES
        public async Task<IActionResult> Index()
        {
            var schedules = await _Context.Schedules
                .Include(s => s.Report)
                .ToListAsync();
            return View(schedules);
        }

        // ✅ GET: Create
        public IActionResult Create(int? reportId)
        {
            if (reportId.HasValue)
            {
                var report = _Context.Reports.FirstOrDefault(r => r.Id == reportId.Value);
                if (report == null)
                    return NotFound();

                var schedule = new Schedule
                {
                    ReportId = report.Id,
                    Name = report.Name,
                    Description = report.Description,
                    Is_Active = report.Is_Active,
                    User_Id = report.User_Id
                };

                ViewBag.FileName = report.FileName;
                return View(schedule);
            }

            // 🔄 No reportId, show dropdown
            ViewBag.ReportId = new SelectList(_Context.Reports, "Id", "Name");
            return View();
        }

        // ✅ POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _Context.Schedules.Add(schedule);
                await _Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdown or file info on error
            var report = _Context.Reports.FirstOrDefault(r => r.Id == schedule.ReportId);
            ViewBag.FileName = report?.FileName;

            // If dropdown needed again
            if (report == null)
            {
                ViewBag.ReportId = new SelectList(_Context.Reports, "Id", "Name");
            }

            return View(schedule);
        }
    }
}
