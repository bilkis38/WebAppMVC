using System;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Tanggal Daftar")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        
        [Display(Name = "Status")]
        [StringLength(50)]
        public string Status { get; set; } = "Active";
        
        [Display(Name = "Nilai")]
        [Range(0, 100)]
        public decimal? Grade { get; set; }
        
        // Foreign Keys
        [Required]
        public int StudentId { get; set; }
        
        [Required]
        public int CourseId { get; set; }
        
        // Navigation Properties
        public Student? Student { get; set; }
        public Course? Course { get; set; }
    }
}