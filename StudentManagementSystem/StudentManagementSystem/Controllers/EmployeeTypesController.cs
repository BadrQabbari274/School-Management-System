using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
using System;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    public class EmployeeTypesController : BaseController
    {
        private readonly IUserService _userService;

        public EmployeeTypesController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: EmployeeTypes
        public async Task<IActionResult> Index()
        {
            var employeeTypes = await _userService.GetActiveEmployeeTypesAsync();
            return View(employeeTypes);
        }

        // GET: EmployeeTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeType = await _userService.GetEmployeeTypeByIdAsync(id.Value);
            if (employeeType == null || employeeType.IsDeleted)
            {
                return NotFound();
            }

            return View(employeeType);
        }

        // GET: EmployeeTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmployeeTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeTypes employeeType)
        {
                try
                {
                    employeeType.CreatedBy_Id = GetCurrentUserId();
                    employeeType.CreatedDate = DateTime.Now;
                    employeeType.IsDeleted = false;
                    await _userService.CreateEmployeeTypeAsync(employeeType);
                    SetSuccessMessage("تم إنشاء نوع الموظف بنجاح");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    SetErrorMessage("حدث خطأ أثناء إنشاء نوع الموظف: " + ex.Message);
                }
                    return View(employeeType);
        }

        // GET: EmployeeTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeType = await _userService.GetEmployeeTypeByIdAsync(id.Value);
            if (employeeType == null || employeeType.IsDeleted)
            {
                return NotFound();
            }
            return View(employeeType);
        }

        // POST: EmployeeTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeTypes employeeType)
        {
            if (id != employeeType.Id)
            {
                return NotFound();
            }

                try
                {
                    await _userService.UpdateEmployeeTypeAsync(employeeType);
                    SetSuccessMessage("تم تحديث نوع الموظف بنجاح");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if (!await _userService.EmployeeTypeExistsAsync(employeeType.Id))
                    {
                        return NotFound();
                    }
                    SetErrorMessage("حدث خطأ أثناء تحديث نوع الموظف: " + ex.Message);
                }
            return View(employeeType);
        }

        // POST: EmployeeTypes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _userService.DeleteEmployeeTypeAsync(id);
                if (result)
                {
                    // استخدم SetSuccessMessage من BaseController
                    SetSuccessMessage("تم حذف نوع الموظف بنجاح");
                }
                else
                {
                    // استخدم SetErrorMessage من BaseController
                    SetErrorMessage("لم يتم العثور على نوع الموظف");
                }
            }
            catch (Exception ex)
            {
                // استخدم SetErrorMessage من BaseController
                SetErrorMessage("حدث خطأ أثناء حذف نوع الموظف: " + ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}