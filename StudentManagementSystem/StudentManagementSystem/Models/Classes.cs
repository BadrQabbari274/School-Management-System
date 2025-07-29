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
 
        public int? MaxStudents { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        public virtual ICollection<Student_Class_Section_Year> StudentClassSectionYears { get; set; }
    }
}
