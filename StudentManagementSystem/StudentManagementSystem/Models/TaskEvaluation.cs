using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    // Task Evaluation Model
    public class TaskEvaluation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public bool IsDeleted { get; set; } = false;
        public int? OutcomeId { get; set; }
        public int? StudentId { get; set; }

        // Navigation Properties
        [ForeignKey("OutcomeId")]
        public virtual Outcome Outcome { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        // Collections
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
