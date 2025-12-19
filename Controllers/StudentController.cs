using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMVC.Models;
using WebMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace WebMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .Include(s => s.Course)  // 
                .ToListAsync();
            return View(students);
        }

        // GET: Student/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Course)      // ✅ Course lama (One-to-Many)
                .Include(s => s.Enrollments) // ✅ TAMBAHKAN INI (Many-to-Many)
                    .ThenInclude(e => e.Course) // ✅ Load course dari enrollment
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public async Task<IActionResult> Create(int? courseId)
        {
            // Populate dropdown untuk pilih course
            ViewBag.CourseId = new SelectList(await _context.Courses.ToListAsync(), "Id", "CourseName", courseId);

            // Jika dipanggil dari Course Detail, set CourseId default
            if (courseId.HasValue)
            {
                var student = new Student { CourseId = courseId.Value };
                return View(student);
            }

            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,PhoneNumber,Address,CourseId")] Student student)  // ✅ PERBAIKAN
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();

                // Redirect ke Course Detail jika ada CourseId
                if (student.CourseId.HasValue)
                {
                    return RedirectToAction("Detail", "Course", new { id = student.CourseId.Value });
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.CourseId = new SelectList(await _context.Courses.ToListAsync(), "Id", "CourseName", student.CourseId);
            return View(student);
        }

        // GET: Student/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            ViewBag.CourseId = new SelectList(await _context.Courses.ToListAsync(), "Id", "CourseName", student.CourseId);
            return View(student);
        }

        // POST: Student/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,PhoneNumber,Address,CourseId")] Student student)  // ✅ PERBAIKAN
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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

            ViewBag.CourseId = new SelectList(await _context.Courses.ToListAsync(), "Id", "CourseName", student.CourseId);
            return View(student);
        }

        // GET: Student/Delete/{id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Course)  // ✅ PERBAIKAN: Course (singular)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Student/RemoveFromCourse/5
        [HttpPost]
        public async Task<IActionResult> RemoveFromCourse(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var courseId = student.CourseId;
            student.CourseId = null;

            await _context.SaveChangesAsync();

            if (courseId.HasValue)
            {
                return RedirectToAction("Detail", "Course", new { id = courseId.Value });
            }

            return RedirectToAction("Index");
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}