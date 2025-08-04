// Services/DashboardService.cs
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Services.Interfaces;
using StudentManagementSystem.ViewModels;
using System.Globalization;

namespace StudentManagementSystem.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var statistics = await GetStudentStatisticsAsync();
            var monthlyData = await GetMonthlyStudentDataAsync();
            var weeklyData = await GetWeeklyStudentDataAsync();
            var dashboardCards = await GetDashboardCardsAsync();

            return new DashboardViewModel
            {  
                TotalStudentsCount = statistics.TotalStudents,
                MonthlyData = monthlyData,
                WeeklyData = weeklyData,
                DashboardCards = dashboardCards
            };
        }

        public async Task<StudentStatistics> GetStudentStatisticsAsync()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var lastMonth = currentMonth == 1 ? 12 : currentMonth - 1;
            var lastMonthYear = currentMonth == 1 ? currentYear - 1 : currentYear;

            var activeStudents = await _context.Students.CountAsync(s => s.IsActive);
            var inactiveStudents = await _context.Students.CountAsync(s => !s.IsActive);
            var totalStudents = await _context.Students.CountAsync();
            var studentsThisMonth = await _context.Students
                .CountAsync(s => s.Date.Month == currentMonth && s.Date.Year == currentYear);

            // Previous month data for percentage calculation
            var activeStudentsLastMonth = await _context.Students
                .CountAsync(s => s.IsActive && s.Date.Month <= lastMonth && s.Date.Year <= lastMonthYear);
            var inactiveStudentsLastMonth = await _context.Students
                .CountAsync(s => !s.IsActive && s.Date.Month <= lastMonth && s.Date.Year <= lastMonthYear);
            var totalStudentsLastMonth = await _context.Students
                .CountAsync(s => s.Date.Month <= lastMonth && s.Date.Year <= lastMonthYear);
            var studentsLastMonth = await _context.Students
                .CountAsync(s => s.Date.Month == lastMonth && s.Date.Year == lastMonthYear);

            return new StudentStatistics
            {
                ActiveStudents = activeStudents,
                InactiveStudents = inactiveStudents,
                TotalStudents = totalStudents,
                StudentsThisMonth = studentsThisMonth,
                ActivePercentageChange = CalculatePercentageChange(activeStudents, activeStudentsLastMonth),
                InactivePercentageChange = CalculatePercentageChange(inactiveStudents, inactiveStudentsLastMonth),
                TotalPercentageChange = CalculatePercentageChange(totalStudents, totalStudentsLastMonth),
                ThisMonthPercentageChange = CalculatePercentageChange(studentsThisMonth, studentsLastMonth)
            };
        }

        public async Task<List<MonthlyStudentData>> GetMonthlyStudentDataAsync()
        {
            var currentYear = DateTime.Now.Year;
            var monthlyData = new List<MonthlyStudentData>();

            var arabicMonths = new Dictionary<int, string>
            {
                {1, "يناير"}, {2, "فبراير"}, {3, "مارس"}, {4, "أبريل"},
                {5, "مايو"}, {6, "يونيو"}, {7, "يوليو"}, {8, "أغسطس"},
                {9, "سبتمبر"}, {10, "أكتوبر"}, {11, "نوفمبر"}, {12, "ديسمبر"}
            };

            for (int month = 1; month <= 12; month++)
            {
                var thisMonthCount = await _context.Students
                    .CountAsync(s => s.Date.Month == month && s.Date.Year == currentYear);

                var lastMonthCount = await _context.Students
                    .CountAsync(s => s.Date.Month == month && s.Date.Year == currentYear - 1);

                monthlyData.Add(new MonthlyStudentData
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                    MonthArabic = arabicMonths[month],
                    StudentCount = thisMonthCount,
                    ThisMonth = thisMonthCount,
                    LastMonth = lastMonthCount
                });
            }

            return monthlyData;
        }

        public async Task<List<DailyStudentData>> GetWeeklyStudentDataAsync()
        {
            var arabicDays = new Dictionary<DayOfWeek, string>
            {
                {DayOfWeek.Sunday, "الأحد"}, {DayOfWeek.Monday, "الاثنين"},
                {DayOfWeek.Tuesday, "الثلاثاء"}, {DayOfWeek.Wednesday, "الأربعاء"},
                {DayOfWeek.Thursday, "الخميس"}, {DayOfWeek.Friday, "الجمعة"},
                {DayOfWeek.Saturday, "السبت"}
            };

            var weeklyData = new List<DailyStudentData>();
            var students = await _context.Students.ToListAsync();
            var totalStudents = students.Count;

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                var dayStudents = students.Count(s => s.Date.DayOfWeek == day);
                var percentage = totalStudents > 0 ? (double)dayStudents / totalStudents * 100 : 0;

                weeklyData.Add(new DailyStudentData
                {
                    Day = day.ToString(),
                    DayArabic = arabicDays[day],
                    StudentCount = dayStudents,
                    Percentage = Math.Round(percentage, 1)
                });
            }

            return weeklyData;
        }

        public async Task<List<DashboardCard>> GetDashboardCardsAsync()
        {
            var cards = new List<DashboardCard>();

            cards.Add(new DashboardCard
            {
                Title = "Departments",
                TitleArabic = "الأقسام",
                Count = await _context.Departments.CountAsync(d => d.IsActive),
                Icon = "🏢",
                Color = "bg-primary",
                Controller = "Department",
                Action = "Index"
            });

            cards.Add(new DashboardCard
            {
                Title = "Grades",
                TitleArabic = "السنوات الدراسية",
                Count = await _context.Grades.CountAsync(g => g.IsActive),
                Icon = "⭐",
                Color = "bg-success",
                Controller = "Grade",
                Action = "Index"
            });

            cards.Add(new DashboardCard
            {
                Title = "Sections",
                TitleArabic = "الشعب",
                Count = await _context.Sections.CountAsync(s => s.IsActive),
                Icon = "📚",
                Color = "bg-warning",
                Controller = "Section",
                Action = "Index"
            });

            cards.Add(new DashboardCard
            {
                Title = "Classes",
                TitleArabic = "الفصول",
                Count = await _context.Classes.CountAsync(c => c.IsActive),
                Icon = "🎓",
                Color = "bg-info",
                Controller = "Class",
                Action = "Index"
            });

            cards.Add(new DashboardCard
            {
                Title = "Working Years",
                TitleArabic = "سنوات العمل",
                Count = await _context.Working_Years.CountAsync(w => w.IsActive),
                Icon = "📅",
                Color = "bg-secondary",
                Controller = "WorkingYear",
                Action = "Index"
            });

            return cards;
        }

        public async Task<int> GetActiveStudentsCountAsync()
        {
            return await _context.Students.CountAsync(s => s.IsActive);
        }

        public async Task<int> GetInactiveStudentsCountAsync()
        {
            return await _context.Students.CountAsync(s => !s.IsActive);
        }

        public async Task<int> GetTotalStudentsCountAsync()
        {
            return await _context.Students.CountAsync();
        }

        public async Task<int> GetStudentsAddedThisMonthAsync()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            return await _context.Students
                .CountAsync(s => s.Date.Month == currentMonth && s.Date.Year == currentYear);
        }

        private double CalculatePercentageChange(int current, int previous)
        {
            if (previous == 0) return current > 0 ? 100 : 0;
            return Math.Round(((double)(current - previous) / previous) * 100, 1);
        }

        private async Task<int> GetEntityCountAsync(string entityName)
        {
            return entityName switch
            {
                "Departments" => await _context.Departments.CountAsync(d => d.IsActive),
                "Grades" => await _context.Grades.CountAsync(g => g.IsActive),
                "Sections" => await _context.Sections.CountAsync(s => s.IsActive),
                "Classes" => await _context.Classes.CountAsync(c => c.IsActive),
                "WorkingYears" => await _context.Working_Years.CountAsync(w => w.IsActive),
                _ => 0
            };
        }
    }
}