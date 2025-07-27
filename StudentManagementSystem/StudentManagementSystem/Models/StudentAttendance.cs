using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    // Student Attendance Model
    public class StudentAttendance
    {
        [Key]
        public int Id { get; set; }

        public int? StudentGrade { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int? CreatedBy { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public int? StudentId { get; set; }

        // Additional notes field for any extra information
        [StringLength(500)]
        public string? Notes { get; set; }

        // Field to indicate if this is a field attendance record
        public bool IsFieldAttendance { get; set; } = false;
        public bool? Without_Incentive { get; set; } = true;

        // Navigation Properties
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual Employee CreatedByUser { get; set; }
    }
}
