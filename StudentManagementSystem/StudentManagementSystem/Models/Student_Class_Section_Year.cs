using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Student_Class_Section_Year
    {
        [Key, Column(Order = 0)]
        public int Student_Id { get; set; }

        [Key, Column(Order = 1)] 
        public int Working_Year_Id { get; set; }

        [Key, Column(Order = 2)]
        public int Section_id { get; set; }

        public bool IsActive { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }
        public int? Class_Id { get; set; }
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
        public virtual ICollection<Student_Tasks> Student_Tasks { get; set; }
        public virtual ICollection<StudentAttendances> StudentAttendances { get; set; }
    }
}
