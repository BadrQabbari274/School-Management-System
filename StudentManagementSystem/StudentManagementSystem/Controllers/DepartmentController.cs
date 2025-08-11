using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
using System.Security.Claims;

namespace StudentManagementSystem.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _fieldService;
        private readonly IGradeService _gradeService;
        private readonly IUserService _employeeService;

        public DepartmentController(
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
                var fields = await _fieldService.GetAllDepartmentsAsync();
                return View(fields);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء جلب البيانات: {ex.Message}");
                return View(new List<Department>());
            }
        }

        // GET: Field/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var field = await _fieldService.GetDepartmentByIdAsync(id);
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
        [HttpGet]
        public IActionResult Create()
        {
                return View();
        }

        // POST: Field/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department field)
        {
            try
            {
       
                    field.CreatedBy_Id = GetCurrentUserId();
                    field.CreatedDate = DateTime.Now;
                    field.IsActive = true;

                    var createdField = await _fieldService.CreateDepartmentAsync(field);
                    SetSuccessMessage("تم إنشاء القسم بنجاح");
                    return RedirectToAction(nameof(Index));
                

            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء إنشاء القسم: {ex.Message}");
                return View(field);
            }
        }

        // GET: Field/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var field = await _fieldService.GetDepartmentByIdAsync(id);
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

        // POST: Field/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( Department field)
        {


            try
            {
                field.IsActive = true;
                await _fieldService.UpdateDepartmentAsync(field);
                    SetSuccessMessage("تم تحديث القسم بنجاح");
                    return RedirectToAction(nameof(Index));
           
            }
            catch (Exception ex)
            {
                SetErrorMessage($"حدث خطأ أثناء تحديث القسم: {ex.Message}");
                return View(field);
            }
        }

        // GET: Field/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var field = await _fieldService.GetDepartmentByIdAsync(id);
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
                var result = await _fieldService.DeleteDepartmentAsync(id);
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
                var field = await _fieldService.GetDepartmentByIdAsync(id);
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
                var result = await _fieldService.AssignUserToDepartmentAsync(employeeId, id);
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
                var result = await _fieldService.RemoveUserFromDepartmentAsync(employeeId, fieldId);
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

       

        
        //[HttpGet]
        //public async Task<JsonResult> GetFieldsByGrade(int gradeId)
        //{
        //    try
        //    {
        //        var fields = await _fieldService.GetDepartmentsByAcademicYearAsync(gradeId);
        //        var result = fields.Select(f => new {
        //            id = f.Id,
        //            name = f.Name
        //        }).ToList();
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { error = ex.Message });
        //    }
        //}
    }
}