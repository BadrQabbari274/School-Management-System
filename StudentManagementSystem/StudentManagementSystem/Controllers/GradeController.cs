using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class GradeController : BaseController
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        // GET: Grade
        public async Task<IActionResult> Index()
        {
            try
            {
                var grades = await _gradeService.GetAllAcademicYearsAsync();
                return View(grades);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return View(new List<Grade>());
            }
        }

        // GET: Grade/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var grade = await _gradeService.GetAcademicYearByIdAsync(id);
                if (grade == null)
                {
                    SetErrorMessage("المرحلة الدراسية غير موجودة");
                    return RedirectToAction(nameof(Index));
                }
                return View(grade);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Grade/Create
        public IActionResult Create()
        {
            var grade = new Grade();
            return View(grade);
        }

        // POST: Grade/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Grade grade)
        {
            try
            {
             
                    // Set the created by user
                    grade.CreatedBy = GetCurrentUserId();
                    grade.Date = DateTime.Now;
                    grade.IsActive = true;

                    await _gradeService.CreateAcademicYearAsync(grade);
                    SetSuccessMessage("تم إضافة المرحلة الدراسية بنجاح");
                    return RedirectToAction(nameof(Index));
                

             
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في إضافة المرحلة الدراسية: {ex.Message}");
                return View(grade);
            }
        }

        // GET: Grade/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var grade = await _gradeService.GetAcademicYearByIdAsync(id);
                if (grade == null)
                {
                    SetErrorMessage("المرحلة الدراسية غير موجودة");
                    return RedirectToAction(nameof(Index));
                }
                return View(grade);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Grade/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Grade grade)
        {
            if (id != grade.Id)
            {
                SetErrorMessage("خطأ في البيانات المرسلة");
                return RedirectToAction(nameof(Index));
            }

            try
            {
               
                    // Get the existing grade to preserve some properties
                    var existingGrade = await _gradeService.GetAcademicYearByIdAsync(id);
                    if (existingGrade == null)
                    {
                        SetErrorMessage("المرحلة الدراسية غير موجودة");
                        return RedirectToAction(nameof(Index));
                    }

                    // Update only the allowed properties
                    existingGrade.Name = grade.Name;
                    existingGrade.IsActive = grade.IsActive;
                    // Keep the original creation date and creator

                    await _gradeService.UpdateAcademicYearAsync(existingGrade);
                    SetSuccessMessage("تم تحديث المرحلة الدراسية بنجاح");
                    return RedirectToAction(nameof(Index));
               
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحديث المرحلة الدراسية: {ex.Message}");
                return View(grade);
            }
        }

        // POST: Grade/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _gradeService.DeleteAcademicYearAsync(id);
                if (result)
                {
                    SetSuccessMessage("تم حذف المرحلة الدراسية بنجاح");
                }
                else
                {
                    SetErrorMessage("المرحلة الدراسية غير موجودة");
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في حذف المرحلة الدراسية: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Grade/GetActiveGrades
        [HttpGet]
        public async Task<IActionResult> GetActiveGrades()
        {
            try
            {
                var grades = await _gradeService.GetActiveAcademicYearsAsync();
                return Json(grades.Select(g => new {
                    Id = g.Id,
                    Name = g.Name,
                    Date = g.Date.ToString("dd/MM/yyyy")
                }));
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // POST: Grade/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var grade = await _gradeService.GetAcademicYearByIdAsync(id);
                if (grade == null)
                {
                    SetErrorMessage("المرحلة الدراسية غير موجودة");
                    return RedirectToAction(nameof(Index));
                }

                grade.IsActive = !grade.IsActive;
                await _gradeService.UpdateAcademicYearAsync(grade);

                string status = grade.IsActive ? "مفعلة" : "معطلة";
                SetSuccessMessage($"تم تغيير حالة المرحلة الدراسية إلى {status}");
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تغيير حالة المرحلة الدراسية: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}