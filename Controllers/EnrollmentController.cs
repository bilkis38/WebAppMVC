using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public EnrollmentController(
            IEnrollmentService enrollmentService,
            IStudentService studentService,
            ICourseService courseService)
        {
            _enrollmentService = enrollmentService;
            _studentService = studentService;
            _courseService = courseService;
        }

        // GET: Enrollment
        public async Task<IActionResult> Index()
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            return View(enrollments);
        }

        // GET: Enrollment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id.Value);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollment/Create
        public async Task<IActionResult> Create(int? courseId)
        {
            var students = await _studentService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();

            if (courseId.HasValue)
            {
                // Ambil semua enrollment untuk course ini
                var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
                var enrolledStudentIds = enrollments
                    .Where(e => e.CourseId == courseId.Value)
                    .Select(e => e.StudentId)
                    .ToList();

                // Filter student yang belum enrolled
                var availableStudents = students.Where(s => !enrolledStudentIds.Contains(s.Id)).ToList();
                
                ViewBag.Students = new SelectList(availableStudents, "Id", "Name");
                
                // Filter hanya course yang dipilih
                var selectedCourse = courses.Where(c => c.Id == courseId.Value).ToList();
                ViewBag.Courses = new SelectList(selectedCourse, "Id", "CourseName", courseId.Value);
                
                // Kirim courseId ke view untuk disable dropdown course
                ViewBag.SelectedCourseId = courseId.Value;
            }
            else
            {
                // Jika tidak ada courseId (dipanggil dari menu Enrollment), tampilkan semua
                ViewBag.Students = new SelectList(students, "Id", "Name");
                ViewBag.Courses = new SelectList(courses, "Id", "CourseName");
                ViewBag.SelectedCourseId = null;
            }
            
            return View();
        }

        // POST: Enrollment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,CourseId,EnrollmentDate,Status,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                // Cek apakah student sudah terdaftar di course ini
                var isEnrolled = await _enrollmentService.IsStudentEnrolledInCourseAsync(enrollment.StudentId, enrollment.CourseId);
                
                if (isEnrolled)
                {
                    ModelState.AddModelError("", "Student sudah terdaftar di course ini!");
                    
                    var students = await _studentService.GetAllStudentsAsync();
                    var courses = await _courseService.GetAllCoursesAsync();
                    ViewBag.Students = new SelectList(students, "Id", "Name", enrollment.StudentId);
                    ViewBag.Courses = new SelectList(courses, "Id", "CourseName", enrollment.CourseId);
                    
                    return View(enrollment);
                }

                var success = await _enrollmentService.CreateEnrollmentAsync(enrollment);
                
                if (success)
                {
                    TempData["SuccessMessage"] = "Enrollment berhasil ditambahkan!";
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Gagal menambahkan enrollment.");
            }

            var allStudents = await _studentService.GetAllStudentsAsync();
            var allCourses = await _courseService.GetAllCoursesAsync();
            ViewBag.Students = new SelectList(allStudents, "Id", "Name", enrollment.StudentId);
            ViewBag.Courses = new SelectList(allCourses, "Id", "CourseName", enrollment.CourseId);
            
            return View(enrollment);
        }

        // GET: Enrollment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id.Value);
            if (enrollment == null)
            {
                return NotFound();
            }

            var students = await _studentService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();
            ViewBag.Students = new SelectList(students, "Id", "Name", enrollment.StudentId);
            ViewBag.Courses = new SelectList(courses, "Id", "CourseName", enrollment.CourseId);

            return View(enrollment);
        }

        // POST: Enrollment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,CourseId,EnrollmentDate,Status,Grade")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var success = await _enrollmentService.UpdateEnrollmentAsync(enrollment);
                
                if (success)
                {
                    TempData["SuccessMessage"] = "Enrollment berhasil diupdate!";
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Gagal mengupdate enrollment.");
            }

            var students = await _studentService.GetAllStudentsAsync();
            var courses = await _courseService.GetAllCoursesAsync();
            ViewBag.Students = new SelectList(students, "Id", "Name", enrollment.StudentId);
            ViewBag.Courses = new SelectList(courses, "Id", "CourseName", enrollment.CourseId);

            return View(enrollment);
        }

        // GET: Enrollment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id.Value);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _enrollmentService.DeleteEnrollmentAsync(id);
            
            if (success)
            {
                TempData["SuccessMessage"] = "Enrollment berhasil dihapus!";
            }
            else
            {
                TempData["ErrorMessage"] = "Gagal menghapus enrollment.";
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}