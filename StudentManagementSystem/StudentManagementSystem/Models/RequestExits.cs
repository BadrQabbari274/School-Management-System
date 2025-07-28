using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class RequestExits
    {
        [Key]
        public int Id { get; set; }
        public string Reason { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }
        public int AttendanceId { get; set; }
        public TimeSpan ExitTime { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("AttendanceId")]
        public virtual StudentAttendances Attendance { get; set; }
    }
}
