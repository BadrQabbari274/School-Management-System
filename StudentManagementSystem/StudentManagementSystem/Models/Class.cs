using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    // Class Model
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;
        public int? CreatedBy { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int? FieldId { get; set; }

        // Navigation Properties
        [ForeignKey("CreatedBy")]
        public virtual Employee CreatedByUser { get; set; }

        [ForeignKey("FieldId")]
        public virtual Field Field { get; set; }

        // Collections
        public virtual ICollection<Student> Students { get; set; }
    }
}
