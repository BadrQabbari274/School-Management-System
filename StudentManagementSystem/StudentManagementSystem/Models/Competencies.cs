using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Competencies
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Duration { get; set; }
        public int Section_Id { get; set; }
        public int Max_Outcome { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("Section_Id")]
        public virtual Section Section { get; set; }

        public virtual ICollection<Learning_Outcome> Learning_Outcomes { get; set; }
    }
}
