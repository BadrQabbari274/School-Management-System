using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CreatedBy_Id { get; set; }
        public int Competencies_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }
        [ForeignKey("Competencies_Id")]
        public virtual Competencies Competencies { get; set; }
        public virtual ICollection<Evidence> Evidences { get; set; }
        public virtual ICollection<Student_Tasks> Student_Tasks { get; set; }
    }
}
