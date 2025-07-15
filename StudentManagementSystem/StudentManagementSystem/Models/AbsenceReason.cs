using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    // Absence Reason Model
    public class AbsenceReason
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("CreatedBy")]
        public virtual Employee CreatedByUser { get; set; }

        // Collections
        public virtual ICollection<StudentAbsent> MajorAttendances { get; set; }
    }

}
