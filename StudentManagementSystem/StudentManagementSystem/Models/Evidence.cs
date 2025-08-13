using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Evidence
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Ispractical { get; set; }
        public bool IsActive { get; set; }
        public int Outcome_Id { get; set; }
        public int CreatedBy_Id { get; set; }
        public int Number { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? Task_Id { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }
        // Navigation properties
        [ForeignKey("Task_Id")]
        public virtual Tasks? Tasks { get; set; }

        [ForeignKey("Outcome_Id")]
        public virtual Learning_Outcome Learning_Outcome { get; set; }

        public virtual ICollection<Student_Tasks> Student_Evidences { get; set; }
    }
    public class Tasks
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }
        public virtual ICollection<Evidence> Evidences { get; set; }
        public virtual ICollection<Student_Tasks> Student_Tasks { get; set; }
    }
}

