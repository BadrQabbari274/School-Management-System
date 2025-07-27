using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
using System.Security.Claims;

namespace StudentManagementSystem.Controllers
{
    public class FieldController : BaseController
    {
        private readonly IDepartmentService _fieldService;
        private readonly IGradeService _gradeService;
        private readonly IUserService _employeeService;

        public FieldController(
            IDepartmentService fieldService,
            IGradeService gradeService,
            IUserService employeeService)
        {
            _fieldService = fieldService;
            _gradeService = gradeService;
            _employeeService = employeeService;
        }

        // GET: Field
        public async Task<IActionResult> Index()
        {
            try
            {
                var fields = await _fieldService.GetAllFieldsAsync();
                return View(fields);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء جلب البيانات: {ex.Message}");
                return View(new List<Field>());
            }
        }

        // GET: Field/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var field = await _fieldService.GetFieldByIdAsync(id);
                if (field == null)
                {
                    SetErrorMessage("القسم المطلوب غير موجود");
                    return RedirectToAction(nameof(Index));
                }
                return View(field);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء جلب البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Field/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                await PopulateDropDownLists();
                return View();
            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء تحضير البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Field/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Field field)
        {
            try
            {
       
                    field.CreatedBy = GetCurrentUserId();
                    field.CreatedDate = DateTime.Now;
                    field.IsActive = true;

                    var createdField = await _fieldService.CreateFieldAsync(field);
                    SetSuccessMessage("تم إنشاء القسم بنجاح");
                    return RedirectToAction(nameof(Index));
                

            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء إنشاء القسم: {ex.Message}");
                await PopulateDropDownLists(field.GradeId);
                return View(field);
            }
        }

        // GET: Field/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var field = await _fieldService.GetFieldByIdAsync(id);
                if (field == null)
                {
                    SetErrorMessage("القسم المطلوب غير موجود");
                    return RedirectToAction(nameof(Index));
                }

                await PopulateDropDownLists(field.GradeId);
                return View(field);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء جلب البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Field/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Field field)
        {
            if (id != field.Id)
            {
                SetErrorMessage("بيانات غير صحيحة");
                return RedirectToAction(nameof(Index));
            }

            try
            {
             
                    await _fieldService.UpdateFieldAsync(field);
                    SetSuccessMessage("تم تحديث القسم بنجاح");
                    return RedirectToAction(nameof(Index));
           
            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء تحديث القسم: {ex.Message}");
                await PopulateDropDownLists(field.GradeId);
                return View(field);
            }
        }

        // GET: Field/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var field = await _fieldService.GetFieldByIdAsync(id);
                if (field == null)
                {
                    SetErrorMessage("القسم المطلوب غير موجود");
                    return RedirectToAction(nameof(Index));
                }

                return View(field);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء جلب البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Field/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _fieldService.DeleteFieldAsync(id);
                if (result)
                {
                    SetSuccessMessage("تم حذف القسم بنجاح");
                }
                else
                {
                    SetErrorMessage("لم يتم العثور على القسم");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء حذف القسم: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Field/AssignEmployee/5
        public async Task<IActionResult> AssignEmployee(int id)
        {
            try
            {
                var field = await _fieldService.GetFieldByIdAsync(id);
                if (field == null)
                {
                    SetErrorMessage("القسم المطلوب غير موجود");
                    return RedirectToAction(nameof(Index));
                }

                // Get all active employees
                var employees = await _employeeService.GetActiveUsersAsync();
                ViewBag.Employees = new SelectList(employees, "Id", "FullName");
                ViewBag.Field = field;

                return View();
            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء جلب البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Field/AssignEmployee/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignEmployee(int id, int employeeId)
        {
            try
            {
                var result = await _fieldService.AssignUserToFieldAsync(employeeId, id);
                if (result)
                {
                    SetSuccessMessage("تم تعيين الموظف للقسم بنجاح");
                }
                else
                {
                    SetWarningMessage("الموظف مُعيّن مسبقاً لهذا القسم");
                }
                return RedirectToAction(nameof(Details), new { id = id });
            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء تعيين الموظف: {ex.Message}");
                return RedirectToAction(nameof(AssignEmployee), new { id = id });
            }
        }

        // POST: Field/RemoveEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveEmployee(int fieldId, int employeeId)
        {
            try
            {
                var result = await _fieldService.RemoveUserFromFieldAsync(employeeId, fieldId);
                if (result)
                {
                    SetSuccessMessage("تم إزالة الموظف من القسم بنجاح");
                }
                else
                {
                    SetErrorMessage("لم يتم العثور على تعيين الموظف");
                }
                return RedirectToAction(nameof(Details), new { id = fieldId });
            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء إزالة الموظف: {ex.Message}");
                return RedirectToAction(nameof(Details), new { id = fieldId });
            }
        }

        // GET: Field/GetByGrade/5
        public async Task<IActionResult> GetByGrade(int gradeId)
        {
            try
            {
                var fields = await _fieldService.GetFieldsByAcademicYearAsync(gradeId);
                return View("Index", fields);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء جلب البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        
        [HttpGet]
        public async Task<JsonResult> GetFieldsByGrade(int gradeId)
        {
            try
            {
                var fields = await _fieldService.GetFieldsByAcademicYearAsync(gradeId);
                var result = fields.Select(f => new {
                    id = f.Id,
                    name = f.Name
                }).ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

     
        private async Task PopulateDropDownLists(int? selectedGradeId = null)
        {
            try
            {
                var grades = await _gradeService.GetActiveAcademicYearsAsync();
                ViewBag.GradeId = new SelectList(grades, "Id", "Name", selectedGradeId);
            }
            catch (Exception ex)
            {
                ViewBag.GradeId = new SelectList(new List<Grade>(), "Id", "Name");
                SetErrorMessage($"حدث خطأ أثناء جلب المراحل الدراسية: {ex.Message}");
            }
        }
    }
}