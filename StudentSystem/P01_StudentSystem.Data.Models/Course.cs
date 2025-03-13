


namespace P01_StudentSystem.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [MaxLength(80)]
        [Unicode]
        [Required]
        public string Name { get; set; } = null!;

        [Unicode]       
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal Price { get; set; }
        //One course can have many Students
        //One course can have many Resources
        //One course can have many Homeworks
        public ICollection<Resource> Resources { get; set; } 
            = new HashSet<Resource>();

        public ICollection<StudentCourse> StudentsCourses { get; set; } 
            = new HashSet<StudentCourse>();

        public ICollection<Homework> Homeworks { get; set; }
        = new HashSet<Homework>();
    }
}
