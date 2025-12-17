using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Display(Name = "Enrollment Date")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        [StringLength(2)]
        public string? Grade { get; set; }  // ⬅️ PENTING: pakai ?

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}