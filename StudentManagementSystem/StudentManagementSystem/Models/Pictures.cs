using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Pictures
    {
        [Key]
        public int Id { get; set; }
        public string FilePath { get; set; }
        public int StudentId { get; set; }
        public bool IsDeleted { get; set; }
        public int TaskId { get; set; }
        public int? CreatedBy_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees? CreatedBy { get; set; }

        [ForeignKey("StudentId")]
        public virtual Students Student { get; set; }

        [ForeignKey("TaskId")]
        public virtual TaskEvaluations Task { get; set; }
    }
}
