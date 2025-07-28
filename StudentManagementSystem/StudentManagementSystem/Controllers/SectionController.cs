using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class SectionController : BaseController
    {
        private readonly ISectionService _sectionService;
        private readonly IDepartmentService _departmentService;

        public SectionController(ISectionService sectionService, IDepartmentService departmentService)
        {
            _sectionService = sectionService;
            _departmentService = departmentService;
        }

        // GET: Section
        public async Task<IActionResult> Index()
        {
            try
            {
                var sections = await _sectionService.GetAllSectionsAsync();
                return View(sections);
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء تحميل البيانات: " + ex.Message);
                return View(new List<Section>());
            }
        }

        // GET: Section/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                SetErrorMessage("لم يتم تحديد القسم المطلوب");
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var section = await _sectionService.GetSectionByIdAsync(id.Value);
                if (section == null)
                {
                    SetErrorMessage("القسم المطلوب غير موجود");
                    return RedirectToAction(nameof(Index));
                }

                return View(section);
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء تحميل البيانات: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Section/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                await LoadDepartmentsDropdown1();
                return View();
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء تحميل البيانات: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Section/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Section section)
        {
            try
            {
                
                    // Check if section name is unique within the department
                    var isUnique = await _sectionService.IsSectionNameUniqueAsync(
                        section.Name_Of_Section, section.Department_Id);

                    if (!isUnique)
                    {
                        ModelState.AddModelError("Name_Of_Section",
                            "اسم الشعبة موجود بالفعل في هذا القسم");
                        await LoadDepartmentsDropdown(section.Department_Id);
                        return View(section);
                    }

                    section.CreatedBy_Id = GetCurrentUserId();
                    var createdSection = await _sectionService.CreateSectionAsync(section);

                    if (createdSection != null)
                    {
                        SetSuccessMessage("تم إضافة الشعبة بنجاح");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        SetErrorMessage("فشل في إضافة الشعبة");
                    }
            
                return RedirectToAction("index","section");
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء إضافة الشعبة: " + ex.Message);
                await LoadDepartmentsDropdown(section.Department_Id);
                return View(section);
            }
        }

        // GET: Section/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                SetErrorMessage("لم يتم تحديد الشعبة المطلوبة");
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var section = await _sectionService.GetSectionByIdAsync(id.Value);
                if (section == null)
                {
                    SetErrorMessage("الشعبة المطلوبة غير موجودة");
                    return RedirectToAction(nameof(Index));
                }

                await LoadDepartmentsDropdown1(section.Department_Id);
                return View(section);
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء تحميل البيانات: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Section/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Section section)
        {
            if (id != section.Id)
            {
                SetErrorMessage("البيانات غير صحيحة");
                return RedirectToAction(nameof(Index));
            }

            try
            {
                    // Check if section name is unique within the department (excluding current section)
                    var isUnique = await _sectionService.IsSectionNameUniqueAsync(
                        section.Name_Of_Section, section.Department_Id, section.Id);

                    if (!isUnique)
                    {
                        ModelState.AddModelError("Name_Of_Section",
                            "اسم الشعبة موجود بالفعل في هذا القسم");
                        await LoadDepartmentsDropdown(section.Department_Id);
                        return View(section);
                    }

                    var updatedSection = await _sectionService.UpdateSectionAsync(section);

                    if (updatedSection != null)
                    {
                        SetSuccessMessage("تم تحديث الشعبة بنجاح");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        SetErrorMessage("فشل في تحديث الشعبة");
                    }
                

                return RedirectToAction("index","section");
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء تحديث الشعبة: " + ex.Message);
                await LoadDepartmentsDropdown(section.Department_Id);
                return View(section);
            }
        }

        // GET: Section/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        SetErrorMessage("لم يتم تحديد الشعبة المطلوبة");
        //        return RedirectToAction(nameof(Index));
        //    }

        //    try
        //    {
        //        var section = await _sectionService.GetSectionByIdAsync(id.Value);
        //        if (section == null)
        //        {
        //            SetErrorMessage("الشعبة المطلوبة غير موجودة");
        //            return RedirectToAction(nameof(Index));
        //        }

        //        return View(section);
        //    }
        //    catch (Exception ex)
        //    {
        //        SetErrorMessage("حدث خطأ أثناء تحميل البيانات: " + ex.Message);
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        // POST: Section/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _sectionService.DeleteSectionAsync(id);

                if (result)
                {
                    SetSuccessMessage("تم حذف الشعبة بنجاح");
                }
                else
                {
                    SetErrorMessage("فشل في حذف الشعبة");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء حذف الشعبة: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // Helper method to load departments dropdown
        private async Task LoadDepartmentsDropdown1(int? selectedDepartmentId = null)
        {
            try
            {
                var departments = await _departmentService.GetActiveDepartmentsAsync();
                ViewBag.Department_Id = new SelectList(departments, "Id", "Name", selectedDepartmentId);
            }
            catch (Exception ex)
            {
                ViewBag.SectionId = new SelectList(new List<SelectListItem>(), "Value", "Text");
            }
        }
        private async Task LoadDepartmentsDropdown(int? selectedDepartmentId = null)
        {
            var departments = await _departmentService.GetActiveDepartmentsAsync();
            ViewBag.Department_Id = new SelectList(departments, "Id", "Name_Of_Department", selectedDepartmentId);
        }


        // API method to get sections by department (for AJAX calls)
        [HttpGet]
        public async Task<JsonResult> GetSectionsByDepartment(int departmentId)
        {
            try
            {
                var sections = await _sectionService.GetSectionsByDepartmentAsync(departmentId);
                var result = sections.Select(s => new { id = s.Id, name = s.Name_Of_Section });
                return Json(result);
            }
            catch (Exception)
            {
                return Json(new List<object>());
            }
        }
    }
}