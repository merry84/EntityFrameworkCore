using System.ComponentModel.DataAnnotations;

namespace AcademicRecordsApp.Data.Models
{
    public  class Student
    {
        public Student()
        {
            Grades = new HashSet<Grade>();
        }
        [Key]
        public int Id { get; set; }

        [Required]

        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<StudentCourse> Courses { get; set; } = new HashSet<StudentCourse>();
    }
}
