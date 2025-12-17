using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMVC.Models
{
    public class Student
    {
        [Key] //menandakan ini adalah primary key
         public int Id { get; set; }
        [Required(ErrorMessage = "Nama harus diisi.")]
        [StringLength(100, ErrorMessage = "Nama tidak boleh lebih dari 100 karakter.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email harus diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        public string Email { get; set; }
        [Range(18, 60, ErrorMessage = "Usia harus antara 18 dan 60.")]

        public int Age { get; set; }

        // Navigation property - One Student has Many Enrollments
        public ICollection<Enrollment>? Enrollments { get; set; }    

        // Navigation property - One Student has Many Attendances
        public ICollection<Attendance>? Attendances { get; set; }
    }
}