using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{

    public class StudentAbsent
    {
        [Key]
        public int Id { get; set; }

        public int? StudentGrade { get; set; }
        public int? AttendanceType { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int? StudentId { get; set; }
        public int? TeacherId { get; set; }
        public int? AbsenceReasonId { get; set; }

        // Navigation Properties
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [ForeignKey("TeacherId")]
        public virtual Employee Teacher { get; set; }

        [ForeignKey("AbsenceReasonId")]
        public virtual AbsenceReason AbsenceReason { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual Employee CreatedByUser { get; set; }
    }
}
