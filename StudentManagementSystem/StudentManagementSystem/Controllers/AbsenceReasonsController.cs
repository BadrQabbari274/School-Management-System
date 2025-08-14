using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Implementation;
using StudentManagementSystem.Service.Interface;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class AbsenceReasonsController : BaseController
    {
        private readonly IAbsenceReasonsService2 _absenceReasonsService;

        public AbsenceReasonsController(IAbsenceReasonsService2 absenceReasonsService)
        {
            _absenceReasonsService = absenceReasonsService;
        }

        // GET: AbsenceReasons
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var absenceReasons = await _absenceReasonsService.GetAllAsync();
                return View(absenceReasons);
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء تحميل أسباب الغياب");
                return View(new List<AbsenceReasons>());
            }
        }

        // GET: AbsenceReasons/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var absenceReason = await _absenceReasonsService.GetByIdAsync(id);
                if (absenceReason == null)
                {
                    SetErrorMessage("سبب الغياب غير موجود");
                    return RedirectToAction(nameof(Index));
                }
                return View(absenceReason);
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء تحميل تفاصيل سبب الغياب");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: AbsenceReasons/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new AbsenceReasonViewModel());
        }

        // POST: AbsenceReasons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AbsenceReasonViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var absenceReason = new AbsenceReasons
                    {
                        Name = model.Name,
                        CreatedBy_Id = GetCurrentUserId(),
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    };

                    var result = await _absenceReasonsService.CreateAsync(absenceReason);
                    if (result)
                    {
                        SetSuccessMessage("تم إضافة سبب الغياب بنجاح");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        SetErrorMessage("حدث خطأ أثناء إضافة سبب الغياب");
                    }
                }
                catch (Exception ex)
                {
                    SetErrorMessage("حدث خطأ غير متوقع");
                }
            }
            return View(model);
        }

        // GET: AbsenceReasons/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var absenceReason = await _absenceReasonsService.GetByIdAsync(id);
                if (absenceReason == null)
                {
                    SetErrorMessage("سبب الغياب غير موجود");
                    return RedirectToAction(nameof(Index));
                }

                var model = new AbsenceReasonViewModel
                {
                    Id = absenceReason.Id,
                    Name = absenceReason.Name
                };

                return View(model);
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء تحميل سبب الغياب للتعديل");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: AbsenceReasons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AbsenceReasonViewModel model)
        {
            if (id != model.Id)
            {
                SetErrorMessage("حدث خطأ في البيانات المرسلة");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var absenceReason = await _absenceReasonsService.GetByIdAsync(id);
                    if (absenceReason == null)
                    {
                        SetErrorMessage("سبب الغياب غير موجود");
                        return RedirectToAction(nameof(Index));
                    }

                    absenceReason.Name = model.Name;

                    var result = await _absenceReasonsService.UpdateAsync(absenceReason);
                    if (result)
                    {
                        SetSuccessMessage("تم تحديث سبب الغياب بنجاح");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        SetErrorMessage("حدث خطأ أثناء تحديث سبب الغياب");
                    }
                }
                catch (Exception ex)
                {
                    SetErrorMessage("حدث خطأ غير متوقع");
                }
            }
            return View(model);
        }

        // POST: AbsenceReasons/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _absenceReasonsService.DeleteAsync(id);
                if (result)
                {
                    SetSuccessMessage("تم حذف سبب الغياب بنجاح");
                }
                else
                {
                    SetErrorMessage("حدث خطأ أثناء حذف سبب الغياب");
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage("لا يمكن حذف سبب الغياب لأنه مرتبط ببيانات أخرى");
            }

            return RedirectToAction(nameof(Index));
        }

        /*
         
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == id)
                {
                    SetSuccessMessage("لا يمكن حذف المستخدم الحالي");
                    return RedirectToAction("ManageUsers");
                }

                var result = await _userService.DeleteUserAsync(id);
                if (result)
                {
                    SetSuccessMessage ("تم حذف المستخدم بنجاح");
                }
                else
                {
                    SetSuccessMessage("فشل في حذف المستخدم");
                }
            }
            catch (Exception ex)
            {
                SetSuccessMessage("حدث خطأ أثناء حذف المستخدم");
            }

            return RedirectToAction("ManageUsers");
        }
         */

        // GET: API endpoint for search functionality
        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            try
            {
                var absenceReasons = await _absenceReasonsService.SearchAsync(searchTerm);
                return Json(absenceReasons.Select(ar => new
                {
                    Id = ar.Id,
                    Name = ar.Name,
                    CreatedDate = ar.CreatedDate.ToString("dd/MM/yyyy"),
                    CreatedBy = ar.CreatedBy?.Name ?? "غير محدد"
                }));
            }
            catch (Exception ex)
            {
                return Json(new { error = "حدث خطأ أثناء البحث" });
            }
        }
    }
}