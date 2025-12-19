using System;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    /// <summary>
    /// Model Attendance untuk absensi student di course
    /// </summary>
    public class Attendance
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Tanggal")]
        public DateTime Date { get; set; } = DateTime.Now;
        
        [Required(ErrorMessage = "Status kehadiran harus dipilih")]
        [Display(Name = "Status Kehadiran")]
        public string Status { get; set; } = string.Empty; // Hadir, Izin, Sakit, Alpha
        
        [Display(Name = "Keterangan")]
        [StringLength(500, ErrorMessage = "Keterangan maksimal 500 karakter")]
        public string? Note { get; set; }
        
        // Foreign Keys
        [Required(ErrorMessage = "Student harus dipilih")]
        [Display(Name = "Student")]
        public int StudentId { get; set; }
        
        [Required(ErrorMessage = "Course harus dipilih")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }
        
        // Navigation Properties
        public Student? Student { get; set; }
        public Course? Course { get; set; }
    }
}
















