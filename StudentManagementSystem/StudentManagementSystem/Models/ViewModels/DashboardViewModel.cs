// Models/ViewModels/DashboardViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int ActiveStudentsCount { get; set; }
        public int InactiveStudentsCount { get; set; }
        public int TotalStudentsCount { get; set; }
        public int StudentsAddedThisMonth { get; set; }

        public List<MonthlyStudentData> MonthlyData { get; set; } = new List<MonthlyStudentData>();
        public List<DailyStudentData> WeeklyData { get; set; } = new List<DailyStudentData>();

        // Cards for the 5 pages
        public List<DashboardCard> DashboardCards { get; set; } = new List<DashboardCard>();
    }

    public class MonthlyStudentData
    {
        public string Month { get; set; }
        public string MonthArabic { get; set; }
        public int StudentCount { get; set; }
        public int ThisMonth { get; set; }
        public int LastMonth { get; set; }
    }

    public class DailyStudentData
    {
        public string Day { get; set; }
        public string DayArabic { get; set; }
        public int StudentCount { get; set; }
        public double Percentage { get; set; }
    }

    public class DashboardCard
    {
        public string Title { get; set; }
        public string TitleArabic { get; set; }
        public int Count { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }

    public class StudentStatistics
    {
        public int ActiveStudents { get; set; }
        public int InactiveStudents { get; set; }
        public int TotalStudents { get; set; }
        public int StudentsThisMonth { get; set; }
        public double ActivePercentageChange { get; set; }
        public double InactivePercentageChange { get; set; }
        public double TotalPercentageChange { get; set; }
        public double ThisMonthPercentageChange { get; set; }
    }
}