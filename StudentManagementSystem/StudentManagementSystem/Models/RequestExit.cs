using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class RequestExit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Reason { get; set; }

        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int? AttendanceId { get; set; }

        // Navigation Properties
        [ForeignKey("CreatedBy")]
        public virtual Employee CreatedByUser { get; set; }

        [ForeignKey("AttendanceId")]
        public virtual StudentAttendance Attendance { get; set; }
    }


}
