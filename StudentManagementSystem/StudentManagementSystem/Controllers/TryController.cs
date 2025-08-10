using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Services.Interfaces;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class TryController : BaseController
    {
        private readonly ITryService _tryService;

        public TryController(ITryService tryService)
        {
            _tryService = tryService;
        }

        // GET: Try
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10,
            string searchTerm = null, bool? isActiveFilter = null)
        {
            try
            {
                var model = await _tryService.GetAllTriesAsync(page, pageSize,
                    searchTerm, isActiveFilter);

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل البيانات: " + ex.Message;
                return View(new TryIndexViewModel());
            }
        }

        // GET: Try/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "المعرف غير صحيح";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var tryItem = await _tryService.GetTryDetailsAsync(id.Value);
                if (tryItem == null)
                {
                    TempData["ErrorMessage"] = "العنصر غير موجود";
                    return RedirectToAction(nameof(Index));
                }

                return View(tryItem);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل التفاصيل: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Try/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new TryViewModel
                {
                    IsActive = true
                };

                await _tryService.LoadSelectListsAsync(model);
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل نموذج الإنشاء: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Try/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TryViewModel model)
        {
            try
            {
                // Check if name is unique
                if (!await _tryService.IsTryNameUniqueAsync(model.Name))
                {
                    ModelState.AddModelError("Name", "الاسم موجود بالفعل");
                }

                    var currentUserId = GetCurrentUserId();
                    var createdTry = await _tryService.CreateTryAsync(model, currentUserId);

                    SetSuccessMessage("تم إنشاء العنصر بنجاح");
                    return RedirectToAction(nameof(Details), new { id = createdTry.Id });
           
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء إنشاء العنصر: " + ex.Message;
            }

            await _tryService.LoadSelectListsAsync(model);
            return View(model);
        }

        // GET: Try/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "المعرف غير صحيح";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var tryItem = await _tryService.GetTryByIdAsync(id.Value);
                if (tryItem == null)
                {
                    TempData["ErrorMessage"] = "العنصر غير موجود";
                    return RedirectToAction(nameof(Index));
                }

                await _tryService.LoadSelectListsAsync(tryItem);
                return View(tryItem);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل نموذج التعديل: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Try/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TryViewModel model)
        {
            if (id != model.Id)
            {
                TempData["ErrorMessage"] = "المعرف غير متطابق";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Check if name is unique (excluding current record)
                if (!await _tryService.IsTryNameUniqueAsync(model.Name, model.Id))
                {
                    ModelState.AddModelError("Name", "الاسم موجود بالفعل");
                }

           
                    var currentUserId = GetCurrentUserId();
                    var updatedTry = await _tryService.UpdateTryAsync(model, currentUserId);

                    if (updatedTry != null)
                    {
                        SetSuccessMessage("تم تحديث العنصر بنجاح");
                        return RedirectToAction(nameof(Details), new { id = model.Id });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "العنصر غير موجود";
                        return RedirectToAction(nameof(Index));
                    }
                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث العنصر: " + ex.Message;
            }

            await _tryService.LoadSelectListsAsync(model);
            return View(model);
        }

        // POST: Try/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _tryService.DeleteTryAsync(id);

                if (result)
                {
                    SetSuccessMessage("تم حذف العنصر بنجاح");
                }
                else
                {
                    TempData["ErrorMessage"] = "العنصر غير موجود";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف العنصر: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // AJAX: Check if name is unique
        [HttpPost]
        public async Task<JsonResult> IsNameUnique(string name, int? id)
        {
            try
            {
                var isUnique = await _tryService.IsTryNameUniqueAsync(name, id);
                return Json(isUnique);
            }
            catch
            {
                return Json(false);
            }
        }
    }
}