using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<FieldEmployee> FieldEmployees { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Competence> Competences { get; set; }
        public DbSet<Outcome> Outcomes { get; set; }
        public DbSet<TaskEvaluation> TaskEvaluations { get; set; }
        public DbSet<StudentAbsent> StudentAbsents { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }
        public DbSet<AbsenceReason> AbsenceReasons { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        public DbSet<StudentGrade> StudentGrades { get; set; }
        public DbSet<AttendanceType> AttendanceTypes { get; set; }
        public DbSet<RequestExit> RequestExits { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User self-referencing relationship
            modelBuilder.Entity<Employee>()
                .HasOne(u => u.CreatedByUser)
                .WithMany(u => u.CreatedUsers)
                .HasForeignKey(u => u.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Role -> User relationship (CreatedBy)
            modelBuilder.Entity<EmployeeType>()
                .HasOne(r => r.CreatedByUser)
                .WithMany() // No navigation property on User for created roles
                .HasForeignKey(r => r.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure User -> Role relationship
            modelBuilder.Entity<Employee>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure FieldUser relationships
            modelBuilder.Entity<FieldEmployee>()
                .HasOne(fu => fu.User)
                .WithMany(u => u.FieldEmployees)
                .HasForeignKey(fu => fu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FieldEmployee>()
                .HasOne(fu => fu.Field)
                .WithMany(f => f.FieldEmployees)
                .HasForeignKey(fu => fu.FieldId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure other relationships as needed
            base.OnModelCreating(modelBuilder);
        }
    }
}