using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IStudentService
    {
        //METHOD LAMA (Synchronous) - Tetap ada biar gak error
        List<Student> GetAllStudents();
        Student? GetStudentById(int id);
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
        
        // METHOD BARU (Asynchronous) - Untuk Enrollment
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<bool> CreateStudentAsync(Student student);
        Task<bool> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
    }
}