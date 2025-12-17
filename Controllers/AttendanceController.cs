using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMVC.Models;
// using WebMVC.Services;
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
            var aatendances = await _context.Attendance
                .Include(a => a.Student)
                .ToListAsync();
            return View(aatendances);
        }

        // GET: Attendances/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendance
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
        public async Task<IActionResult> Create([Bind("Id,StudentId,Date,Status,Notes")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await PopulateStudentsDropdown();
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

            var attendance = await _context.Attendance.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            await PopulateStudentsDropdown();
            ViewBag.StatusList = GetStatusList();
            return View(attendance);
        }

        // POST: Attendance/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,Date,Status,Notes")] Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
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
            await PopulateStudentsDropdown();
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

            var attendance = await _context.Attendance
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
            var attendance = await _context.Attendance.FindAsync(id);
            _context.Attendance.Remove(attendance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper Methods
        private async Task PopulateStudentsDropdown()
        {
            var students = await _context.Students.ToListAsync();
            ViewBag.Students = new SelectList(students, "Id", "Name");
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
            return _context.Attendance.Any(e => e.Id == id);
        }
    }
}