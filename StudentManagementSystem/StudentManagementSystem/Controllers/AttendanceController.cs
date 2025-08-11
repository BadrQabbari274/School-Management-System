using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Service.Implementation;
using StudentManagementSystem.Service.Interface;
using StudentManagementSystem.ViewModels;
namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class AttendanceController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IGradeService _gradeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AttendanceController(IStudentService studentService, IGradeService gradeService, IWebHostEnvironment webHostEnvironment)
        {
            _studentService = studentService;
            _gradeService = gradeService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new GradeSelectionViewModel();
            var grades = await _gradeService.GetActiveAcademicYearsAsync();
            viewModel.GradesList = new SelectList(grades, "Id", "Name");
            return View(viewModel);
        }
        // إضافة هذه الأكشنز إلى AttendanceController

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkUnregisteredAsFieldAbsent(int classId)
        {
            try
            {
                bool success = await _studentService.MarkUnregisteredStudentsAsFieldAbsentAsync(classId, GetCurrentUserId());

                if (success)
                {
                    SetSuccessMessage("تم تسجيل الطلاب غير المتسجلين كغياب ميداني بنجاح");
                }
                else
                {
                    SetErrorMessage("حدث خطأ أثناء تسجيل الغياب الميداني");
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ غير متوقع");
            }

            return RedirectToAction("Field", new { classId = classId });
        }

        [HttpGet]
        public async Task<IActionResult> Statistics()
        {
            var viewModel = new StatisticsRequestViewModel();
            var grades = await _gradeService.GetActiveAcademicYearsAsync();
            ViewBag.Grades = new SelectList(grades, "Id", "Name");
            ViewBag.Classes = new SelectList(new List<object>(), "Id", "Name");
            ViewBag.Students = new SelectList(new List<object>(), "Id", "Name");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Statistics(StatisticsRequestViewModel model)
        {
            var grades = await _gradeService.GetActiveAcademicYearsAsync();
            ViewBag.Grades = new SelectList(grades, "Id", "Name", model.ClassId);

            if (model.ClassId.HasValue)
            {
                var classes = await _studentService.GetClassesByGradeAsync(model.ClassId.Value);
                ViewBag.Classes = new SelectList(classes, "Id", "Name");
            }

            if (ModelState.IsValid)
            {
                switch (model.ReportType)
                {
                    case "Class":
                        if (model.ClassId.HasValue)
                        {
                            var classStats = await _studentService.GetClassStatisticsAsync(model.ClassId.Value, model.StartDate, model.EndDate);
                            ViewBag.ClassStatistics = classStats;
                        }
                        break;

                    case "Student":
                        if (model.StudentId.HasValue)
                        {
                            var studentStats = await _studentService.GetStudentStatisticsAsync(model.StudentId.Value, model.StartDate, model.EndDate);
                            ViewBag.StudentStatistics = studentStats;
                        }
                        break;

                    case "AllStudents":
                        if (model.ClassId.HasValue)
                        {
                            var allStudentsStats = await _studentService.GetAllStudentsStatisticsInClassAsync(model.ClassId.Value, model.StartDate, model.EndDate);
                            ViewBag.AllStudentsStatistics = allStudentsStats;
                        }
                        break;
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetClassesByGrade(int gradeId)
        {
            var classes = await _studentService.GetClassesByGradeAsync(gradeId);
            return Json(classes.Select(c => new { Id = c.Id, Name = c.Name }));
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentsByClass(int classId)
        {
            var students = await _studentService.GetAllStudentsStatisticsInClassAsync(classId, DateTime.Now.AddMonths(-1), DateTime.Now);
            return Json(students.Select(s => new { Id = s.Student.Id, Name = s.Student.Name }));
        }

        [HttpGet]
        public async Task<IActionResult> ExportClassStatistics(int classId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var excelData = await _studentService.ExportClassStatisticsToExcelAsync(classId, startDate, endDate);

                if (excelData == null)
                {
                    SetErrorMessage("فشل في تصدير البيانات");
                    return RedirectToAction("Statistics");
                }

                var classInfo = await _studentService.GetClassesByGradeAsync(1); // استبدل بالطريقة المناسبة للحصول على اسم الفصل
                var fileName = $"احصائيات_الفصل_{classId}_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.xlsx";

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء تصدير البيانات");
                return RedirectToAction("Statistics");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportStudentStatistics(int studentId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var excelData = await _studentService.ExportStudentStatisticsToExcelAsync(studentId, startDate, endDate);

                if (excelData == null)
                {
                    SetErrorMessage("فشل في تصدير البيانات");
                    return RedirectToAction("Statistics");
                }

                var fileName = $"احصائيات_الطالب_{studentId}_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.xlsx";

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء تصدير البيانات");
                return RedirectToAction("Statistics");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportAllStudentsStatistics(int classId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var excelData = await _studentService.ExportAllStudentsStatisticsToExcelAsync(classId, startDate, endDate);

                if (excelData == null)
                {
                    SetErrorMessage("فشل في تصدير البيانات");
                    return RedirectToAction("Statistics");
                }

                var fileName = $"احصائيات_جميع_الطلاب_الفصل_{classId}_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.xlsx";

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء تصدير البيانات");
                return RedirectToAction("Statistics");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(GradeSelectionViewModel viewModel)
        {
            var grades = await _gradeService.GetActiveAcademicYearsAsync();
            viewModel.GradesList = new SelectList(grades, "Id", "Name", viewModel.SelectedGradeId);
            if (viewModel.SelectedGradeId.HasValue && viewModel.SelectedGradeId > 0)
            {
                viewModel.ClassesResult = await _studentService.GetClassesByGradeAsync(viewModel.SelectedGradeId.Value);
            }
            return View(viewModel);
        }
        public async Task<IActionResult> Normal(int classId)
        {
            var classwithstudent = await _studentService.GetStudentsAsync(classId);
            return View(classwithstudent);
        }
        public async Task<IActionResult> Field(int classId)
        {
            var classwithstudent = await _studentService.GetStudentsFieldAsync(classId);
            // التحقق من وجود رسالة خطأ (عدم وجود حضور يومي)
            if (classwithstudent != null && !string.IsNullOrEmpty(classwithstudent.ErrorMessage))
            {
                SetErrorMessage(classwithstudent.ErrorMessage);
                // توجيه المستخدم لصفحة الحضور اليومي
                return RedirectToAction("Normal", new { classId = classId });
            }
            return View(classwithstudent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAttendanceField(AttendanceViewModel model, DateTime attendanceDate)
        {
            bool success = await _studentService.SaveAttendanceFieldAsync(model, attendanceDate, GetCurrentUserId());
            if (success)
            {
                SetSuccessMessage("تم حفظ الحضور الميداني بنجاح");
                return RedirectToAction("Index");
            }
            else
            {
                SetErrorMessage("حدث خطأ أثناء حفظ الحضور الميداني");
                return View("Field", model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAttendance(AttendanceViewModel model, DateTime attendanceDate)
        {
            bool success = await _studentService.SaveAttendanceAsync(model, attendanceDate, GetCurrentUserId());
            if (success)
            {
                SetSuccessMessage("تم حفظ الحضور اليومي بنجاح");
                return RedirectToAction("Index");
            }
            else
            {
                SetErrorMessage("حدث خطأ أثناء حفظ الحضور اليومي");
                return View("Normal", model);
            }
        }
    }
}