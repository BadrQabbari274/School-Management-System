using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class TaskEvaluations
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDeleted { get; set; }
        public int OutcomeId { get; set; }
        public int StudentId { get; set; }

        // Navigation properties
        [ForeignKey("OutcomeId")]
        public virtual Outcomes Outcome { get; set; }

        [ForeignKey("StudentId")]
        public virtual Students Student { get; set; }

        public virtual ICollection<Pictures> Pictures { get; set; }
    }
}
