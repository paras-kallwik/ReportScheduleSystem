using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportScheduleSystem.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

namespace ReportScheduleSystem.Controllers
{
    public class ReportController : Controller
    {
        private readonly ReportDbContext _context;

        public ReportController(ReportDbContext context)
        {
            _context = context;// ?? throw new ArgumentNullException(nameof(context)); // Ensure _context is not null
        }

        // ✅ List Reports
        public async Task<IActionResult> Index()
        {
            var data = await _context.Reports
                .Select(r => new Reports
                {
                    Id = r.Id,
                    User_Id = r.User_Id,
                    Name = r.Name,
                    Description = r.Description,
                    Is_Active = r.Is_Active,
                    FileName = r.FileName,
                    Created_At = r.Created_At,
                    Updated_At = r.Updated_At
                }).ToListAsync();

            return View(data);
        }

        // ✅ Show Create Form
        public IActionResult Create()
        {
            return View();
        }

        // ✅ Handle Create Form Submission with File Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reports reports, IFormFile file)
        {
            {
                ModelState.AddModelError("File", "File is required.");
            if (file == null || file.Length == 0)
                return View(reports);
            }

            if (file.Length > 5 * 1024 * 1024) // 5MB limit
            {
                ModelState.AddModelError("File", "File size must be less than 5MB.");
                return View(reports);
            }

            var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx", ".png", ".jpg", ".jpeg" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("File", "Invalid file type. Allowed types: PDF, DOCX, XLSX, PNG, JPG.,.jpeg");
                return View(reports);
            }

            // Process and save the file
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            reports.FileName = file.FileName;
            reports.ContentType = file.ContentType;
            reports.FileData = memoryStream.ToArray();

            // Set timestamps
            reports.Created_At = DateTime.Now;
            reports.Updated_At = DateTime.Now;

            if (ModelState.IsValid)
            {
                return View(reports);
            }

            _context.Reports.Add(reports);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Report created successfully!";
            return RedirectToAction("Index");
        }


        // ✅ Edit Report (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        // ✅ Edit Report (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reports reports, IFormFile file)
        {
            if (id != reports.Id)
            {
                return NotFound();
            }

            var existingReport = await _context.Reports.FindAsync(id);
            if (existingReport == null)
            {
                return NotFound();
            }

            // Update report details
            existingReport.User_Id = reports.User_Id;
            existingReport.Name = reports.Name;
            existingReport.Description = reports.Description;
            existingReport.Is_Active = reports.Is_Active;
            existingReport.Updated_At = DateTime.Now;

            // Handle file upload if a new file is uploaded
            if (file != null && file.Length > 0)
            {
                if (file.Length > 5 * 1024 * 1024) // 5MB limit
                {
                    ModelState.AddModelError("File", "File size must be less than 5MB.");
                    return View(reports);
                }

                var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx", ".png", ".jpg" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("File", "Invalid file type. Allowed types: PDF, DOCX, XLSX, PNG, JPG.");
                    return View(reports);
                }

                // Save new file data
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                existingReport.FileName = file.FileName;
                existingReport.ContentType = file.ContentType;
                existingReport.FileData = memoryStream.ToArray();
            }

            // Save changes
            _context.Reports.Update(existingReport);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // ✅ View Report Details
        public async Task<IActionResult> Details(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        // ✅ Show Delete Confirmation
        public async Task<IActionResult> Delete(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        // ✅ Handle Delete Confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

       //  ✅ File Download
        public async Task<IActionResult> DownloadFile(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null || report.FileData == null)
            {
                return NotFound();
            }

            return File(report.FileData, report.ContentType, report.FileName);
        }
    }
}
