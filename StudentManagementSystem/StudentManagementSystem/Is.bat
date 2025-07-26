@echo off
echo Creating C# Models for Student Management System...

REM Create Models Directory
if not exist "Models" mkdir Models

REM Create AttendanceTypes Model
echo Creating AttendanceTypes.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class AttendanceTypes
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string Name { get; set; }
echo.
echo         public string Description { get; set; }
echo.
echo         // Navigation Properties
echo         public virtual ICollection^<StudentAbsents^> StudentAbsents { get; set; }
echo         public virtual ICollection^<StudentAttendances^> StudentAttendances { get; set; }
echo     }
echo }
) > Models\AttendanceTypes.cs

REM Create Working_Year Model
echo Creating Working_Year.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class Working_Year
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string Name { get; set; }
echo.
echo         public DateTime Start_date { get; set; }
echo.
echo         public DateTime End_date { get; set; }
echo.
echo         public bool IsActive { get; set; }
echo.
echo         public int? CreatedBy_Id { get; set; }
echo.
echo         public DateTime? Date { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         public virtual ICollection^<Studnet_Class_Section_year^> StudentClassSectionYears { get; set; }
echo         public virtual ICollection^<StudentGrades^> StudentGrades { get; set; }
echo     }
echo }
) > Models\Working_Year.cs

REM Create Studnet_Class_Section_year Model
echo Creating Studnet_Class_Section_year.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class Studnet_Class_Section_year
echo     {
echo         [Key, Column(Order = 0)]
echo         public int Student_Id { get; set; }
echo.
echo         [Key, Column(Order = 1)]
echo         public int Class_Id { get; set; }
echo.
echo         [Key, Column(Order = 2)]
echo         public int Working_Year_Id { get; set; }
echo.
echo         [Key, Column(Order = 3)]
echo         public int Section_id { get; set; }
echo.
echo         public bool IsActive { get; set; }
echo.
echo         public int? CreatedBy_Id { get; set; }
echo.
echo         public DateTime? Date { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("Student_Id")]
echo         public virtual Students Student { get; set; }
echo.
echo         [ForeignKey("Class_Id")]
echo         public virtual Classes Class { get; set; }
echo.
echo         [ForeignKey("Working_Year_Id")]
echo         public virtual Working_Year WorkingYear { get; set; }
echo.
echo         [ForeignKey("Section_id")]
echo         public virtual Section Section { get; set; }
echo.
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         public virtual ICollection^<StudentAbsents^> StudentAbsents { get; set; }
echo         public virtual ICollection^<StudentAttendances^> StudentAttendances { get; set; }
echo     }
echo }
) > Models\Studnet_Class_Section_year.cs

REM Create Section Model
echo Creating Section.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class Section
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public int Department_Id { get; set; }
echo.
echo         public string Name_Of_Section { get; set; }
echo.
echo         public bool IsActive { get; set; }
echo.
echo         public int CreatedBy_Id { get; set; }
echo.
echo         public DateTime Date { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("Department_Id")]
echo         public virtual Department Department { get; set; }
echo.
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         public virtual ICollection^<Studnet_Class_Section_year^> StudentClassSectionYears { get; set; }
echo     }
echo }
) > Models\Section.cs

REM Create AbsenceReasons Model
echo Creating AbsenceReasons.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class AbsenceReasons
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string Name { get; set; }
echo.
echo         public bool IsDeleted { get; set; }
echo.
echo         public int CreatedBy_Id { get; set; }
echo.
echo         public DateTime CreatedDate { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         public virtual ICollection^<StudentAbsents^> StudentAbsents { get; set; }
echo     }
echo }
) > Models\AbsenceReasons.cs

REM Create Classes Model
echo Creating Classes.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class Classes
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string Name { get; set; }
echo.
echo         public bool IsActive { get; set; }
echo.
echo         public int CreatedBy_Id { get; set; }
echo.
echo         public DateTime Date { get; set; }
echo.
echo         public int Department_Id { get; set; }
echo.
echo         public int MaxStudents { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         [ForeignKey("Department_Id")]
echo         public virtual Department Department { get; set; }
echo.
echo         public virtual ICollection^<Students^> Students { get; set; }
echo         public virtual ICollection^<Studnet_Class_Section_year^> StudentClassSectionYears { get; set; }
echo     }
echo }
) > Models\Classes.cs

REM Create Competences Model
echo Creating Competences.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class Competences
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string Name { get; set; }
echo.
echo         public bool IsActive { get; set; }
echo.
echo         public int Duration { get; set; }
echo.
echo         public int Department_Id { get; set; }
echo.
echo         public int CreatedBy_Id { get; set; }
echo.
echo         public DateTime CreatedDate { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         [ForeignKey("Department_Id")]
echo         public virtual Department Department { get; set; }
echo.
echo         public virtual ICollection^<Outcomes^> Outcomes { get; set; }
echo     }
echo }
) > Models\Competences.cs

REM Create Employees Model
echo Creating Employees.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class Employees
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string Name { get; set; }
echo.
echo         public string Username { get; set; }
echo.
echo         public bool IsActive { get; set; }
echo.
echo         public string Password { get; set; }
echo.
echo         public int RoleId { get; set; }
echo.
echo         public int CreatedBy_Id { get; set; }
echo.
echo         public DateTime Date { get; set; }
echo.
echo         public DateTime LastLogin { get; set; }
echo.
echo         public string Email { get; set; }
echo.
echo         public DateTime Join_Date { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("RoleId")]
echo         public virtual EmployeeTypes EmployeeType { get; set; }
echo.
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         public virtual ICollection^<Employee_Department^> EmployeeDepartments { get; set; }
echo         public virtual ICollection^<AbsenceReasons^> CreatedAbsenceReasons { get; set; }
echo         public virtual ICollection^<Classes^> CreatedClasses { get; set; }
echo         public virtual ICollection^<Competences^> CreatedCompetences { get; set; }
echo         public virtual ICollection^<Department^> CreatedDepartments { get; set; }
echo         public virtual ICollection^<Grades^> CreatedGrades { get; set; }
echo         public virtual ICollection^<Section^> CreatedSections { get; set; }
echo         public virtual ICollection^<Pictures^> CreatedPictures { get; set; }
echo         public virtual ICollection^<RequestExits^> CreatedRequestExits { get; set; }
echo         public virtual ICollection^<StudentAbsents^> CreatedStudentAbsents { get; set; }
echo         public virtual ICollection^<StudentAttendances^> CreatedStudentAttendances { get; set; }
echo         public virtual ICollection^<Students^> CreatedStudents { get; set; }
echo         public virtual ICollection^<Studnet_Class_Section_year^> CreatedStudentClassSectionYears { get; set; }
echo         public virtual ICollection^<Working_Year^> CreatedWorkingYears { get; set; }
echo     }
echo }
) > Models\Employees.cs

REM Create EmployeeTypes Model
echo Creating EmployeeTypes.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class EmployeeTypes
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string Name { get; set; }
echo.
echo         public bool IsDeleted { get; set; }
echo.
echo         public int CreatedBy_Id { get; set; }
echo.
echo         public DateTime CreatedDate { get; set; }
echo.
echo         // Navigation Properties
echo         public virtual ICollection^<Employees^> Employees { get; set; }
echo     }
echo }
) > Models\EmployeeTypes.cs

REM Create Employee_Department Model
echo Creating Employee_Department.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class Employee_Department
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public int UserId { get; set; }
echo.
echo         public int Department_Id { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("UserId")]
echo         public virtual Employees Employee { get; set; }
echo.
echo         [ForeignKey("Department_Id")]
echo         public virtual Department Department { get; set; }
echo     }
echo }
) > Models\Employee_Department.cs

REM Create Department Model
echo Creating Department.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class Department
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string Name { get; set; }
echo.
echo         public bool IsActive { get; set; }
echo.
echo         public int CreatedBy_Id { get; set; }
echo.
echo         public DateTime CreatedDate { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         public virtual ICollection^<Classes^> Classes { get; set; }
echo         public virtual ICollection^<Competences^> Competences { get; set; }
echo         public virtual ICollection^<Employee_Department^> EmployeeDepartments { get; set; }
echo         public virtual ICollection^<Section^> Sections { get; set; }
echo     }
echo }
) > Models\Department.cs

REM Create Grades Model
echo Creating Grades.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class Grades
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string Name { get; set; }
echo.
echo         public bool IsActive { get; set; }
echo.
echo         public int CreatedBy_Id { get; set; }
echo.
echo         public DateTime Date { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         public virtual ICollection^<StudentGrades^> StudentGrades { get; set; }
echo     }
echo }
) > Models\Grades.cs

REM Create Outcomes Model
echo Creating Outcomes.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class Outcomes
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string Name { get; set; }
echo.
echo         public bool IsActive { get; set; }
echo.
echo         public int CompId { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("CompId")]
echo         public virtual Competences Competence { get; set; }
echo.
echo         public virtual ICollection^<TaskEvaluations^> TaskEvaluations { get; set; }
echo     }
echo }
) > Models\Outcomes.cs

REM Create Pictures Model
echo Creating Pictures.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class Pictures
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string FilePath { get; set; }
echo.
echo         public int StudentId { get; set; }
echo.
echo         public bool IsDeleted { get; set; }
echo.
echo         public int TaskId { get; set; }
echo.
echo         public int CreatedBy_Id { get; set; }
echo.
echo         public DateTime CreatedDate { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         [ForeignKey("StudentId")]
echo         public virtual Students Student { get; set; }
echo.
echo         [ForeignKey("TaskId")]
echo         public virtual TaskEvaluations TaskEvaluation { get; set; }
echo     }
echo }
) > Models\Pictures.cs

REM Create RequestExits Model
echo Creating RequestExits.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class RequestExits
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string Reason { get; set; }
echo.
echo         public bool IsDeleted { get; set; }
echo.
echo         public int CreatedBy_Id { get; set; }
echo.
echo         public DateTime Date { get; set; }
echo.
echo         public int AttendanceId { get; set; }
echo.
echo         public TimeSpan ExitTime { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         [ForeignKey("AttendanceId")]
echo         public virtual StudentAttendances StudentAttendance { get; set; }
echo     }
echo }
) > Models\RequestExits.cs

REM Create StudentAbsents Model
echo Creating StudentAbsents.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class StudentAbsents
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public bool IsDeleted { get; set; }
echo.
echo         public int CreatedBy_Id { get; set; }
echo.
echo         public DateTime Date { get; set; }
echo.
echo         public int AbsenceReasonId { get; set; }
echo.
echo         public int AttendanceTypeId { get; set; }
echo.
echo         public string CustomReasonDetails { get; set; }
echo.
echo         // Composite Foreign Key for Student Class Section Year
echo         public int StudentClassSectionYear_Student_Id { get; set; }
echo         public int StudentClassSectionYear_Class_Id { get; set; }
echo         public int StudentClassSectionYear_Working_Year_Id { get; set; }
echo         public int StudentClassSectionYear_Section_id { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         [ForeignKey("AbsenceReasonId")]
echo         public virtual AbsenceReasons AbsenceReason { get; set; }
echo.
echo         [ForeignKey("AttendanceTypeId")]
echo         public virtual AttendanceTypes AttendanceType { get; set; }
echo.
echo         [ForeignKey("StudentClassSectionYear_Student_Id,StudentClassSectionYear_Class_Id,StudentClassSectionYear_Working_Year_Id,StudentClassSectionYear_Section_id")]
echo         public virtual Studnet_Class_Section_year StudentClassSectionYear { get; set; }
echo     }
echo }
) > Models\StudentAbsents.cs

REM Create StudentAttendances Model
echo Creating StudentAttendances.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class StudentAttendances
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public bool IsDeleted { get; set; }
echo.
echo         public int CreatedBy_Id { get; set; }
echo.
echo         public DateTime Date { get; set; }
echo.
echo         public int AttendanceTypeId { get; set; }
echo.
echo         public string CustomReasonDetails { get; set; }
echo.
echo         // Composite Foreign Key for Student Class Section Year
echo         public int StudentClassSectionYear_Student_Id { get; set; }
echo         public int StudentClassSectionYear_Class_Id { get; set; }
echo         public int StudentClassSectionYear_Working_Year_Id { get; set; }
echo         public int StudentClassSectionYear_Section_id { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         [ForeignKey("AttendanceTypeId")]
echo         public virtual AttendanceTypes AttendanceType { get; set; }
echo.
echo         [ForeignKey("StudentClassSectionYear_Student_Id,StudentClassSectionYear_Class_Id,StudentClassSectionYear_Working_Year_Id,StudentClassSectionYear_Section_id")]
echo         public virtual Studnet_Class_Section_year StudentClassSectionYear { get; set; }
echo.
echo         public virtual ICollection^<RequestExits^> RequestExits { get; set; }
echo     }
echo }
) > Models\StudentAttendances.cs

REM Create StudentGrades Model
echo Creating StudentGrades.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class StudentGrades
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public int GradeId { get; set; }
echo.
echo         public int StudentId { get; set; }
echo.
echo         public DateTime Date { get; set; }
echo.
echo         public bool IsActive { get; set; }
echo.
echo         public int Working_Year_Id { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("GradeId")]
echo         public virtual Grades Grade { get; set; }
echo.
echo         [ForeignKey("StudentId")]
echo         public virtual Students Student { get; set; }
echo.
echo         [ForeignKey("Working_Year_Id")]
echo         public virtual Working_Year WorkingYear { get; set; }
echo     }
echo }
) > Models\StudentGrades.cs

REM Create Students Model
echo Creating Students.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class Students
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string Name { get; set; }
echo.
echo         public bool IsActive { get; set; }
echo.
echo         public int CreatedBy_Id { get; set; }
echo.
echo         public DateTime Date { get; set; }
echo.
echo         public int ClassId { get; set; }
echo.
echo         public string Adress { get; set; }
echo.
echo         public string Code { get; set; }
echo.
echo         public string Date_of_birth { get; set; }
echo.
echo         public string Email { get; set; }
echo.
echo         public string Governate { get; set; }
echo.
echo         public string Jop_of_Father { get; set; }
echo.
echo         public string Jop_of_Mother { get; set; }
echo.
echo         public string Natrual_Id { get; set; }
echo.
echo         public string Phone_Number { get; set; }
echo.
echo         public string Phone_Number_Father { get; set; }
echo.
echo         public string Phone_Number_Mother { get; set; }
echo.
echo         public string Picture_Profile { get; set; }
echo.
echo         public string birth_Certificate { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("ClassId")]
echo         public virtual Classes Class { get; set; }
echo.
echo         [ForeignKey("CreatedBy_Id")]
echo         public virtual Employees CreatedBy { get; set; }
echo.
echo         public virtual ICollection^<Pictures^> Pictures { get; set; }
echo         public virtual ICollection^<StudentGrades^> StudentGrades { get; set; }
echo         public virtual ICollection^<TaskEvaluations^> TaskEvaluations { get; set; }
echo         public virtual ICollection^<Studnet_Class_Section_year^> StudentClassSectionYears { get; set; }
echo     }
echo }
) > Models\Students.cs

REM Create TaskEvaluations Model
echo Creating TaskEvaluations.cs...
(
echo using System.ComponentModel.DataAnnotations;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class TaskEvaluations
echo     {
echo         [Key]
echo         public int Id { get; set; }
echo.
echo         public string Title { get; set; }
echo.
echo         public bool IsDeleted { get; set; }
echo.
echo         public int OutcomeId { get; set; }
echo.
echo         public int StudentId { get; set; }
echo.
echo         // Navigation Properties
echo         [ForeignKey("OutcomeId")]
echo         public virtual Outcomes Outcome { get; set; }
echo.
echo         [ForeignKey("StudentId")]
echo         public virtual Students Student { get; set; }
echo.
echo         public virtual ICollection^<Pictures^> Pictures { get; set; }
echo     }
echo }
) > Models\TaskEvaluations.cs

echo.
echo All C# Models have been created successfully in the Models folder!

REM Create DbContext with Composite Key Configuration
echo Creating ApplicationDbContext.cs...
(
echo using Microsoft.EntityFrameworkCore;
echo using System.ComponentModel.DataAnnotations.Schema;
echo.
echo namespace StudentManagementSystem.Models
echo {
echo     public class ApplicationDbContext : DbContext
echo     {
echo         public ApplicationDbContext(DbContextOptions^<ApplicationDbContext^> options) : base(options) { }
echo.
echo         // DbSets for all models
echo         public DbSet^<AttendanceTypes^> AttendanceTypes { get; set; }
echo         public DbSet^<Working_Year^> Working_Years { get; set; }
echo         public DbSet^<Studnet_Class_Section_year^> Student_Class_Section_Years { get; set; }
echo         public DbSet^<Section^> Sections { get; set; }
echo         public DbSet^<AbsenceReasons^> AbsenceReasons { get; set; }
echo         public DbSet^<Classes^> Classes { get; set; }
echo         public DbSet^<Competences^> Competences { get; set; }
echo         public DbSet^<Employees^> Employees { get; set; }
echo         public DbSet^<EmployeeTypes^> EmployeeTypes { get; set; }
echo         public DbSet^<Employee_Department^> Employee_Departments { get; set; }
echo         public DbSet^<Department^> Departments { get; set; }
echo         public DbSet^<Grades^> Grades { get; set; }
echo         public DbSet^<Outcomes^> Outcomes { get; set; }
echo         public DbSet^<Pictures^> Pictures { get; set; }
echo         public DbSet^<RequestExits^> RequestExits { get; set; }
echo         public DbSet^<StudentAbsents^> StudentAbsents { get; set; }
echo         public DbSet^<StudentAttendances^> StudentAttendances { get; set; }
echo         public DbSet^<StudentGrades^> StudentGrades { get; set; }
echo         public DbSet^<Students^> Students { get; set; }
echo         public DbSet^<TaskEvaluations^> TaskEvaluations { get; set; }
echo.
echo         protected override void OnModelCreating(ModelBuilder modelBuilder)
echo         {
echo             base.OnModelCreating(modelBuilder);
echo.
echo             // Configure Composite Primary Key for Studnet_Class_Section_year
echo             modelBuilder.Entity^<Studnet_Class_Section_year^>()
echo                 .HasKey(e =^> new { e.Student_Id, e.Class_Id, e.Working_Year_Id, e.Section_id });
echo.
echo             // Configure relationships for StudentAbsents with composite foreign key
echo             modelBuilder.Entity^<StudentAbsents^>()
echo                 .HasOne(sa =^> sa.StudentClassSectionYear)
echo                 .WithMany(scsy =^> scsy.StudentAbsents)
echo                 .HasForeignKey(sa =^> new { 
echo                     sa.StudentClassSectionYear_Student_Id, 
echo                     sa.StudentClassSectionYear_Class_Id, 
echo                     sa.StudentClassSectionYear_Working_Year_Id, 
echo                     sa.StudentClassSectionYear_Section_id 
echo                 });
echo.
echo             // Configure relationships for StudentAttendances with composite foreign key
echo             modelBuilder.Entity^<StudentAttendances^>()
echo                 .HasOne(sa =^> sa.StudentClassSectionYear)
echo                 .WithMany(scsy =^> scsy.StudentAttendances)
echo                 .HasForeignKey(sa =^> new { 
echo                     sa.StudentClassSectionYear_Student_Id, 
echo                     sa.StudentClassSectionYear_Class_Id, 
echo                     sa.StudentClassSectionYear_Working_Year_Id, 
echo                     sa.StudentClassSectionYear_Section_id 
echo                 });
echo.
echo             // Configure self-referencing relationship for Employees
echo             modelBuilder.Entity^<Employees^>()
echo                 .HasOne(e =^> e.CreatedBy)
echo                 .WithMany()
echo                 .HasForeignKey(e =^> e.CreatedBy_Id)
echo                 .OnDelete(DeleteBehavior.Restrict);
echo.
echo             // Configure nullable foreign keys
echo             modelBuilder.Entity^<Working_Year^>()
echo                 .HasOne(wy =^> wy.CreatedBy)
echo                 .WithMany(e =^> e.CreatedWorkingYears)
echo                 .HasForeignKey(wy =^> wy.CreatedBy_Id)
echo                 .OnDelete(DeleteBehavior.SetNull);
echo.
echo             modelBuilder.Entity^<Studnet_Class_Section_year^>()
echo                 .HasOne(scsy =^> scsy.CreatedBy)
echo                 .WithMany(e =^> e.CreatedStudentClassSectionYears)
echo                 .HasForeignKey(scsy =^> scsy.CreatedBy_Id)
echo                 .OnDelete(DeleteBehavior.SetNull);
echo.
echo             // Configure table names if needed
echo             modelBuilder.Entity^<Studnet_Class_Section_year^>().ToTable("Student_Class_Section_Year");
echo             modelBuilder.Entity^<Employee_Department^>().ToTable("Employee_Department");
echo         }
echo     }
echo }
) > Models\ApplicationDbContext.cs

echo.
echo Models Created:
echo - AttendanceTypes.cs
echo - Working_Year.cs
echo - Studnet_Class_Section_year.cs (with Composite Primary Key)
echo - Section.cs
echo - AbsenceReasons.cs
echo - Classes.cs
echo - Competences.cs
echo - Employees.cs
echo - EmployeeTypes.cs
echo - Employee_Department.cs
echo - Department.cs
echo - Grades.cs
echo - Outcomes.cs
echo - Pictures.cs
echo - RequestExits.cs
echo - StudentAbsents.cs (with Composite Foreign Key)
echo - StudentAttendances.cs (with Composite Foreign Key)
echo - StudentGrades.cs
echo - Students.cs
echo - TaskEvaluations.cs
echo - ApplicationDbContext.cs (with Composite Key Configuration)
echo.
echo ============ COMPOSITE KEY CONFIGURATIONS ============
echo 1. Studnet_Class_Section_year: Composite Primary Key (Student_Id, Class_Id, Working_Year_Id, Section_id)
echo 2. StudentAbsents: Composite Foreign Key to Studnet_Class_Section_year
echo 3. StudentAttendances: Composite Foreign Key to Studnet_Class_Section_year
echo 4. Self-referencing Employee relationships configured
echo 5. Nullable foreign keys configured properly
echo.
echo Note: Install Entity Framework Core packages:
echo - Microsoft.EntityFrameworkCore
echo - Microsoft.EntityFrameworkCore.SqlServer
echo - Microsoft.EntityFrameworkCore.Tools
echo.
pause