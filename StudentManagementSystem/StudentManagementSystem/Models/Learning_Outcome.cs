using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace StudentManagementSystem.Models
{
    public class Learning_Outcome
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Competency_Id { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy_Id { get; set; }
        public int Number { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("Competency_Id")]
        public virtual Competencies Competency { get; set; }

        public virtual ICollection<Evidence> Evidences { get; set; }
    }
}
