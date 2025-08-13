using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Student_Tasks
    {
        [Key]
        public int Id { get; set; }
        //public string Title { get; set; }
        public bool IsActive { get; set; }
        public int Task_Id { get; set; }
        public string Image_Path { get; set; }
        public int Try_Id { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Class_Id { get; set; }
        // Composite key reference fields for Student_Class_Section_Year   
        public int Student_Id { get; set; }
        public int Working_Year_Id { get; set; }
        public int Section_id { get; set; }
        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("Task_Id")]
        public virtual Tasks Tasks { get; set; }

        [ForeignKey("Try_Id")]
        public virtual Try Try { get; set; }

        [ForeignKey("Class_Id")]
        public virtual Classes Class { get; set; }

        // Composite foreign key reference to Student_Class_Section_Year
        [ForeignKey("Student_Id,Working_Year_Id,Section_id")]
        public virtual Student_Class_Section_Year StudentClassSectionYear { get; set; }
    }
}
