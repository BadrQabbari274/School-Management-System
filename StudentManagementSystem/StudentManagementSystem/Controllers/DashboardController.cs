// Controllers/DashboardController.cs
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dashboardData = await _dashboardService.GetDashboardDataAsync();
                return View(dashboardData);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "حدث خطأ في تحميل البيانات: " + ex.Message;
                return View(new Models.ViewModels.DashboardViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentStatistics()
        {
            try
            {
                var statistics = await _dashboardService.GetStudentStatisticsAsync();
                return Json(statistics);
            }
            catch (Exception ex)
            {
                return Json(new { error = "فشل في تحميل الإحصائيات" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMonthlyData()
        {
            try
            {
                var monthlyData = await _dashboardService.GetMonthlyStudentDataAsync();
                return Json(monthlyData);
            }
            catch (Exception ex)
            {
                return Json(new { error = "فشل في تحميل البيانات الشهرية" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetWeeklyData()
        {
            try
            {
                var weeklyData = await _dashboardService.GetWeeklyStudentDataAsync();
                return Json(weeklyData);
            }
            catch (Exception ex)
            {
                return Json(new { error = "فشل في تحميل البيانات الأسبوعية" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> RefreshDashboard()
        {
            try
            {
                var dashboardData = await _dashboardService.GetDashboardDataAsync();
                return PartialView("_DashboardContent", dashboardData);
            }
            catch (Exception ex)
            {
                return Json(new { error = "فشل في تحديث لوحة التحكم" });
            }
        }
    }
}