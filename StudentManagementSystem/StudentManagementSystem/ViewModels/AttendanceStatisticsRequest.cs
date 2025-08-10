using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class ClassStatisticsViewModel
    {
        public Classes Class { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public int TotalStudents { get; set; }
        public int TotalPossibleAttendances { get; set; } // TotalDays * TotalStudents
        public int TotalActualAttendances { get; set; }
        public int TotalAbsences { get; set; }
        public decimal AttendancePercentage { get; set; }
        public decimal AbsencePercentage { get; set; }
        public List<AbsenceReasonStatistic> TopAbsenceReasons { get; set; } = new List<AbsenceReasonStatistic>();
        public List<DailyAttendanceStatistic> DailyStatistics { get; set; } = new List<DailyAttendanceStatistic>();
        public List<StudentStatisticsViewModel> StudentStatistics { get; set; } = new List<StudentStatisticsViewModel>();
    }

    public class StudentStatisticsViewModel
    {
        public Students Student { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ClassName { get; set; }
        public int TotalPossibleDays { get; set; }
        public int AttendanceDays { get; set; }
        public int AbsenceDays { get; set; }
        public decimal AttendancePercentage { get; set; }
        public decimal AbsencePercentage { get; set; }
        public List<AbsenceReasonStatistic> AbsenceReasons { get; set; } = new List<AbsenceReasonStatistic>();
        public string MostCommonAbsenceReason { get; set; }
        public int MostCommonAbsenceReasonCount { get; set; }
    }

    public class AbsenceReasonStatistic
    {
        public string ReasonName { get; set; }
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    public class DailyAttendanceStatistic
    {
        public DateTime Date { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public decimal AttendancePercentage { get; set; }
    }

    public class StatisticsRequestViewModel
    {
        public int? ClassId { get; set; }
        public int? StudentId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime EndDate { get; set; } = DateTime.Now;
        public string ReportType { get; set; } = "Class"; // Class, Student, AllStudents
    }
}