using System.ComponentModel.DataAnnotations;

namespace AcademicRecordsApp.Data.Models;

public class Course
{
    [Key]
    public int  Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;


    public ICollection<Exam> Exams { get; set; }
        = new HashSet<Exam>();
    public virtual ICollection<StudentCourse> Students { get; set; }
        = new HashSet<StudentCourse>();

}