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

        // Status: 0 = Pending, 1 = Approved, 2 = Rejected
        public int Status { get; set; } = 0;

        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int? AttendanceId { get; set; }
        public int? StudentId { get; set; }

        // Time of exit request
        public TimeSpan ExitTime { get; set; }

        // Expected return time (optional)
        public TimeSpan? ExpectedReturnTime { get; set; }

        // Actual return time (filled when student returns)
        public TimeSpan? ActualReturnTime { get; set; }

        // Who approved/rejected the request
        public int? ProcessedBy { get; set; }
        public DateTime? ProcessedDate { get; set; }

        [StringLength(255)]
        public string ProcessingNotes { get; set; }

        // Navigation Properties
        [ForeignKey("CreatedBy")]
        public virtual Employee CreatedByUser { get; set; }

        [ForeignKey("AttendanceId")]
        public virtual StudentAttendance Attendance { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [ForeignKey("ProcessedBy")]
        public virtual Employee ProcessedByUser { get; set; }
    }

}
