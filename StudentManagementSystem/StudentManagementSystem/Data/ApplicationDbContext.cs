using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<FieldUser> FieldUsers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Competence> Competences { get; set; }
        public DbSet<Outcome> Outcomes { get; set; }
        public DbSet<TaskEvaluation> TaskEvaluations { get; set; }
        public DbSet<MajorAttendance> MajorAttendances { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }
        public DbSet<AbsenceReason> AbsenceReasons { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships and constraints
            modelBuilder.Entity<User>()
                .HasOne(u => u.CreatedByUser)
                .WithMany(u => u.CreatedUsers)
                .HasForeignKey(u => u.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FieldUser>()
                .HasOne(fu => fu.User)
                .WithMany(u => u.FieldUsers)
                .HasForeignKey(fu => fu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FieldUser>()
                .HasOne(fu => fu.Field)
                .WithMany(f => f.FieldUsers)
                .HasForeignKey(fu => fu.FieldId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure other relationships as needed
            base.OnModelCreating(modelBuilder);
        }
    }
}
