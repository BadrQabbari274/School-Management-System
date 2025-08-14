using ClosedXML.Excel;
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
        private readonly IAttendanceService _attendanceService;
        private readonly IGradeService _gradeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AttendanceController(IStudentService studentService, IGradeService gradeService, IWebHostEnvironment webHostEnvironment, IAttendanceService attendanceService)
        {
            _studentService = studentService;
            _gradeService = gradeService;
            _webHostEnvironment = webHostEnvironment;
            _attendanceService = attendanceService;
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
        [HttpGet]
        [Route("Attendance/Report")]
        public async Task<IActionResult> AttendanceReport()
        {
            var viewModel = new AttendanceReportViewModel();
            var grades = await _gradeService.GetActiveAcademicYearsAsync();
            viewModel.GradesList = new SelectList(grades, "Id", "Name");

            // جلب أنواع الحضور/الغياب
            var attendanceTypes = await _attendanceService.GetAllAttendanceTypesAsync();
            viewModel.AttendanceTypesList = new SelectList(attendanceTypes, "Id", "Name");

            // تعيين التواريخ الافتراضية
            viewModel.StartDate = DateTime.Now.AddDays(-30);
            viewModel.EndDate = DateTime.Now;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Attendance/Report")]
        public async Task<IActionResult> AttendanceReport(AttendanceReportViewModel viewModel)
        {
            // إعادة تحميل قائمة الصفوف
            var grades = await _gradeService.GetActiveAcademicYearsAsync();
            viewModel.GradesList = new SelectList(grades, "Id", "Name", viewModel.SelectedGradeId);

            // إعادة تحميل قائمة أنواع الحضور/الغياب
            var attendanceTypes = await _attendanceService.GetAllAttendanceTypesAsync();
            viewModel.AttendanceTypesList = new SelectList(attendanceTypes, "Id", "Name", viewModel.SelectedAttendanceTypeId);

            if (viewModel.SelectedGradeId.HasValue && viewModel.SelectedGradeId > 0)
            {
                // جلب الفصول للصف المحدد
                viewModel.ClassesResult = await _attendanceService.GetClassesByGradeAsync(viewModel.SelectedGradeId.Value);
            }

            if (viewModel.SelectedClassId.HasValue && viewModel.SelectedClassId > 0)
            {
                try
                {
                    // جلب تقرير الحضور والغياب للفصل
                    viewModel.AttendanceReport = await _attendanceService.GetClassAttendanceReportAsync(
                        viewModel.SelectedClassId.Value,
                        viewModel.StartDate ?? DateTime.Now.AddDays(-30),
                        viewModel.EndDate ?? DateTime.Now,
                        viewModel.SelectedAttendanceTypeId);

                    // جلب حضور اليوم
                    viewModel.TodayAttendance = await _attendanceService.GetTodayClassAttendanceAsync(viewModel.SelectedClassId.Value);

                    TempData["Success"] = "تم تحميل التقرير بنجاح";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "حدث خطأ أثناء تحميل التقرير: " + ex.Message;
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentAttendanceDetails(int studentId, int classId, DateTime startDate, DateTime endDate, int? attendanceTypeId = null)
        {
            try
            {
                var studentDetails = await _attendanceService.GetStudentAttendanceDetailsAsync(studentId, classId, startDate, endDate, attendanceTypeId);
                return Json(new { success = true, data = studentDetails });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportToExcel(int classId, DateTime startDate, DateTime endDate, int? attendanceTypeId = null)
        {
            try
            {
                var report = await _attendanceService.GetClassAttendanceReportAsync(classId, startDate, endDate, attendanceTypeId);

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("تقرير الحضور والغياب");

                // إعداد الهيدر
                worksheet.Cell(1, 1).Value = "اسم الطالب";
                worksheet.Cell(1, 2).Value = "كود الطالب";
                worksheet.Cell(1, 3).Value = "الفصل";
                worksheet.Cell(1, 4).Value = "عدد أيام الحضور";
                worksheet.Cell(1, 5).Value = "عدد أيام الغياب";
                worksheet.Cell(1, 6).Value = "نسبة الحضور %";
                worksheet.Cell(1, 7).Value = "أسباب الغياب";
                worksheet.Cell(1, 8).Value = "نوع الغياب";
                worksheet.Cell(1, 9).Value = "من تاريخ";
                worksheet.Cell(1, 10).Value = "إلى تاريخ";

                // تنسيق الهيدر
                var headerRange = worksheet.Range(1, 1, 1, 10);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

                // إضافة البيانات
                for (int i = 0; i < report.Students.Count; i++)
                {
                    var student = report.Students[i];
                    var row = i + 2;

                    worksheet.Cell(row, 1).Value = student.StudentName;
                    worksheet.Cell(row, 2).Value = student.StudentCode;
                    worksheet.Cell(row, 3).Value = student.ClassName;
                    worksheet.Cell(row, 4).Value = student.PresentDays;
                    worksheet.Cell(row, 5).Value = student.AbsentDays;
                    worksheet.Cell(row, 6).Value = Math.Round(student.AttendancePercentage, 2);
                    worksheet.Cell(row, 7).Value = string.Join(", ", student.AbsenceReasons);
                    worksheet.Cell(row, 8).Value = string.Join(", ", student.AbsenceTypes);
                    worksheet.Cell(row, 9).Value = startDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 10).Value = endDate.ToString("yyyy-MM-dd");
                }

                // تعديل عرض الأعمدة
                worksheet.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                var fileName = $"تقرير_الحضور_والغياب_{DateTime.Now:yyyyMMdd}.xlsx";
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء تصدير التقرير: " + ex.Message;
                return RedirectToAction("AttendanceReport");
            }
        }
    }
}