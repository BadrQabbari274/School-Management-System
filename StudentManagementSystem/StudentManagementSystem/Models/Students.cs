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
        public string Adress { get; set; }
        public string? Code { get; set; }
        public string? Date_of_birth { get; set; }
        public string Email { get; set; }
        public string? Governate { get; set; }
        public string Jop_of_Father { get; set; }
        public string Jop_of_Mother { get; set; }
        public string Natrual_Id { get; set; }
        public string Phone_Number { get; set; }
        public string Phone_Number_Father { get; set; }
        public string Phone_Number_Mother { get; set; }
        public string? Picture_Profile { get; set; }
        public string? birth_Certificate { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        public virtual ICollection<Pictures> Pictures { get; set; }
        public virtual ICollection<StudentGrades> StudentGrades { get; set; }
        public virtual ICollection<TaskEvaluations> TaskEvaluations { get; set; }
        public virtual ICollection<Student_Class_Section_Year> StudentClassSectionYears { get; set; }
        
    }
}
