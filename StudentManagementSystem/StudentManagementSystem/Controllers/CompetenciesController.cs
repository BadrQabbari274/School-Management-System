 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Service.Interface;
using StudentManagementSystem.Services.Interfaces;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class CompetenciesController : BaseController
    {
        private readonly ICompetenciesService _competenciesService;
        private readonly IGradeService _gradeService;
        private readonly IStudentService _studentService;

        public CompetenciesController(ICompetenciesService competenciesService,IGradeService gradeService,IStudentService studentService)
        {
            _competenciesService = competenciesService;
            _gradeService = gradeService;
            _studentService = studentService;
        }
        // في CompetenciesController

        [HttpGet]
        public async Task<IActionResult> SelectClass()
        {
            var viewModel = new GradeSelectionViewModel();
            var grades = await _gradeService.GetActiveAcademicYearsAsync();
            viewModel.GradesList = new SelectList(grades, "Id", "Name");

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectClass(GradeSelectionViewModel viewModel)
        {
            var grades = await _gradeService.GetActiveAcademicYearsAsync();
            viewModel.GradesList = new SelectList(grades, "Id", "Name", viewModel.SelectedGradeId);
            if (viewModel.SelectedGradeId.HasValue && viewModel.SelectedGradeId > 0)
            {
                viewModel.ClassesResult = await _studentService.GetClassesByGradeAsync(viewModel.SelectedGradeId.Value);
            }
            return View(viewModel);
        }
        // في CompetenciesController

        [HttpGet]
        public async Task<IActionResult> SelectCompetencies(int classId)
        {
            try
            {
                var competenciesData = await _competenciesService.GetCompetencies_Outcame_Evidence(classId);
                var tries = await _competenciesService.GetAllTriesAsync();

                var viewModel = new CompetenciesSelectionViewModel
                {
                    ClassId = classId,
                    CompetenciesList = competenciesData.Competencies.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList(),
                    TrysList = tries.Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.Name
                    }).ToList(),
                    LearningOutcomes = competenciesData.LearningOutcomes
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ في تحميل البيانات");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetLearningOutcomes(int competencyId)
        {
            try
            {
                var outcomes = await _competenciesService.GetLearningOutcomesByCompetencyId(competencyId);

                var outcomesList = outcomes.Select(o => new
                {
                    id = o.Id,
                    name = o.Name
                }).ToList();

                return Json(new { success = true, data = outcomesList });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ في تحميل النواتج" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetEvidences(int outcomeId)
        {
            try
            {
                var evidences = await _competenciesService.GetEvidencesByOutcomeId(outcomeId);

                var evidencesList = evidences.Select(e => new
                {
                    id = e.Id,
                    name = e.Name,
                    isPractical = e.Ispractical
                }).ToList();

                return Json(new { success = true, data = evidencesList });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ في تحميل الأدلة" });
            }
        }
        public async Task<IActionResult> Evaluate(CompetenciesSelectionViewModel model)
        {
            try
            {
                // التحقق من صحة البيانات
                if (!model.SelectedCompetencyId.HasValue ||
                    !model.SelectedOutcomeId.HasValue  ||
                    !model.SelectedTryId.HasValue)
                {
                    TempData["Error"] = "يرجى تحديد جميع البيانات المطلوبة";
                    return RedirectToAction("Index");
                }

                // الحصول على قائمة الطلاب
                var students = await _competenciesService.GetStudentToEvidences(model);

                if (students == null || !students.Any())
                {
                    TempData["Error"] = "لا توجد بيانات طلاب للفصل المحدد";
                    return RedirectToAction("Index");
                }

                // ترتيب الطلاب حسب الجنس والاسم
                var sortedStudents = students
                    .OrderBy(s => GetGenderFromNationalId(s.Student.Natrual_Id)) // البنين أولاً (0)، ثم البنات (1)
                    .ThenBy(s => s.Student.Name) // ترتيب أبجدي للاسم
                    .ToList();

                // تمرير البيانات للعرض
                ViewBag.ClassId = model.ClassId;
                ViewBag.CompetencyId = model.SelectedCompetencyId.Value;
                ViewBag.OutcomeId = model.SelectedOutcomeId.Value;
                //ViewBag.EvidenceId = model.SelectedEvidenceId.Value;
                ViewBag.TryId = model.SelectedTryId.Value;

                return View(sortedStudents);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء تحميل بيانات الطلاب";
                return RedirectToAction("Index");
            }
        }

        private int GetGenderFromNationalId(string nationalId)
        {
            if (string.IsNullOrEmpty(nationalId) || nationalId.Length < 14)
                return 1; // نفترض أنثى لو الرقم غير صحيح

            if (int.TryParse(nationalId[12].ToString(), out int genderDigit))
            {
                // لو فردي => ذكر (0)، لو زوجي => أنثى (1)
                return (genderDigit % 2 != 0) ? 0 : 1;
            }

            return 1; // افتراض أنثى لو حصل خطأ
        }


        // GET: Competencies
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10,
            string searchTerm = null, int? sectionFilter = null, bool? isActiveFilter = null)
        {
            try
            {
                var model = await _competenciesService.GetAllCompetenciesAsync(page, pageSize,
                    searchTerm, sectionFilter, isActiveFilter);

                // Load sections for filter dropdown
                var sections = await _competenciesService.GetActiveSectionsAsync();
                ViewBag.Sections = sections.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name_Of_Section
                }).ToList();

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل البيانات: " + ex.Message;
                return View(new CompetenciesIndexViewModel());
            }
        }

        // GET: Competencies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "معرف الجدارة غير صحيح";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var competency = await _competenciesService.GetCompetencyDetailsAsync(id.Value);
                if (competency == null)
                {
                    TempData["ErrorMessage"] = "الجدارة غير موجودة";
                    return RedirectToAction(nameof(Index));
                }

                return View(competency);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل تفاصيل الجدارة: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Competencies/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new CompetenciesViewModel
                {
                    IsActive = true
                };

                await _competenciesService.LoadSelectListsAsync(model);
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل نموذج الإنشاء: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Competencies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompetenciesViewModel model)
        {
            try
            {
                // Check if name is unique
                if (!await _competenciesService.IsCompetencyNameUniqueAsync(model.Name))
                {
                    ModelState.AddModelError("Name", "اسم الجدارة موجود بالفعل");
                }

    
                    // Get current user ID (you should implement this based on your authentication system)
                    var currentUserId = GetCurrentUserId();

                    var createdCompetency = await _competenciesService.CreateCompetencyAsync(model, currentUserId);

                    TempData["SuccessMessage"] = "تم إنشاء الجدارة بنجاح";
                    return RedirectToAction(nameof(Index), new { id = createdCompetency.Id });
                

    
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء إنشاء الجدارة: " + ex.Message;
                await _competenciesService.LoadSelectListsAsync(model);
                return View(model);
            }
        }

        // GET: Competencies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "معرف الجدارة غير صحيح";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var competency = await _competenciesService.GetCompetencyByIdAsync(id.Value);
                if (competency == null)
                {
                    TempData["ErrorMessage"] = "الجدارة غير موجودة";
                    return RedirectToAction(nameof(Index));
                }

                await _competenciesService.LoadSelectListsAsync(competency);
                return View(competency);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل نموذج التعديل: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Competencies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompetenciesViewModel model)
        {
            if (id != model.Id)
            {
                TempData["ErrorMessage"] = "معرف الجدارة غير متطابق";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Check if name is unique (excluding current record)
                if (!await _competenciesService.IsCompetencyNameUniqueAsync(model.Name, model.Id))
                {
                    ModelState.AddModelError("Name", "اسم الجدارة موجود بالفعل");
                }

   
                    var currentUserId = GetCurrentUserId();
                    var updatedCompetency = await _competenciesService.UpdateCompetencyAsync(model, currentUserId);

                    if (updatedCompetency != null)
                    {
                        TempData["SuccessMessage"] = "تم تحديث الجدارة بنجاح";
                        return RedirectToAction(nameof(Index), new { id = model.Id });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "الجدارة غير موجودة";
                        return RedirectToAction(nameof(Index));
                    }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث الجدارة: " + ex.Message;
                await _competenciesService.LoadSelectListsAsync(model);
                return View(model);
            }
        }

        // GET: Competencies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "معرف الجدارة غير صحيح";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var competency = await _competenciesService.GetCompetencyDetailsAsync(id.Value);
                if (competency == null)
                {
                    TempData["ErrorMessage"] = "الجدارة غير موجودة";
                    return RedirectToAction(nameof(Index));
                }

                return View(competency);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل صفحة الحذف: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Competencies/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            try
            {
                var result = await _competenciesService.DeleteCompetencyAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "تم حذف الجدارة بنجاح" });
                }
                else
                {
                    return Json(new { success = false, message = "الجدارة غير موجودة" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء حذف الجدارة: " + ex.Message });
            }
        }

        // AJAX: Check if competency name is unique
        [HttpPost]
        public async Task<JsonResult> IsNameUnique(string name, int? id)
        {
            try
            {
                var isUnique = await _competenciesService.IsCompetencyNameUniqueAsync(name, id);
                return Json(isUnique);
            }
            catch
            {
                return Json(false);
            }
        }


        // AJAX: Get sections by department (if needed for future enhancements)
        [HttpGet]
        public async Task<JsonResult> GetSectionsByDepartment(int departmentId)
        {
            try
            {
                // This would be implemented if you need to filter sections by department
                var sections = await _competenciesService.GetActiveSectionsAsync();
                var result = sections.Select(s => new { id = s.Id, name = s.Name_Of_Section });
                return Json(result);
            }
            catch
            {
                return Json(new { error = "خطأ في تحميل الأقسام" });
            }
        }
    }
}