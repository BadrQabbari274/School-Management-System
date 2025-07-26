using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class StudentGrades
    {
        [Key]
        public int Id { get; set; }
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public int Working_Year_Id { get; set; }

        // Navigation properties
        [ForeignKey("GradeId")]
        public virtual Grades Grade { get; set; }

        [ForeignKey("StudentId")]
        public virtual Students Student { get; set; }

        [ForeignKey("Working_Year_Id")]
        public virtual Working_Year WorkingYear { get; set; }
    }
}
