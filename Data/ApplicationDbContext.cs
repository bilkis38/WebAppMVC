using Microsoft.EntityFrameworkCore;
using WebMVC.Models;

namespace WebMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Attendance>().ToTable("Attendance");

         modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add indexes for better query performance
            modelBuilder.Entity<Course>()
                .HasIndex(c => c.CourseCode)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique();

            // SEED DATA
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Students
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, Name = "Ahmad Ridwan", Email = "ahmad.ridwan@email.com", Age = 20 },
                new Student { Id = 2, Name = "Siti Aminah", Email = "siti.aminah@email.com", Age = 19 },
                new Student { Id = 3, Name = "Budi Santoso", Email = "budi.santoso@email.com", Age = 21 },
                new Student { Id = 4, Name = "Dewi Lestari", Email = "dewi.lestari@email.com", Age = 20 },
                new Student { Id = 5, Name = "Eko Prasetyo", Email = "eko.prasetyo@email.com", Age = 22 }
            );

             // Seed Courses
            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    CourseCode = "CS101",
                    CourseName = "Introduction to Programming",
                    Credits = 3,
                    Description = "Basic programming concepts using C#",
                    Instructor = "Dr. Agus Wijaya",
                    StartDate = new DateTime(2025, 1, 15),
                    EndDate = new DateTime(2025, 5, 15)
                },
                new Course
                {
                    Id = 2,
                    CourseCode = "CS201",
                    CourseName = "Database Management Systems",
                    Credits = 4,
                    Description = "Introduction to databases and SQL",
                    Instructor = "Prof. Rina Kusuma",
                    StartDate = new DateTime(2025, 1, 15),
                    EndDate = new DateTime(2025, 5, 15)
                },
                new Course
                {
                    Id = 3,
                    CourseCode = "CS301",
                    CourseName = "Web Development",
                    Credits = 3,
                    Description = "Modern web development with ASP.NET Core",
                    Instructor = "Dr. Hendra Gunawan",
                    StartDate = new DateTime(2025, 1, 15),
                    EndDate = new DateTime(2025, 5, 15)
                },
                new Course
                {
                    Id = 4,
                    CourseCode = "CS202",
                    CourseName = "Data Structures",
                    Credits = 4,
                    Description = "Advanced data structures and algorithms",
                    Instructor = "Dr. Agus Wijaya",
                    StartDate = new DateTime(2025, 1, 15),
                    EndDate = new DateTime(2025, 5, 15)
                },
                new Course
                {
                    Id = 5,
                    CourseCode = "CS302",
                    CourseName = "Software Engineering",
                    Credits = 3,
                    Description = "Software development lifecycle and best practices",
                    Instructor = "Prof. Rina Kusuma",
                    StartDate = new DateTime(2025, 1, 15),
                    EndDate = new DateTime(2025, 5, 15)
                }
            );

            // Seed Enrollments
            modelBuilder.Entity<Enrollment>().HasData(
                // Ahmad Ridwan enrollments
                new Enrollment { Id = 1, StudentId = 1, CourseId = 1, EnrollmentDate = new DateTime(2025, 1, 10), Grade = "A", IsActive = true },
                new Enrollment { Id = 2, StudentId = 1, CourseId = 2, EnrollmentDate = new DateTime(2025, 1, 10), Grade = "B", IsActive = true },
                new Enrollment { Id = 3, StudentId = 1, CourseId = 3, EnrollmentDate = new DateTime(2025, 1, 10), Grade = null, IsActive = true },
                
                // Siti Aminah enrollments
                new Enrollment { Id = 4, StudentId = 2, CourseId = 1, EnrollmentDate = new DateTime(2025, 1, 11), Grade = "A", IsActive = true },
                new Enrollment { Id = 5, StudentId = 2, CourseId = 4, EnrollmentDate = new DateTime(2025, 1, 11), Grade = "B", IsActive = true },
                
                // Budi Santoso enrollments
                new Enrollment { Id = 6, StudentId = 3, CourseId = 2, EnrollmentDate = new DateTime(2025, 1, 12), Grade = "C", IsActive = true },
                new Enrollment { Id = 7, StudentId = 3, CourseId = 3, EnrollmentDate = new DateTime(2025, 1, 12), Grade = null, IsActive = true },
                new Enrollment { Id = 8, StudentId = 3, CourseId = 5, EnrollmentDate = new DateTime(2025, 1, 12), Grade = "B", IsActive = true },
                
                // Dewi Lestari enrollments
                new Enrollment { Id = 9, StudentId = 4, CourseId = 1, EnrollmentDate = new DateTime(2025, 1, 13), Grade = "A", IsActive = true },
                new Enrollment { Id = 10, StudentId = 4, CourseId = 3, EnrollmentDate = new DateTime(2025, 1, 13), Grade = null, IsActive = true },
                
                // Eko Prasetyo enrollments
                new Enrollment { Id = 11, StudentId = 5, CourseId = 4, EnrollmentDate = new DateTime(2025, 1, 14), Grade = "A", IsActive = true },
                new Enrollment { Id = 12, StudentId = 5, CourseId = 5, EnrollmentDate = new DateTime(2025, 1, 14), Grade = "B", IsActive = true }
            );

            // Seed Attendances
            modelBuilder.Entity<Attendance>().HasData(
                new Attendance { Id = 1, StudentId = 1, Date = new DateTime(2025, 1, 20), Status = "Present", Note = "On time" },
                new Attendance { Id = 2, StudentId = 2, Date = new DateTime(2025, 1, 20), Status = "Present", Note = "On time" },
                new Attendance { Id = 3, StudentId = 3, Date = new DateTime(2025, 1, 20), Status = "Absent", Note = "No information" },
                new Attendance { Id = 4, StudentId = 1, Date = new DateTime(2025, 1, 21), Status = "Present", Note = "On time" },
                new Attendance { Id = 5, StudentId = 2, Date = new DateTime(2025, 1, 21), Status = "Sick", Note = "Medical certificate provided" }
            );
        }
    }
}