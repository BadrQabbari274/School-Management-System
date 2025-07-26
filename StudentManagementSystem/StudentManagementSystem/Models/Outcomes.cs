using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Outcomes
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int CompId { get; set; }

        // Navigation properties
        [ForeignKey("CompId")]
        public virtual Competences Competence { get; set; }
        public virtual ICollection<TaskEvaluations> TaskEvaluations { get; set; }
    }
}
