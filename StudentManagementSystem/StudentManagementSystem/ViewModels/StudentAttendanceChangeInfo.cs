using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class StudentAttendanceChangeInfo
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public bool PreviousStatus { get; set; } // الحالة السابقة (يومي)
        public bool NewStatus { get; set; } // الحالة الجديدة (يومي)
        public bool FieldStatus { get; set; } = true; // الحالة الميدانية الجديدة (افتراضي حاضر)
        public string ChangeType { get; set; } // "FromAbsentToPresent" أو "FromPresentToAbsent"
        public int? AbsenceReasonId { get; set; }
        public string AbsenceReason { get; set; }
        public string CustomReason { get; set; }
    }

}
