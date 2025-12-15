using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Participant harus dipilih")]
        [Display(Name = "Participant")]
        public int StudentId { get; set; }
        
        [Required(ErrorMessage = "Date harus diisi")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [Required(ErrorMessage = "Status harus dipilih")]
        [Display(Name = "Status")]
        public string Status { get; set; } = string.Empty;
        
        [Display(Name = "Note")]
        [StringLength(200)]
        public string? Note { get; set; }
        
        // Navigation property
        public Student? Student { get; set; }
    }
}