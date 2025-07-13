using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace StudentManagementSystem.Models
{

    // Student Model
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;
        public int? CreatedBy { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int? ClassId { get; set; }

        // Navigation Properties
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

        // Collections
        public virtual ICollection<TaskEvaluation> TaskEvaluations { get; set; }
        public virtual ICollection<MajorAttendance> MajorAttendances { get; set; }
        public virtual ICollection<StudentAttendance> StudentAttendances { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
    }

}
