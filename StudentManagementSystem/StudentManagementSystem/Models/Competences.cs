using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Competences
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Duration { get; set; }
        public int Department_Id { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("Department_Id")]
        public virtual Department Department { get; set; }

        public virtual ICollection<Outcomes> Outcomes { get; set; }
    }
}
