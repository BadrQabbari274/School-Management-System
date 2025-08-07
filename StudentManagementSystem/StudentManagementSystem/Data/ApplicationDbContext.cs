using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Students> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Grades> Grades { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<Working_Year> Working_Years { get; set; }
        public DbSet<AttendanceTypes> AttendanceTypes { get; set; }
        public DbSet<Student_Class_Section_Year> Student_Class_Section_Years { get; set; }
        public DbSet<AbsenceReasons> AbsenceReasons { get; set; }
        public DbSet<Competencies> Competencies { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<EmployeeTypes> EmployeeTypes { get; set; }
        public DbSet<Employee_Department> Employee_Departments { get; set; }
        public DbSet<Learning_Outcome> Outcomes { get; set; }
     
        public DbSet<RequestExits> RequestExits { get; set; }
        public DbSet<Try> Try { get; set; }
        public DbSet<Evidence> Evidences { get; set; }
        public DbSet<StudentAbsents> StudentAbsents { get; set; }
        public DbSet<StudentAttendances> StudentAttendances { get; set; }
        public DbSet<StudentGrades> StudentGrades { get; set; }
        public DbSet<Student_Evidence> Student_Evidence { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite primary key for Student_Class_Section_Year
            modelBuilder.Entity<Student_Class_Section_Year>()
                .HasKey(e => new { e.Student_Id, e.Working_Year_Id, e.Section_id });

            // Configure composite foreign key relationships for StudentAbsents
            modelBuilder.Entity<StudentAbsents>()
                .HasOne(sa => sa.StudentClassSectionYear)
                .WithMany(scsy => scsy.StudentAbsents)
                .HasForeignKey(sa => new { sa.StudentClassSectionYear_Student_Id, sa.StudentClassSectionYear_Working_Year_Id, sa.StudentClassSectionYear_Section_id })
                .OnDelete(DeleteBehavior.NoAction);

            // Configure composite foreign key relationships for StudentAttendances
            modelBuilder.Entity<StudentAttendances>()
                .HasOne(sa => sa.StudentClassSectionYear)
                .WithMany(scsy => scsy.StudentAttendances)
                .HasForeignKey(sa => new { sa.StudentClassSectionYear_Student_Id, sa.StudentClassSectionYear_Working_Year_Id, sa.StudentClassSectionYear_Section_id })
                .OnDelete(DeleteBehavior.NoAction);

            // --- Start of configurations to resolve cascade delete issue for all relationships ---

            // EmployeeType and Employees
            modelBuilder.Entity<EmployeeTypes>()
                .HasMany(et => et.Employees)
                .WithOne(e => e.Role)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            // Employees self-referencing (CreatedBy)
            modelBuilder.Entity<Employees>()
                .HasOne(e => e.CreatedBy)
                .WithMany(e => e.CreatedEmployees)
                .HasForeignKey(e => e.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Department and Employees (CreatedBy)
            modelBuilder.Entity<Department>()
                .HasOne(d => d.CreatedBy)
                .WithMany(e => e.CreatedDepartments)
                .HasForeignKey(d => d.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Employee_Department and Employees (User)
            modelBuilder.Entity<Employee_Department>()
                .HasOne(ed => ed.User)
                .WithMany(e => e.EmployeeDepartments)
                .HasForeignKey(ed => ed.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Employee_Department and Department
            modelBuilder.Entity<Employee_Department>()
                .HasOne(ed => ed.Department)
                .WithMany(d => d.EmployeeDepartments)
                .HasForeignKey(ed => ed.Department_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Working_Year and Employees (CreatedBy)
            modelBuilder.Entity<Working_Year>()
                .HasOne(wy => wy.CreatedBy)
                .WithMany(e => e.CreatedWorkingYears)
                .HasForeignKey(wy => wy.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Section and Department
            modelBuilder.Entity<Section>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Sections)
                .HasForeignKey(s => s.Department_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Section and Employees (CreatedBy)
            modelBuilder.Entity<Section>()
                .HasOne(s => s.CreatedBy)
                .WithMany(e => e.CreatedSections)
                .HasForeignKey(s => s.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // AbsenceReasons and Employees (CreatedBy)
            modelBuilder.Entity<AbsenceReasons>()
                .HasOne(ar => ar.CreatedBy)
                .WithMany(e => e.CreatedAbsenceReasons)
                .HasForeignKey(ar => ar.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Classes and Employees (CreatedBy)
            modelBuilder.Entity<Classes>()
                .HasOne(c => c.CreatedBy)
                .WithMany(e => e.CreatedClasses)
                .HasForeignKey(c => c.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Competencies and Employees (CreatedBy)
            modelBuilder.Entity<Competencies>()
                .HasOne(c => c.CreatedBy)
                .WithMany(e => e.CreatedCompetences)
                .HasForeignKey(c => c.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Competencies and Section
            modelBuilder.Entity<Competencies>()
                .HasOne(c => c.Section)
                .WithMany(s => s.Competencies)
                .HasForeignKey(c => c.Section_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Grades and Employees (CreatedBy)
            modelBuilder.Entity<Grades>()
                .HasOne(g => g.CreatedBy)
                .WithMany(e => e.CreatedGrades)
                .HasForeignKey(g => g.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Outcomes and Competencies
            modelBuilder.Entity<Learning_Outcome>()
                .HasOne(o => o.Competency)
                .WithMany(c => c.Learning_Outcomes)
                .HasForeignKey(o => o.Competency_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Outcomes and Employees (CreatedBy)
            modelBuilder.Entity<Learning_Outcome>()
                .HasOne(lo => lo.CreatedBy)
                .WithMany(e => e.CreatedLearningOutcomes)
                .HasForeignKey(lo => lo.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Evidence and Learning_Outcome
            modelBuilder.Entity<Evidence>()
                .HasOne(e => e.Learning_Outcome)
                .WithMany(lo => lo.Evidences)
                .HasForeignKey(e => e.Outcome_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Evidence and Employees (CreatedBy)
            modelBuilder.Entity<Evidence>()
                .HasOne(e => e.CreatedBy)
                .WithMany(emp => emp.CreatedEvidences)
                .HasForeignKey(e => e.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Try and Employees (CreatedBy)
            modelBuilder.Entity<Try>()
                .HasOne(t => t.CreatedBy)
                .WithMany(e => e.CreatedTries)
                .HasForeignKey(t => t.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Student_Evidence and Employees (CreatedBy)
            modelBuilder.Entity<Student_Evidence>()
                .HasOne(se => se.CreatedBy)
                .WithMany(e => e.CreatedStudentEvidences)
                .HasForeignKey(se => se.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Student_Evidence and Evidence
            modelBuilder.Entity<Student_Evidence>()
                .HasOne(se => se.Evidence)
                .WithMany(e => e.Student_Evidences)
                .HasForeignKey(se => se.Evidence_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Student_Evidence and Try
            modelBuilder.Entity<Student_Evidence>()
                .HasOne(se => se.Try)
                .WithMany(t => t.Student_Evidences)
                .HasForeignKey(se => se.Try_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Student_Evidence and Classes
            modelBuilder.Entity<Student_Evidence>()
                .HasOne(se => se.Class)
                .WithMany(c => c.StudentEvidences)
                .HasForeignKey(se => se.Class_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Student_Evidence and Student_Class_Section_Year
            modelBuilder.Entity<Student_Evidence>()
                .HasOne(se => se.StudentClassSectionYear)
                .WithMany(scsy => scsy.StudentEvidences)
                .HasForeignKey(se => new { se.Student_Id, se.Working_Year_Id, se.Section_id })
                .OnDelete(DeleteBehavior.NoAction);


            // RequestExits and Employees (CreatedBy)
            modelBuilder.Entity<RequestExits>()
                .HasOne(re => re.CreatedBy)
                .WithMany(e => e.CreatedRequestExits)
                .HasForeignKey(re => re.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // RequestExits and StudentAttendances
            modelBuilder.Entity<RequestExits>()
                .HasOne(re => re.Attendance)
                .WithMany(sa => sa.RequestExits)
                .HasForeignKey(re => re.AttendanceId)
                .OnDelete(DeleteBehavior.NoAction);

            // StudentAbsents and AbsenceReasons
            modelBuilder.Entity<StudentAbsents>()
                .HasOne(sa => sa.AbsenceReason)
                .WithMany(ar => ar.StudentAbsents)
                .HasForeignKey(sa => sa.AbsenceReasonId)
                .OnDelete(DeleteBehavior.NoAction);

            // StudentAbsents and AttendanceTypes
            modelBuilder.Entity<StudentAbsents>()
                .HasOne(sa => sa.AttendanceType)
                .WithMany(at => at.StudentAbsents)
                .HasForeignKey(sa => sa.AttendanceTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // StudentAbsents and Employees (CreatedBy)
            modelBuilder.Entity<StudentAbsents>()
                .HasOne(sa => sa.CreatedBy)
                .WithMany(e => e.CreatedStudentAbsents)
                .HasForeignKey(sa => sa.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // StudentAttendances and AttendanceTypes
            modelBuilder.Entity<StudentAttendances>()
                .HasOne(sa => sa.AttendanceType)
                .WithMany(at => at.StudentAttendances)
                .HasForeignKey(sa => sa.AttendanceTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // StudentAttendances and Employees (CreatedBy)
            modelBuilder.Entity<StudentAttendances>()
                .HasOne(sa => sa.CreatedBy)
                .WithMany(e => e.CreatedStudentAttendances)
                .HasForeignKey(sa => sa.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // StudentGrades and Grades
            modelBuilder.Entity<StudentGrades>()
                .HasOne(sg => sg.Grade)
                .WithMany(g => g.StudentGrades)
                .HasForeignKey(sg => sg.GradeId)
                .OnDelete(DeleteBehavior.NoAction);

            // StudentGrades and Grades (Duplicate config)
            modelBuilder.Entity<Classes>()
                .HasOne(sg => sg.Grade)
                .WithMany(g => g.CreatedClasses)
                .HasForeignKey(sg => sg.GradeId)
                .OnDelete(DeleteBehavior.NoAction);

            // StudentGrades and Students
            modelBuilder.Entity<StudentGrades>()
                .HasOne(sg => sg.Student)
                .WithMany(s => s.StudentGrades)
                .HasForeignKey(sg => sg.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            // StudentGrades and Working_Year (Already configured in previous step, ensuring it's here)
            modelBuilder.Entity<StudentGrades>()
                .HasOne(sg => sg.WorkingYear)
                .WithMany(wy => wy.StudentGrades)
                .HasForeignKey(sg => sg.Working_Year_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Students and Employees (CreatedBy)
            modelBuilder.Entity<Students>()
                .HasOne(s => s.CreatedBy)
                .WithMany(e => e.CreatedStudents)
                .HasForeignKey(s => s.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);


            // Configure relationships for Student_Class_Section_Year to prevent cascade delete cycles
            // Explicitly map navigation properties to their foreign keys and set delete behavior to NoAction.
            // This prevents EF Core from inferring additional foreign key columns (e.g., StudentId, SectionId).
            modelBuilder.Entity<Student_Class_Section_Year>()
                .HasOne(scsy => scsy.Student)
                .WithMany(s => s.StudentClassSectionYears) // Assuming Students has this collection
                .HasForeignKey(scsy => scsy.Student_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Student_Class_Section_Year>()
                .HasOne(scsy => scsy.Class)
                .WithMany(c => c.StudentClassSectionYears) // Assuming Classes has this collection
                .HasForeignKey(scsy => scsy.Class_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Student_Class_Section_Year>()
                .HasOne(scsy => scsy.WorkingYear)
                .WithMany(wy => wy.StudentClassSectionYears) // Assuming Working_Year has this collection
                .HasForeignKey(scsy => scsy.Working_Year_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Student_Class_Section_Year>()
                .HasOne(scsy => scsy.Section)
                .WithMany(s => s.StudentClassSectionYears) // Assuming Section has this collection
                .HasForeignKey(scsy => scsy.Section_id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Student_Class_Section_Year>()
                .HasOne(scsy => scsy.CreatedBy)
                .WithMany(e => e.CreatedStudentClassSectionYears) // Assuming Employees has this collection
                .HasForeignKey(scsy => scsy.CreatedBy_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // --- End of configurations ---

            base.OnModelCreating(modelBuilder);
        }
    }
}