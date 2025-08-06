using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class StudentFieldChangeViewModel
    {
        public Students Student { get; set; }
        public bool PreviousDailyStatus { get; set; }
        public bool NewDailyStatus { get; set; }
        public bool CurrentFieldStatus { get; set; }
        public string? CurrentFieldReason { get; set; }
        public int? CurrentFieldReasonId { get; set; }
        public string? CustomFieldReason { get; set; }
        public bool IsFromAttendanceToAbsence { get; set; }
        public bool IsFromAbsenceToAttendance { get; set; }
    }

}
