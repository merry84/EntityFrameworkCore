using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [MaxLength(100)]
        [Unicode]
        public string Name { get; set; } = null!;

        [Unicode(false)]
        public string? PhoneNumber { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public ICollection<Homework> Homeworks { get; set; }
            = new HashSet<Homework>();

        public ICollection<StudentCourse> StudentsCourses { get; set; }
            = new HashSet<StudentCourse>();

    }
}
