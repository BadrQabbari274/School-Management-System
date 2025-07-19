namespace StudentManagementSystem.ViewModels
{
    public class StudentFieldAttendanceItem
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public bool IsDailyPresent { get; set; }
        public bool IsFieldPresent { get; set; }
        public bool IsAbsent { get; set; }
        public int? AbsenceReasonId { get; set; }
        public string CustomAbsenceReason { get; set; }
        public bool WithoutIncentive { get; set; }
        public bool CanDisableAttendance { get; set; }
    }

}
