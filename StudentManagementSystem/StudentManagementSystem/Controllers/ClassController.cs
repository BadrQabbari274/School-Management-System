using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
// Removed: using Microsoft.AspNetCore.Authentication;
// Removed: using Microsoft.AspNetCore.Authentication.Cookies;
// Removed: using System.Security.Claims;
// Removed: using Microsoft.AspNetCore.Authorization; // Authorization is now handled by BaseController

namespace StudentManagementSystem.Controllers
{
    // Inherit from BaseController to utilize its helper methods and class-level [Authorize] attribute
    public class ClassController : BaseController
    {
        private readonly IClassService _classService;
        private readonly IFieldService _fieldService;
        private readonly IUserService _employeeService;

        public ClassController(IClassService classService, IFieldService fieldService, IUserService employeeService)
        {
            _classService = classService;
            _fieldService = fieldService;
            _employeeService = employeeService;
        }

        // GET: Class
        [Authorize] // تم إرجاع سمة Authorize بناءً على طلبك
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var classes = await _classService.GetAllClassesAsync();
            return View(classes);
        }

        // GET: Class/Details/5
        // [Authorize] // This is now inherited from BaseController
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var classEntity = await _classService.GetClassByIdAsync(id);
            if (classEntity == null)
            {
                // Using NotFound() from Controller base class
                return NotFound();
            }
            return View(classEntity);
        }

        // GET: Class/Create
        // [Authorize] // This is now inherited from BaseController
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropDownLists();
            return View();
        }

        // POST: Class/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize] // This is now inherited from BaseController
        public async Task<IActionResult> Create(Class classEntity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _classService.CreateClassAsync(classEntity);
                    // Using SetSuccessMessage from BaseController
                    SetSuccessMessage("Class created successfully!");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Using SetErrorMessage from BaseController
                    SetErrorMessage("An error occurred while creating the class: " + ex.Message);
                    ModelState.AddModelError("", "An error occurred while creating the class: " + ex.Message);
                }
            }

            await PopulateDropDownLists();
            return View(classEntity);
        }

        // GET: Class/Edit/5
        // [Authorize] // This is now inherited from BaseController
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var classEntity = await _classService.GetClassByIdAsync(id);
            if (classEntity == null)
            {
                // Using NotFound() from Controller base class
                return NotFound();
            }

            await PopulateDropDownLists();
            return View(classEntity);
        }

        // POST: Class/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize] // This is now inherited from BaseController
        public async Task<IActionResult> Edit(int id, Class classEntity)
        {
            if (id != classEntity.Id)
            {
                // Using NotFound() from Controller base class
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _classService.UpdateClassAsync(classEntity);
                    // Using SetSuccessMessage from BaseController
                    SetSuccessMessage("Class updated successfully!");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Using SetErrorMessage from BaseController
                    SetErrorMessage("An error occurred while updating the class: " + ex.Message);
                    ModelState.AddModelError("", "An error occurred while updating the class: " + ex.Message);
                }
            }

            await PopulateDropDownLists();
            return View(classEntity);
        }

        // GET: Class/Delete/5
        // [Authorize] // This is now inherited from BaseController
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var classEntity = await _classService.GetClassByIdAsync(id);
            if (classEntity == null)
            {
                // Using NotFound() from Controller base class
                return NotFound();
            }
            return View(classEntity);
        }

        // POST: Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // [Authorize] // This is now inherited from BaseController
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _classService.DeleteClassAsync(id);
                if (result)
                {
                    // Using SetSuccessMessage from BaseController
                    SetSuccessMessage("Class deleted successfully!");
                }
                else
                {
                    // Using SetErrorMessage from BaseController
                    SetErrorMessage("Class not found or could not be deleted.");
                }
            }
            catch (Exception ex)
            {
                // Using SetErrorMessage from BaseController
                SetErrorMessage("An error occurred while deleting the class: " + ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Class/GetByField/5
        // [Authorize] // This is now inherited from BaseController
        [HttpGet]
        public async Task<IActionResult> GetByField(int fieldId)
        {
            var classes = await _classService.GetClassesByFieldAsync(fieldId);
            return View("Index", classes);
        }

        // AJAX method for getting classes by field
        // [Authorize] // This is now inherited from BaseController
        [HttpGet]
        public async Task<JsonResult> GetClassesByField(int fieldId)
        {
            var classes = await _classService.GetClassesByFieldAsync(fieldId);
            return Json(classes.Select(c => new { value = c.Id, text = c.Name }));
        }

        
        private async Task PopulateDropDownLists()
        {
            try
            {
               
                var fields = await _fieldService.GetActiveFieldsAsync();
                ViewBag.FieldId = new SelectList(fields, "Id", "Name");

                var employees = await _employeeService.GetActiveUsersAsync();
                ViewBag.CreatedBy = new SelectList(employees, "Id", "Name");
            }
            catch
            {
                ViewBag.FieldId = new SelectList(new List<SelectListItem>(), "Value", "Text");
                ViewBag.CreatedBy = new SelectList(new List<SelectListItem>(), "Value", "Text");
            }
        }
    }
}
