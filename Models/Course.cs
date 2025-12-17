using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course code is required")]
        [StringLength(20)]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        [StringLength(100)]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [Required]
        [Range(1, 6, ErrorMessage = "Credits must be between 1 and 6")]
        public int Credits { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Instructor name is required")]
        [StringLength(100)]
        public string Instructor { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}