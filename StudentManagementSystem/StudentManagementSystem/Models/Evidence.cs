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

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("Outcome_Id")]
        public virtual Learning_Outcome Learning_Outcome { get; set; }

        public virtual ICollection<Student_Evidence> Student_Evidences { get; set; }
    }
}
