using Microsoft.EntityFrameworkCore;
using AcademicRecordsApp.Data.Models;

namespace AcademicRecordsApp.Models
{
    public partial class AcademicRecordsDBContext : DbContext
    {
        public AcademicRecordsDBContext()
        {
        }

        public AcademicRecordsDBContext(DbContextOptions<AcademicRecordsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<Grade> Grades { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<StudentCourse> StudentCourses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=MARIYA;Database=AcademicRecordsDB; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exam>(entity =>
            {
                entity
                .Property(e => e.Name).HasMaxLength(100);
            });
            modelBuilder.Entity<Exam>(entity =>
            {
                entity.Property(e => e.CourseId).IsRequired(false);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.HasOne(e => e.Course)
                    .WithMany((c => c.Exams))
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity
                .Property(e => e.Value).HasColumnType("decimal(3, 2)");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Grades_Exams");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Grades_Students");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity
                .Property(e => e.FullName).HasMaxLength(100);


            });
            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(sc => new { sc.StudentId, sc.CourseId });

                entity
                .HasOne(sc => sc.Student)
                .WithMany(s => s.Courses)
                .HasForeignKey(sc => sc.StudentId);

                entity
                .HasOne(sc => sc.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(sc => sc.CourseId);

            });
            modelBuilder.Entity<Course>(entity =>
            {
                entity
                    .HasKey(c => c.Id);
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasDefaultValue(string.Empty);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity
                .Property(e => e.Name).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
