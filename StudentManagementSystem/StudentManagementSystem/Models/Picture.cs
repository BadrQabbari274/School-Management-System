using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    // Picture Model
    public class Picture
    {
        [Key]
        public int Id { get; set; }

        [StringLength(500)]
        public string FilePath { get; set; }

        public int? StudentId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? TaskId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [ForeignKey("TaskId")]
        public virtual TaskEvaluation Task { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual Employee CreatedByUser { get; set; }
    }
}
