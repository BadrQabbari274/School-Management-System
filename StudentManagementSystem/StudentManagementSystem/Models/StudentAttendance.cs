using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    // Student Attendance Model
    public class StudentAttendance
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        public string State { get; set; }

        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int? StudentId { get; set; }

        // Navigation Properties
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
    }

}
