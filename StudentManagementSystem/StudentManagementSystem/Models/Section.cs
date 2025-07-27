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
