namespace StudentManagementSystem.ViewModels
{
    public class ClassAttendanceStatistics
    {
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public Dictionary<string, int> AbsenceReasons { get; set; } = new Dictionary<string, int>();
        public double AttendancePercentage => TotalDays > 0 ? (PresentDays * 100.0) / TotalDays : 0;
    }
}
