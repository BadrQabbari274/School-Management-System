using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Services.Interfaces;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class CompetenciesController : BaseController
    {
        private readonly ICompetenciesService _competenciesService;

        public CompetenciesController(ICompetenciesService competenciesService)
        {
            _competenciesService = competenciesService;
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
                    return RedirectToAction(nameof(Details), new { id = createdCompetency.Id });
                

    
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
                        return RedirectToAction(nameof(Details), new { id = model.Id });
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _competenciesService.DeleteCompetencyAsync(id);

                if (result)
                {
                   SetSuccessMessage("تم حذف الجدارة بنجاح") ;
                }
                else
                {
                    TempData["ErrorMessage"] = "الجدارة غير موجودة";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف الجدارة: " + ex.Message;
                return RedirectToAction(nameof(Index));
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