//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using StudentManagementSystem.Models;
//using StudentManagementSystem.Service.Interface;

//namespace StudentManagementSystem.Controllers
//{
//    [Authorize]
//    public class ClassRegistrationController : BaseController
//    {
//        private readonly IStudentService _studentService;
//        private readonly IGradeService _gradeService;
//        private readonly IClassService _classService;
//        private readonly ISectionService _sectionService;
//        private readonly IClassRegistrationService _classRegistrationService;

//        public ClassRegistrationController(
//            IStudentService studentService,
//            IGradeService gradeService,
//            IClassService classService,
//            ISectionService sectionService,
//            IClassRegistrationService classRegistrationService)
//        {
//            _studentService = studentService;
//            _gradeService = gradeService;
//            _classService = classService;
//            _sectionService = sectionService;
//            _classRegistrationService = classRegistrationService;
//        }

//        // GET: ClassRegistration
//        public async Task<IActionResult> Index()
//        {
//            try
//            {
//                var grades = await _gradeService.GetActiveAcademicYearsAsync();
//                return View(grades);
//            }
//            catch (Exception ex)
//            {
//                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
//                return View(new List<Grades>());
//            }
//        }

//        // GET: ClassRegistration/SelectClass?gradeId=1
//        public async Task<IActionResult> SelectClass(int gradeId)
//        {
//            try
//            {
//                var grade = await _gradeService.GetAcademicYearByIdAsync(gradeId);
//                if (grade == null)
//                {
//                    SetErrorMessage("الصف الدراسي غير موجود");
//                    return RedirectToAction(nameof(Index));
//                }

//                var classesWithStudentCount = await _classRegistrationService.GetClassesByGradeWithStudentCountAsync(gradeId);

//                ViewBag.Grade = grade;
//                return View(classesWithStudentCount);
//            }
//            catch (Exception ex)
//            {
//                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
//                return RedirectToAction(nameof(Index));
//            }
//        }

//        // GET: ClassRegistration/ManageStudents?gradeId=1&classId=1
//        public async Task<IActionResult> ManageStudents(int gradeId, int classId)
//        {
//            try
//            {
//                var grade = await _gradeService.GetAcademicYearByIdAsync(gradeId);
//                var classEntity = await _classService.GetClassByIdAsync(classId);

//                if (grade == null || classEntity == null)
//                {
//                    SetErrorMessage("البيانات المطلوبة غير موجودة");
//                    return RedirectToAction(nameof(Index));
//                }

//                ClassRegistrationViewModel viewModel;

//                if (grade.Name.ToLower() == "junior")
//                {
//                    viewModel = await _classRegistrationService.GetJuniorStudentsAsync(gradeId, classId);
//                }
//                else
//                {
//                    viewModel = await _classRegistrationService.GetAdvancedGradeStudentsAsync(gradeId, classId);
//                }

//                viewModel.Grade = grade;
//                viewModel.Class = classEntity;

//                return View(viewModel);
//            }
//            catch (Exception ex)
//            {
//                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
//                return RedirectToAction(nameof(Index));
//            }
//        }

//        // POST: ClassRegistration/SaveRegistration
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> SaveRegistration(int gradeId, int classId, List<int> selectedStudentIds, int? selectedSectionId = null)
//        {
//            if (selectedStudentIds == null || !selectedStudentIds.Any())
//            {
//                SetErrorMessage("يجب اختيار طالب واحد على الأقل");
//                return RedirectToAction(nameof(ManageStudents), new { gradeId, classId });
//            }

//            try
//            {
//                var grade = await _gradeService.GetAcademicYearByIdAsync(gradeId);
//                var classEntity = await _classService.GetClassByIdAsync(classId);

//                if (grade == null || classEntity == null)
//                {
//                    SetErrorMessage("البيانات المطلوبة غير موجودة");
//                    return RedirectToAction(nameof(Index));
//                }

//                // التحقق من سعة الفصل
//                if (classEntity.MaxStudents.HasValue && selectedStudentIds.Count > classEntity.MaxStudents.Value)
//                {
//                    SetErrorMessage($"تم تجاوز الحد الأقصى للطلاب في الفصل ({classEntity.MaxStudents.Value})");
//                    return RedirectToAction(nameof(ManageStudents), new { gradeId, classId });
//                }

//                var currentUserId = GetCurrentUserId();
//                bool success;

//                if (grade.Name.ToLower() == "junior")
//                {
//                    success = await _classRegistrationService.RegisterJuniorStudentsAsync(
//                        selectedStudentIds, classId, currentUserId);
//                }
//                else
//                {
//                    if (!selectedSectionId.HasValue)
//                    {
//                        SetErrorMessage("يجب اختيار القسم للصفوف المتقدمة");
//                        return RedirectToAction(nameof(ManageStudents), new { gradeId, classId });
//                    }

//                    success = await _classRegistrationService.RegisterAdvancedGradeStudentsAsync(
//                        selectedStudentIds, gradeId, classId, selectedSectionId.Value, currentUserId);
//                }

//                if (success)
//                {
//                    SetSuccessMessage($"تم تسجيل {selectedStudentIds.Count} طالب بنجاح في الفصل");
//                    return RedirectToAction(nameof(SelectClass), new { gradeId });
//                }
//                else
//                {
//                    SetErrorMessage("فشل في تسجيل الطلاب");
//                    return RedirectToAction(nameof(ManageStudents), new { gradeId, classId });
//                }
//            }
//            catch (Exception ex)
//            {
//                SetErrorMessage($"خطأ في حفظ البيانات: {ex.Message}");
//                return RedirectToAction(nameof(ManageStudents), new { gradeId, classId });
//            }
//        }

//        // GET: ClassRegistration/GetStudentsBySection?sectionId=1&gradeId=1&classId=1
//        [HttpGet]
//        public async Task<IActionResult> GetStudentsBySection(int sectionId, int gradeId, int classId)
//        {
//            try
//            {
//                var students = await _classRegistrationService.GetStudentsBySectionForAdvancedGradeAsync(sectionId, gradeId);

//                return Json(new
//                {
//                    success = true,
//                    students = students.Select(s => new
//                    {
//                        id = s.StudentId,
//                        name = s.StudentName,
//                        code = s.StudentCode,
//                        previousClass = s.PreviousClassName
//                    })
//                });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }
//    }
//}