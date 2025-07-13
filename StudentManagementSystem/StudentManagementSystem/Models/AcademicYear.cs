using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    // Academic Year Model
    public class AcademicYear
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(10)]
        public string Code { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        // Collections
        public virtual ICollection<Field> Fields { get; set; }
    }
}
