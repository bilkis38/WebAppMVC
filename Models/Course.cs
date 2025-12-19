using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    /// <summary>
    /// Model Course untuk sistem Course (Sekolah/Kursus)
    /// </summary>
    public class Course
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Nama course harus diisi")]
        [StringLength(100, ErrorMessage = "Nama course maksimal 100 karakter")]
        [Display(Name = "Nama Course")]
        public string CourseName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Kode course harus diisi")]
        [StringLength(20, ErrorMessage = "Kode course maksimal 20 karakter")]
        [Display(Name = "Kode Course")]
        public string CourseCode { get; set; } = string.Empty;
        
        [Display(Name = "Deskripsi")]
        [StringLength(500, ErrorMessage = "Deskripsi maksimal 500 karakter")]
        public string? Description { get; set; }
        
        [Display(Name = "Instruktur/Pengajar")]
        [StringLength(100, ErrorMessage = "Nama instruktur maksimal 100 karakter")]
        public string? Instructor { get; set; }
        
        // Navigation Property - Course punya banyak Students (OLD - One-to-Many)
        public ICollection<Student> Students { get; set; } = new List<Student>();
        
        // property untuk Attendance
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        
        // Navigation property untuk Many-to-Many
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}