# PowerShell Script to Create C# Models for Student Management System
Write-Host "Creating C# Models for Student Management System..." -ForegroundColor Green
Write-Host ""

# Create Models directory if it doesn't exist
if (!(Test-Path "Models")) {
    New-Item -ItemType Directory -Name "Models"
    Write-Host "Created Models directory" -ForegroundColor Yellow
}

# Create AttendanceTypes.cs
Write-Host "Creating AttendanceTypes.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class AttendanceTypes
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation properties
        public virtual ICollection<StudentAbsents> StudentAbsents { get; set; }
        public virtual ICollection<StudentAttendances> StudentAttendances { get; set; }
    }
}
"@ | Out-File -FilePath "Models\AttendanceTypes.cs" -Encoding UTF8

# Create Working_Year.cs
Write-Host "Creating Working_Year.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Working_Year
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }
        public virtual ICollection<Student_Class_Section_Year> StudentClassSectionYears { get; set; }
        public virtual ICollection<StudentGrades> StudentGrades { get; set; }
    }
}
"@ | Out-File -FilePath "Models\Working_Year.cs" -Encoding UTF8

# Create Student_Class_Section_Year.cs
Write-Host "Creating Student_Class_Section_Year.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Student_Class_Section_Year
    {
        [Key, Column(Order = 0)]
        public int Student_Id { get; set; }

        [Key, Column(Order = 1)]
        public int Class_Id { get; set; }

        [Key, Column(Order = 2)]
        public int Working_Year_Id { get; set; }

        [Key, Column(Order = 3)]
        public int Section_id { get; set; }

        public bool IsActive { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }

        // Navigation properties
        [ForeignKey("Student_Id")]
        public virtual Students Student { get; set; }

        [ForeignKey("Class_Id")]
        public virtual Classes Class { get; set; }

        [ForeignKey("Working_Year_Id")]
        public virtual Working_Year WorkingYear { get; set; }

        [ForeignKey("Section_id")]
        public virtual Section Section { get; set; }

        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        // Navigation properties for related entities
        public virtual ICollection<StudentAbsents> StudentAbsents { get; set; }
        public virtual ICollection<StudentAttendances> StudentAttendances { get; set; }
    }
}
"@ | Out-File -FilePath "Models\Student_Class_Section_Year.cs" -Encoding UTF8

# Create Section.cs
Write-Host "Creating Section.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }
        public int Department_Id { get; set; }
        public string Name_Of_Section { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }

        // Navigation properties
        [ForeignKey("Department_Id")]
        public virtual Department Department { get; set; }

        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        public virtual ICollection<Student_Class_Section_Year> StudentClassSectionYears { get; set; }
    }
}
"@ | Out-File -FilePath "Models\Section.cs" -Encoding UTF8

# Create AbsenceReasons.cs
Write-Host "Creating AbsenceReasons.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class AbsenceReasons
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }
        public virtual ICollection<StudentAbsents> StudentAbsents { get; set; }
    }
}
"@ | Out-File -FilePath "Models\AbsenceReasons.cs" -Encoding UTF8

# Create Classes.cs
Write-Host "Creating Classes.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Classes
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }
        public int Department_Id { get; set; }
        public int MaxStudents { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("Department_Id")]
        public virtual Department Department { get; set; }

        public virtual ICollection<Students> Students { get; set; }
        public virtual ICollection<Student_Class_Section_Year> StudentClassSectionYears { get; set; }
    }
}
"@ | Out-File -FilePath "Models\Classes.cs" -Encoding UTF8

# Create Competences.cs
Write-Host "Creating Competences.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Competences
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Duration { get; set; }
        public int Department_Id { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("Department_Id")]
        public virtual Department Department { get; set; }

        public virtual ICollection<Outcomes> Outcomes { get; set; }
    }
}
"@ | Out-File -FilePath "Models\Competences.cs" -Encoding UTF8

# Create Employees.cs
Write-Host "Creating Employees.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Employees
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastLogin { get; set; }
        public string Email { get; set; }
        public DateTime Join_Date { get; set; }

        // Navigation properties
        [ForeignKey("RoleId")]
        public virtual EmployeeTypes Role { get; set; }

        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        // Self-referencing navigation
        public virtual ICollection<Employees> CreatedEmployees { get; set; }

        // Other navigation properties
        public virtual ICollection<Employee_Department> EmployeeDepartments { get; set; }
        public virtual ICollection<AbsenceReasons> CreatedAbsenceReasons { get; set; }
        public virtual ICollection<Classes> CreatedClasses { get; set; }
        public virtual ICollection<Competences> CreatedCompetences { get; set; }
        public virtual ICollection<Department> CreatedDepartments { get; set; }
        public virtual ICollection<Grades> CreatedGrades { get; set; }
        public virtual ICollection<Section> CreatedSections { get; set; }
        public virtual ICollection<Pictures> CreatedPictures { get; set; }
        public virtual ICollection<RequestExits> CreatedRequestExits { get; set; }
        public virtual ICollection<StudentAbsents> CreatedStudentAbsents { get; set; }
        public virtual ICollection<StudentAttendances> CreatedStudentAttendances { get; set; }
        public virtual ICollection<Students> CreatedStudents { get; set; }
        public virtual ICollection<Student_Class_Section_Year> CreatedStudentClassSectionYears { get; set; }
        public virtual ICollection<Working_Year> CreatedWorkingYears { get; set; }
    }
}
"@ | Out-File -FilePath "Models\Employees.cs" -Encoding UTF8

# Create EmployeeTypes.cs
Write-Host "Creating EmployeeTypes.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class EmployeeTypes
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<Employees> Employees { get; set; }
    }
}
"@ | Out-File -FilePath "Models\EmployeeTypes.cs" -Encoding UTF8

# Create Employee_Department.cs
Write-Host "Creating Employee_Department.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Employee_Department
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Department_Id { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual Employees User { get; set; }

        [ForeignKey("Department_Id")]
        public virtual Department Department { get; set; }
    }
}
"@ | Out-File -FilePath "Models\Employee_Department.cs" -Encoding UTF8

# Create Department.cs
Write-Host "Creating Department.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        public virtual ICollection<Classes> Classes { get; set; }
        public virtual ICollection<Competences> Competences { get; set; }
        public virtual ICollection<Employee_Department> EmployeeDepartments { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
    }
}
"@ | Out-File -FilePath "Models\Department.cs" -Encoding UTF8

# Create StudentAbsents.cs
Write-Host "Creating StudentAbsents.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class StudentAbsents
    {
        [Key]
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }
        public int AbsenceReasonId { get; set; }
        public int AttendanceTypeId { get; set; }
        public string CustomReasonDetails { get; set; }

        // Composite key reference fields
        public int StudentClassSectionYear_Student_Id { get; set; }
        public int StudentClassSectionYear_Class_Id { get; set; }
        public int StudentClassSectionYear_Working_Year_Id { get; set; }
        public int StudentClassSectionYear_Section_id { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("AbsenceReasonId")]
        public virtual AbsenceReasons AbsenceReason { get; set; }

        [ForeignKey("AttendanceTypeId")]
        public virtual AttendanceTypes AttendanceType { get; set; }

        // Composite foreign key reference
        [ForeignKey("StudentClassSectionYear_Student_Id,StudentClassSectionYear_Class_Id,StudentClassSectionYear_Working_Year_Id,StudentClassSectionYear_Section_id")]
        public virtual Student_Class_Section_Year StudentClassSectionYear { get; set; }
    }
}
"@ | Out-File -FilePath "Models\StudentAbsents.cs" -Encoding UTF8

# Create StudentAttendances.cs
Write-Host "Creating StudentAttendances.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class StudentAttendances
    {
        [Key]
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }
        public int AttendanceTypeId { get; set; }
        public string CustomReasonDetails { get; set; }

        // Composite key reference fields
        public int StudentClassSectionYear_Student_Id { get; set; }
        public int StudentClassSectionYear_Class_Id { get; set; }
        public int StudentClassSectionYear_Working_Year_Id { get; set; }
        public int StudentClassSectionYear_Section_id { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("AttendanceTypeId")]
        public virtual AttendanceTypes AttendanceType { get; set; }

        // Composite foreign key reference
        [ForeignKey("StudentClassSectionYear_Student_Id,StudentClassSectionYear_Class_Id,StudentClassSectionYear_Working_Year_Id,StudentClassSectionYear_Section_id")]
        public virtual Student_Class_Section_Year StudentClassSectionYear { get; set; }

        public virtual ICollection<RequestExits> RequestExits { get; set; }
    }
}
"@ | Out-File -FilePath "Models\StudentAttendances.cs" -Encoding UTF8

# Create Grades.cs
Write-Host "Creating Grades.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Grades
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }
        public virtual ICollection<StudentGrades> StudentGrades { get; set; }
    }
}
"@ | Out-File -FilePath "Models\Grades.cs" -Encoding UTF8

# Create Outcomes.cs
Write-Host "Creating Outcomes.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Outcomes
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int CompId { get; set; }

        // Navigation properties
        [ForeignKey("CompId")]
        public virtual Competences Competence { get; set; }
        public virtual ICollection<TaskEvaluations> TaskEvaluations { get; set; }
    }
}
"@ | Out-File -FilePath "Models\Outcomes.cs" -Encoding UTF8

# Create Pictures.cs
Write-Host "Creating Pictures.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Pictures
    {
        [Key]
        public int Id { get; set; }
        public string FilePath { get; set; }
        public int StudentId { get; set; }
        public bool IsDeleted { get; set; }
        public int TaskId { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("StudentId")]
        public virtual Students Student { get; set; }

        [ForeignKey("TaskId")]
        public virtual TaskEvaluations Task { get; set; }
    }
}
"@ | Out-File -FilePath "Models\Pictures.cs" -Encoding UTF8

# Create RequestExits.cs
Write-Host "Creating RequestExits.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class RequestExits
    {
        [Key]
        public int Id { get; set; }
        public string Reason { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }
        public int AttendanceId { get; set; }
        public TimeSpan ExitTime { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("AttendanceId")]
        public virtual StudentAttendances Attendance { get; set; }
    }
}
"@ | Out-File -FilePath "Models\RequestExits.cs" -Encoding UTF8

# Create StudentGrades.cs
Write-Host "Creating StudentGrades.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class StudentGrades
    {
        [Key]
        public int Id { get; set; }
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public int Working_Year_Id { get; set; }

        // Navigation properties
        [ForeignKey("GradeId")]
        public virtual Grades Grade { get; set; }

        [ForeignKey("StudentId")]
        public virtual Students Student { get; set; }

        [ForeignKey("Working_Year_Id")]
        public virtual Working_Year WorkingYear { get; set; }
    }
}
"@ | Out-File -FilePath "Models\StudentGrades.cs" -Encoding UTF8

# Create Students.cs
Write-Host "Creating Students.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Students
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }
        public int ClassId { get; set; }
        public string Adress { get; set; }
        public string Code { get; set; }
        public string Date_of_birth { get; set; }
        public string Email { get; set; }
        public string Governate { get; set; }
        public string Jop_of_Father { get; set; }
        public string Jop_of_Mother { get; set; }
        public string Natrual_Id { get; set; }
        public string Phone_Number { get; set; }
        public string Phone_Number_Father { get; set; }
        public string Phone_Number_Mother { get; set; }
        public string Picture_Profile { get; set; }
        public string birth_Certificate { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Class { get; set; }

        public virtual ICollection<Pictures> Pictures { get; set; }
        public virtual ICollection<StudentGrades> StudentGrades { get; set; }
        public virtual ICollection<TaskEvaluations> TaskEvaluations { get; set; }
        public virtual ICollection<Student_Class_Section_Year> StudentClassSectionYears { get; set; }
    }
}
"@ | Out-File -FilePath "Models\Students.cs" -Encoding UTF8

# Create TaskEvaluations.cs
Write-Host "Creating TaskEvaluations.cs..." -ForegroundColor Cyan
@"
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class TaskEvaluations
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDeleted { get; set; }
        public int OutcomeId { get; set; }
        public int StudentId { get; set; }

        // Navigation properties
        [ForeignKey("OutcomeId")]
        public virtual Outcomes Outcome { get; set; }

        [ForeignKey("StudentId")]
        public virtual Students Student { get; set; }

        public virtual ICollection<Pictures> Pictures { get; set; }
    }
}
"@ | Out-File -FilePath "Models\TaskEvaluations.cs" -Encoding UTF8

# Create DbContext.cs
Write-Host "Creating StudentManagementDbContext.cs..." -ForegroundColor Cyan
@"
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Models
{
    public class StudentManagementDbContext : DbContext
    {
        public StudentManagementDbContext(DbContextOptions<StudentManagementDbContext> options) : base(options)
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
"@ | Out-File -FilePath "Models\StudentManagementDbContext.cs" -Encoding UTF8

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "All C# Models have been created successfully!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Models created in the 'Models' folder:" -ForegroundColor Yellow

$modelFiles = @(
    "AttendanceTypes.cs",
    "Working_Year.cs", 
    "Student_Class_Section_Year.cs (with composite key)",
    "Section.cs",
    "AbsenceReasons.cs",
    "Classes.cs",
    "Competences.cs", 
    "Employees.cs",
    "EmployeeTypes.cs",
    "Employee_Department.cs",
    "Department.cs",
    "Grades.cs",
    "Outcomes.cs",
    "Pictures.cs",
    "RequestExits.cs",
    "StudentAbsents.cs (with corrected composite key relationship)",
    "StudentAttendances.cs (with corrected composite key relationship)",
    "StudentGrades.cs",
    "Students.cs",
    "TaskEvaluations.cs",
    "StudentManagementDbContext.cs (DbContext)"
)

foreach ($file in $modelFiles) {
    Write-Host "- $file" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "Key fixes implemented:" -ForegroundColor Yellow
Write-Host "1. Fixed composite key relationships in StudentAbsents and StudentAttendances" -ForegroundColor White
Write-Host "2. Added proper foreign key fields for composite key references" -ForegroundColor White
Write-Host "3. Configured Entity Framework relationships in DbContext" -ForegroundColor White
Write-Host "4. All navigation properties are properly set up" -ForegroundColor White
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Add Entity Framework Core packages to your project" -ForegroundColor White
Write-Host "2. Configure connection string in appsettings.json" -ForegroundColor White
Write-Host "3. Register DbContext in Program.cs or Startup.cs" -ForegroundColor White
Write-Host "4. Run migrations to create database" -ForegroundColor White
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Green
Read-Host