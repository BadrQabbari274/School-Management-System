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

            if (classwithstudent != null && !string.IsNullOrEmpty(classwithstudent.ErrorMessage))
            {
                SetErrorMessage(classwithstudent.ErrorMessage);
                return RedirectToAction("Normal", new { classId = classId });
            }
            return View(classwithstudent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAttendance(AttendanceViewModel model, DateTime attendanceDate)
        {
            var result = await _studentService.SaveAttendanceAsync(model, attendanceDate, GetCurrentUserId());

            if (result.success)
            {
                // إذا كان هناك طلاب تغيرت حالتهم
                if (result.changedStudentIds.Any())
                {
                    // توجيه لصفحة تأكيد التغييرات الميدانية
                    TempData["ChangedStudentIds"] = string.Join(",", result.changedStudentIds);
                    TempData["ClassId"] = model.Class.Id;
                    return RedirectToAction("ConfirmFieldChanges");
                }

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
        public async Task<IActionResult> ConfirmFieldChanges()
        {
            var changedStudentIdsString = TempData["ChangedStudentIds"]?.ToString();
            var classId = TempData["ClassId"] != null ? Convert.ToInt32(TempData["ClassId"]) : 0;

            if (string.IsNullOrEmpty(changedStudentIdsString) || classId == 0)
            {
                SetErrorMessage("لا توجد تغييرات تتطلب تأكيد");
                return RedirectToAction("Index");
            }

            var changedStudentIds = changedStudentIdsString.Split(',')
                .Select(id => Convert.ToInt32(id))
                .ToList();

            var studentsRequiringUpdate = await _studentService
                .GetStudentsRequiringFieldUpdateAsync(classId, changedStudentIds);

            var classInfo = await _studentService.GetClassesByGradeAsync(1); // احصل على معلومات الفصل
            var currentClass = classInfo.FirstOrDefault(c => c.Id == classId);

            var viewModel = new FieldUpdateConfirmationViewModel
            {
                Class = currentClass,
                StudentsRequiringUpdate = studentsRequiringUpdate,
                Message = "تم تحديث الحضور اليومي للطلاب التاليين. يرجى تحديد حالة الحضور الميداني لهم:"
            };

            // الاحتفاظ بالبيانات للمرحلة التالية
            TempData.Keep("ChangedStudentIds");
            TempData.Keep("ClassId");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmFieldChanges(FieldUpdateConfirmationViewModel model)
        {
            var changedStudentIdsString = TempData["ChangedStudentIds"]?.ToString();
            var classId = TempData["ClassId"] != null ? Convert.ToInt32(TempData["ClassId"]) : 0;

            if (string.IsNullOrEmpty(changedStudentIdsString) || classId == 0)
            {
                SetErrorMessage("حدث خطأ في معالجة البيانات");
                return RedirectToAction("Index");
            }

            var changedStudentIds = changedStudentIdsString.Split(',')
                .Select(id => Convert.ToInt32(id))
                .ToList();

            var newStatuses = model.StudentsRequiringUpdate.Select(s => s.CurrentFieldStatus).ToList();
            var studentIds = model.StudentsRequiringUpdate.Select(s => s.Student.Id).ToList();

            var success = await _studentService.ConfirmFieldAttendanceChangesAsync(
                classId, studentIds, newStatuses, GetCurrentUserId());

            if (success)
            {
                SetSuccessMessage("تم تحديث الحضور الميداني بنجاح");
            }
            else
            {
                SetErrorMessage("حدث خطأ أثناء تحديث الحضور الميداني");
            }

            return RedirectToAction("Index");
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
    }
}