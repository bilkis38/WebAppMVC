using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    /// <summary>
    /// </summary>
    public class Student
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Nama harus diisi")]
        [StringLength(100, ErrorMessage = "Nama maksimal 100 karakter")]
        [Display(Name = "Nama Lengkap")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email harus diisi")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        
        [Phone(ErrorMessage = "Format nomor telepon tidak valid")]
        [Display(Name = "Nomor Telepon")]
        public string? PhoneNumber { get; set; }
        
        [Display(Name = "Alamat")]
        [StringLength(500, ErrorMessage = "Alamat maksimal 500 karakter")]
        public string? Address { get; set; }
        
        // Foreign Key - Student terdaftar di 1 Course (OLD - One-to-Many)
        [Display(Name = "Course")]
        public int? CourseId { get; set; }
        
        // Navigation Property (OLD)
        public Course? Course { get; set; }
        
        // TAMBAHAN BARU - Navigation property untuk Many-to-Many
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}