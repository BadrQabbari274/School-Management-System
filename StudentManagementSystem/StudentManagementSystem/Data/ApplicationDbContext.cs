using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
namespace StudentManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<AttendanceTypes> AttendanceTypes { get; set; }
        public DbSet<Working_Year> Working_Years { get; set; }
        public DbSet<Student_Class_Section_Year> Student_Class_Section_Years { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<AbsenceReasons> AbsenceReasons { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<Competences> Competences { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<EmployeeTypes> EmployeeTypes { get; set; }
        public DbSet<Employee_Department> Employee_Departments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Grades> Grades { get; set; }
        public DbSet<Outcomes> Outcomes { get; set; }
        public DbSet<Pictures> Pictures { get; set; }
        public DbSet<RequestExits> RequestExits { get; set; }
        public DbSet<StudentAbsents> StudentAbsents { get; set; }
        public DbSet<StudentAttendances> StudentAttendances { get; set; }
        public DbSet<StudentGrades> StudentGrades { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<TaskEvaluations> TaskEvaluations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite primary key for Student_Class_Section_Year
            modelBuilder.Entity<Student_Class_Section_Year>()
                .HasKey(e => new { e.Student_Id, e.Class_Id, e.Working_Year_Id, e.Section_id });

            // Configure composite foreign key relationships for StudentAbsents
            modelBuilder.Entity<StudentAbsents>()
                .HasOne(sa => sa.StudentClassSectionYear)
                .WithMany(scsy => scsy.StudentAbsents)
                .HasForeignKey(sa => new { sa.StudentClassSectionYear_Student_Id, sa.StudentClassSectionYear_Class_Id, sa.StudentClassSectionYear_Working_Year_Id, sa.StudentClassSectionYear_Section_id });

            // Configure composite foreign key relationships for StudentAttendances
            modelBuilder.Entity<StudentAttendances>()
                .HasOne(sa => sa.StudentClassSectionYear)
                .WithMany(scsy => scsy.StudentAttendances)
                .HasForeignKey(sa => new { sa.StudentClassSectionYear_Student_Id, sa.StudentClassSectionYear_Class_Id, sa.StudentClassSectionYear_Working_Year_Id, sa.StudentClassSectionYear_Section_id });

            base.OnModelCreating(modelBuilder);
        }
    }
}
