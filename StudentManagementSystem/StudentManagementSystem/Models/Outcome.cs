using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    // Outcome Model
    public class Outcome
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;
        public int? CompId { get; set; }

        // Navigation Properties
        [ForeignKey("CompId")]
        public virtual Competence Competence { get; set; }

        // Collections
        public virtual ICollection<TaskEvaluation> TaskEvaluations { get; set; }
    }
}
