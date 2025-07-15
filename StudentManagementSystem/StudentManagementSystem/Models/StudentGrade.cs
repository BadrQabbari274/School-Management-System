using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class StudentGrade
    {
        [Key]
        public int Id { get; set; }

        public int? GradeId { get; set; }
        public int? StudentId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
    }

}
