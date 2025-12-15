using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IAttendanceService
    {
        List<Attendance> GetAllAttendances();
        Attendance? GetAttendanceById(int id);
        void AddAttendance(Attendance attendance);
        void UpdateAttendance(Attendance attendance);
        void DeleteAttendance(int id);
        List<Attendance> GetAttendancesByStudentId(int studentId);
        List<Attendance> GetAttendancesByDate(DateTime date);
    }
}