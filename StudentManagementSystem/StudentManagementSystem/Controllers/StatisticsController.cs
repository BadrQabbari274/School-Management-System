//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using StudentManagementSystem.Service.Interface;
//using StudentManagementSystem.ViewModels;
//using System;
//using System.Threading.Tasks;

//namespace StudentManagementSystem.Controllers
//{
//    [Authorize]
//    public class StatisticsController : BaseController
//    {
//        private readonly IStudentService _studentService;
//        private readonly IExcelExportService _excelExportService;
//        private readonly IGradeService _gradeService;

//        public StatisticsController(
//            IStudentService studentService,
//            IExcelExportService excelExportService,
//            IGradeService gradeService)
//        {
//            _studentService = studentService;
//            _excelExportService = excelExportService;
//            _gradeService = gradeService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index()
//        {
//            var viewModel = new StatisticsViewModel();
//            var grades = await _gradeService.GetActiveAcademicYearsAsync();
//            viewModel.GradesList = new SelectList(grades, "Id", "Name");
//            return View(viewModel);
//        }

//        [HttpPost]
//        public async Task<IActionResult> GetClassStatistics(StatisticsViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                var grades = await _gradeService.GetActiveAcademicYearsAsync();
//                model.GradesList = new SelectList(grades, "Id", "Name");
//                return View("Index", model);
//            }

//            var request = new AttendanceStatisticsRequest
//            {
//                ClassId = model.SelectedClassId.Value,
//                StartDate = model.StartDate.Value,
//                EndDate = model.EndDate.Value
//            };

//            var statistics = await _studentService.GetClassAttendanceStatistics(request);
//            return View("StatisticsResult", statistics);
//        }

//        [HttpPost]
//        public async Task<IActionResult> ExportStudentsStatistics(StatisticsViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                var grades = await _gradeService.GetActiveAcademicYearsAsync();
//                model.GradesList = new SelectList(grades, "Id", "Name");
//                return View("Index", model);
//            }

//            var request = new AttendanceStatisticsRequest
//            {
//                ClassId = model.SelectedClassId.Value,
//                StartDate = model.StartDate.Value,
//                EndDate = model.EndDate.Value
//            };

//            var studentsStats = await _studentService.GetStudentsAttendanceStatistics(request);
//            var excelBytes = await _excelExportService.ExportStudentStatisticsToExcel(studentsStats);

//            return File(excelBytes,
//                      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
//                      $"إحصائيات_الطلاب_{DateTime.Now:yyyyMMdd}.xlsx");
//        }
//    }
//}