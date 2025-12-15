using WebMVC.Models;

namespace WebMVC.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IStudentService _studentService;
        
        private static List<Attendance> _attendances = new()
        {
            new Attendance
            {
                Id = 1,
                StudentId = 1,
                Date = DateTime.Today,
                Status = "Hadir",
                Note = "Tepat waktu"
            },
            new Attendance
            {
                Id = 2,
                StudentId = 2,
                Date = DateTime.Today,
                Status = "Sakit",
                Note = "Ada surat dokter"
            }
        };

        // Constructor dengan StudentService
        public AttendanceService(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public void AddAttendance(Attendance attendance)
        {
            attendance.Id = _attendances.Any()
                ? _attendances.Max(a => a.Id) + 1
                : 1;

            _attendances.Add(attendance);
        }

        public List<Attendance> GetAllAttendances()
        {
            // Join dengan Student data
            var students = _studentService.GetAllStudents();
            
            foreach (var attendance in _attendances)
            {
                attendance.Student = students.FirstOrDefault(s => s.Id == attendance.StudentId);
            }
            
            return _attendances;
        }

        public Attendance? GetAttendanceById(int id)
        {
            var attendance = _attendances.FirstOrDefault(a => a.Id == id);
            
            if (attendance != null)
            {
                // Load student data
                attendance.Student = _studentService.GetStudentById(attendance.StudentId);
            }
            
            return attendance;
        }

        public void UpdateAttendance(Attendance attendance)
        {
            var existing = _attendances.FirstOrDefault(a => a.Id == attendance.Id);
            if (existing != null)
            {
                existing.StudentId = attendance.StudentId;
                existing.Date = attendance.Date;
                existing.Status = attendance.Status;
                existing.Note = attendance.Note;
            }
        }

        public void DeleteAttendance(int id)
        {
            var attendance = _attendances.FirstOrDefault(a => a.Id == id);
            if (attendance != null)
            {
                _attendances.Remove(attendance);
            }
        }

        public List<Attendance> GetAttendancesByStudentId(int studentId)
        {
            return _attendances
                .Where(a => a.StudentId == studentId)
                .ToList();
        }

        public List<Attendance> GetAttendancesByDate(DateTime date)
        {
            return _attendances
                .Where(a => a.Date.Date == date.Date)
                .ToList();
        }
    }
}