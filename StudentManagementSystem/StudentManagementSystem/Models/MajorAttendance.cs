using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    // Major Attendance Model
    public class MajorAttendance
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public bool Incentive { get; set; } = false;
        public DateTime Date { get; set; } = DateTime.Now;
        public int? StudentId { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        public int? AbsenceReasonId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [ForeignKey("AbsenceReasonId")]
        public virtual AbsenceReason AbsenceReason { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }
    }
}
