using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StudentManagementSystem.Controllers
{
    public class ClassController : Controller
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var classes = await _classService.GetAllClassesAsync();
            return View(classes);
        }
        [HttpGet]
        [Authorize]
        // GET: Class/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var classEntity = await _classService.GetClassByIdAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }
            return View(classEntity);
        }
        [HttpGet]
        [Authorize]
        // GET: Class/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropDownLists();
            return View();
        }

        // POST: Class/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Class classEntity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _classService.CreateClassAsync(classEntity);
                    TempData["SuccessMessage"] = "Class created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the class: " + ex.Message);
                }
            }

            await PopulateDropDownLists();
            return View(classEntity);
        }
        [HttpGet]
        [Authorize]
        // GET: Class/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var classEntity = await _classService.GetClassByIdAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }

            await PopulateDropDownLists();
            return View(classEntity);
        }

        // POST: Class/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Class classEntity)
        {
            if (id != classEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _classService.UpdateClassAsync(classEntity);
                    TempData["SuccessMessage"] = "Class updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the class: " + ex.Message);
                }
            }

            await PopulateDropDownLists();
            return View(classEntity);
        }

        // GET: Class/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var classEntity = await _classService.GetClassByIdAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }
            return View(classEntity);
        }

        // POST: Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _classService.DeleteClassAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Class deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Class not found or could not be deleted.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the class: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Class/GetByField/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetByField(int fieldId)
        {
            var classes = await _classService.GetClassesByFieldAsync(fieldId);
            return View("Index", classes);
        }

        // AJAX method for getting classes by field
        [HttpGet]
        [Authorize]
        public async Task<JsonResult> GetClassesByField(int fieldId)
        {
            var classes = await _classService.GetClassesByFieldAsync(fieldId);
            return Json(classes.Select(c => new { value = c.Id, text = c.Name }));
        }

        // Helper method to populate dropdown lists
        private async Task PopulateDropDownLists()
        {
            try
            {
                // إذا كان عندك FieldService
                var fields = await _fieldService.GetActiveFieldsAsync();
                ViewBag.FieldId = new SelectList(fields, "Id", "Name");

                // إذا كان عندك EmployeeService
                var employees = await _employeeService.GetActiveUsersAsync();
                ViewBag.CreatedBy = new SelectList(employees, "Id", "Name");
            }
            catch
            {
                // إذا مفيش services دي، استخدم قائمة فارغة
                ViewBag.FieldId = new SelectList(new List<SelectListItem>(), "Value", "Text");
                ViewBag.CreatedBy = new SelectList(new List<SelectListItem>(), "Value", "Text");
            }
        }
    }
}