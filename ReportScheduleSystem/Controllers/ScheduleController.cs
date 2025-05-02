using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReportScheduleSystem.Models;
using System.Net.Mail;
using System.Net;
using Hangfire;

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
                    User_Id = report.User_Id,
                    ScheduledDateTime = DateTime.Now
                };

                ViewBag.ShowDropdown = false;
                ViewBag.FileName = report.FileName;

                return View(schedule);
            }

            // If no report ID is passed, show the dropdown to choose
            ViewBag.ShowDropdown = true;
            ViewBag.ReportList = new SelectList(_Context.Reports, "Id", "Name"); // You can change to show "Name" instead of "Id" if you want

            return View(new Schedule { ScheduledDateTime = DateTime.Now });
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
                // ❌ Invalid form, show again with dropdown
                var report = await _Context.Reports.FirstOrDefaultAsync(r => r.Id == schedule.ReportId);
                ViewBag.FileName = report?.FileName;
                ViewBag.ShowDropdown = report == null;

                if (ViewBag.ShowDropdown)
                {
                    ViewBag.ReportList = new SelectList(_Context.Reports, "Id", "Name");
                }

                return View(schedule);
            }
            DateTime scheduledTimeUtc = schedule.ScheduledDateTime.ToUniversalTime();
            string cron = ConvertToCronExpression(scheduledTimeUtc);


            // ✅ Valid case — Save schedule and schedule email job
            _Context.Schedules.Add(schedule);
            await _Context.SaveChangesAsync();



            //// ✅ Create cron expression from ScheduledDateTime
            //string cron = $"{schedule.ScheduledDateTime.Minute} {schedule.ScheduledDateTime.Hour} {schedule.ScheduledDateTime.Day} {schedule.ScheduledDateTime.Month} *";

            // ✅ Add recurring job with Hangfire
            RecurringJob.AddOrUpdate<ScheduleController>(
                $"job-{schedule.ScheduleId}",
                x => x.SendReport(schedule.ReportId, schedule.Email),
                cron
            );

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AutomaticRetry(Attempts = 0)]
        public async Task<IActionResult> SendReport(int reportId, string toEmail)
        {
            try
            {
                var report = await _Context.Reports.FindAsync(reportId);
                if (report == null)
                    return Content("Report not found");

                var message = new MailMessage();
                message.To.Add(toEmail);
                message.Subject = "Your Report";
                message.Body = "Attached report file";
                message.From = new MailAddress("parasvanve218@gmail.com");

                using (var stream = new MemoryStream(report.FileData))
                {
                    message.Attachments.Add(new Attachment(stream, report.FileName, report.ContentType));

                    var smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new NetworkCredential("parasvanve218@gmail.com", "ktnt suqb xwxh mfcc"); // ✅ App password here
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }

                return Content("✅ Email successfully sent!");
            }
            catch (Exception ex)
            {
                return Content("❌ Failed to send email: " + ex.Message);
            }
        }

        private static string ConvertToCronExpression(DateTime dateTime)
        {
            return $"{dateTime.Minute} {dateTime.Hour} {dateTime.Day} {dateTime.Month} *";
        }

        // ✅ EDIT - GET
        public async Task<IActionResult> Edit(int id)
        {
            var schedule = await _Context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            else
            {
                return View(schedule);
            }

        }

        // ✅ EDIT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Schedule schedule)
        {
            if (id != schedule.ScheduleId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                try
                {
                    _Context.Update(schedule);
                    await _Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_Context.Schedules.Any(e => e.ScheduleId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            //ViewBag.ReportList = new SelectList(_Context.Reports, "Id", "Id", schedule.ReportId);
            return View(schedule);
        }

        // ✅ DELETE - GET (Confirmation Page)
        public async Task<IActionResult> Delete(int? id)
        {


            //var schedule = await _Context.Schedules
            //    .Include(s => s.Report)
            //    .FirstOrDefaultAsync(m => m.ScheduleId == id);
            var schedule = await _Context.Schedules.FindAsync(id);

            if (schedule == null)
                return NotFound();

            return View(schedule);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _Context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _Context.Schedules.Remove(schedule);
                await _Context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }



}
