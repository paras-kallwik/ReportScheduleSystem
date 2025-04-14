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

        [HttpGet]
        [HttpGet]
        public IActionResult GetReportDetails(int id)
        {
            var report = _Context.Reports
                .FirstOrDefault(r => r.Id == id);

            if (report == null)
                return NotFound();

            return Json(new
            {
                User_Id = report.User_Id,
                name = report.Name,
                description = report.Description,
                isActive = report.Is_Active,
                fileName = report.FileName
            });
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

                ViewBag.ShowDropdown = false;
                ViewBag.FileName = report.FileName;
                return View(schedule);
            }

            ViewBag.ShowDropdown = true;
            ViewBag.ReportList = new SelectList(_Context.Reports, "Id", "Id"); // Show only ID

            return View(new Schedule());
        }
        // ✅ POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Schedule schedule)
        
        {
            if (schedule.ReportId == 0)
            {
                ModelState.AddModelError("ReportId", "Please select a valid Report.");
            }

            if (ModelState.IsValid)
            {
                _Context.Schedules.Add(schedule);
                await _Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Either model is invalid or ReportId is missing
            var report = await _Context.Reports.FirstOrDefaultAsync(r => r.Id == schedule.ReportId);

            ViewBag.FileName = report?.FileName;
            ViewBag.ShowDropdown = report == null;

            if (ViewBag.ShowDropdown)
            {
                ViewBag.ReportList = new SelectList(_Context.Reports, "Id", "Name");
            }

            return View(schedule);
        }


    }
}
