using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IStudentService _studentService;

        public AttendancesController(IAttendanceService attendanceService, IStudentService studentService)
        {
            _attendanceService = attendanceService;
            _studentService = studentService;
        }

        // GET: Attendances
        public IActionResult Index()
        {
            var attendances = _attendanceService.GetAllAttendances();
            return View(attendances);
        }

        // GET: Attendances/Create
        public IActionResult Create()
        {
            PopulateParticipantsDropdown();
            ViewBag.StatusList = GetStatusList();
            return View();
        }

        // POST: Attendances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _attendanceService.AddAttendance(attendance);
                return RedirectToAction(nameof(Index));
            }
            PopulateParticipantsDropdown();
            ViewBag.StatusList = GetStatusList();
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public IActionResult Edit(int id)
        {
            var attendance = _attendanceService.GetAttendanceById(id);
            if (attendance == null)
            {
                return NotFound();
            }
            PopulateParticipantsDropdown();
            ViewBag.StatusList = GetStatusList();
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _attendanceService.UpdateAttendance(attendance);
                return RedirectToAction(nameof(Index));
            }
            PopulateParticipantsDropdown();
            ViewBag.StatusList = GetStatusList();
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public IActionResult Delete(int id)
        {
            var attendance = _attendanceService.GetAttendanceById(id);
            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _attendanceService.DeleteAttendance(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendances/Details/5
        public IActionResult Details(int id)
        {
            var attendance = _attendanceService.GetAttendanceById(id);
            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }

        private void PopulateParticipantsDropdown()
        {
            var students = _studentService.GetAllStudents();
            ViewBag.Participants = new SelectList(students, "Id", "Name");
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
    }
}