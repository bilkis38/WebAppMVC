using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMVC.Models;
using WebMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace WebMVC.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Attendances
        public async Task<IActionResult> Index()
        {
            var attendances = await _context.Attendances
                .Include(a => a.Student)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
            return View(attendances);
        }

        // GET: Attendances/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendance/Create
        public async Task<IActionResult> Create()
        {
            await PopulateStudentsDropdown();
            ViewBag.StatusList = GetStatusList();
            return View();
        }

        // POST: Attendance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,Date,Status,Note")] Attendance attendance)
        {
            // Validasi tanggal tidak boleh masa lampau
            if (attendance.Date.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("Date", "Tanggal tidak boleh di masa lampau");
            }

            if (ModelState.IsValid)
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateStudentsDropdown(attendance.StudentId);  // ← TAMBAHIN parameter
            ViewBag.StatusList = GetStatusList();
            return View(attendance);
        }

        // GET: Attendance/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            
            await PopulateStudentsDropdown(attendance.StudentId);  // ← Pass StudentId
            ViewBag.StatusList = GetStatusList();
            return View(attendance);
        }

        // POST: Attendance/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,Date,Status,Note")] Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }

            // Validasi tanggal tidak boleh masa lampau
            if (attendance.Date.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("Date", "Tanggal tidak boleh di masa lampau");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.Id))
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

            await PopulateStudentsDropdown(attendance.StudentId);  // ← DIBENERIN: tambahin parameter
            ViewBag.StatusList = GetStatusList();
            return View(attendance);
        }

        // GET: Attendance/Delete/{id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }

        // POST: Attendance/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Helper Methods
        private async Task PopulateStudentsDropdown(int? selectedStudentId = null)
        {
            var students = await _context.Students.ToListAsync();
            ViewBag.Students = new SelectList(students, "Id", "Name", selectedStudentId);
        }

        private List<SelectListItem> GetStatusList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Present", Text = "Present" },
                new SelectListItem { Value = "Absent", Text = "Absent" },
                new SelectListItem { Value = "Sick", Text = "Sick" },
                new SelectListItem { Value = "Permission", Text = "Permission" }
            };
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.Id == id);
        }
    }
}