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
        public int? CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Email { get; set; }
        public DateTime? Join_Date { get; set; }

        // Navigation properties
        [ForeignKey("RoleId")]
        public virtual EmployeeTypes Role { get; set; }

        [ForeignKey("CreatedBy_Id")]
        public virtual Employees? CreatedBy { get; set; }

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
