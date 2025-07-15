using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    // Field Model
    public class Field
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? GradeId { get; set; } // تم تغيير من AcademicYearId إلى GradeId

        // Navigation Properties
        [ForeignKey("CreatedBy")]
        public virtual Employee CreatedByUser { get; set; }

        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; } // تم تغيير من AcademicYear إلى Grade

        // Collections
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Competence> Competences { get; set; }
        public virtual ICollection<FieldEmployee> FieldEmployees { get; set; }
    }
}
