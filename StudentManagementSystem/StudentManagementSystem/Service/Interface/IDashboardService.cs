// Services/Interfaces/IDashboardService.cs
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
        Task<StudentStatistics> GetStudentStatisticsAsync();
        Task<List<MonthlyStudentData>> GetMonthlyStudentDataAsync();
        Task<List<DailyStudentData>> GetWeeklyStudentDataAsync();
        Task<List<DashboardCard>> GetDashboardCardsAsync();
        Task<int> GetActiveStudentsCountAsync();
        Task<int> GetInactiveStudentsCountAsync();
        Task<int> GetTotalStudentsCountAsync();
        Task<int> GetStudentsAddedThisMonthAsync();
        Task<int> GetTodayAbsentStudentsCountAsync();
        Task<int> GetTodayPresentStudentsCountAsync();
        Task<List<AbsentStudentsViewModel>> GetTodayAbsentStudentsDetailsAsync();
        Task<List<object>> GetActiveGradesAsync();
        Task<List<object>> GetClassesByGradeAsync(int gradeId);
    }
}