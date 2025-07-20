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
    
    public class ClassController : BaseController 
    {
        private readonly IClassService _classService;
        private readonly IFieldService _fieldService;
        private readonly IUserService _employeeService; 
        private readonly IStudentService _studentService;

        public ClassController(IClassService classService, IFieldService fieldService, IUserService employeeService, IStudentService studentService)
        {
            _classService = classService;
            _fieldService = fieldService;
            _employeeService = employeeService;
            _studentService = studentService;
        }

        // GET: Class
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var classes = await _classService.GetAllClassesAsync();
            return View(classes);
        }

        // GET: Class/Details/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var classEntity = await _classService.GetClassByIdAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }
            return View(classEntity);
        }

        // GET: Class/Create
        [HttpGet]
        [Authorize]
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
            try
            {
                // Use the helper method from BaseController
                var currentUserId = GetCurrentUserId();

                if (currentUserId == 0) // Check if user ID is valid (0 if not found)
                {
                    ModelState.AddModelError("", "Unable to identify the current user for CreatedBy. Please log in again.");
                    await PopulateDropDownLists();
                    return View(classEntity);
                }

                // Set CreatedBy automatically
                classEntity.CreatedBy = currentUserId;
                classEntity.Date = DateTime.UtcNow;

                await _classService.CreateClassAsync(classEntity);
                // Use the helper method from BaseController
                SetSuccessMessage("Class created successfully!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the class: " + ex.Message);
            }

            await PopulateDropDownLists();
            return View(classEntity);
        }

        // GET: Class/Edit/5
        [HttpGet]
        [Authorize]
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

            try
            {
                // Use the helper method from BaseController
                var currentUserId = GetCurrentUserId();

                if (currentUserId == 0) // Check if user ID is valid
                {
                    ModelState.AddModelError("", "Unable to identify the current user for UpdatedBy. Please log in again.");
                    await PopulateDropDownLists();
                    return View(classEntity);
                }

                // Set CreatedBy automatically (assuming CreatedBy is used for the last updater here,
                // if you have an 'UpdatedBy' property in your model, use that instead)
                classEntity.CreatedBy = currentUserId; // Or classEntity.UpdatedBy = currentUserId;
                // classEntity.Date = DateTime.UtcNow; // This might need to be 'UpdatedAt' if you track both

                await _classService.UpdateClassAsync(classEntity);
                // Use the helper method from BaseController
                SetSuccessMessage("Class updated successfully!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the class: " + ex.Message);
            }

            await PopulateDropDownLists();
            return View(classEntity);
        }

        // POST: Class/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var classEntity = await _classService.GetClassByIdAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }
            var result = await _classService.DeleteClassAsync(id);
            // Use the helper method from BaseController
            if (result)
            {
                SetSuccessMessage("Class deleted successfully!");
            }
            else
            {
                SetErrorMessage("Error deleting class.");
            }
            return RedirectToAction(nameof(Index));
        }

        // Code generation function for students
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> GenerateStudentCodes(int classId)
        {
            try
            {
                var classEntity = await _classService.GetClassByIdAsync(classId);
                if (classEntity == null)
                {
                    SetErrorMessage("الفصل غير موجود");
                    return RedirectToAction(nameof(Details), new { id = classId });
                }

                // Ensure students are eager loaded or accessible
                // You might need to adjust your GetClassByIdAsync to include Students
                // For example: _context.Classes.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == id)
                var students = classEntity.Students?.Where(s => s.IsActive).ToList();
                if (students == null || !students.Any())
                {
                    SetErrorMessage("لا يوجد طلاب في هذا الفصل");
                    return RedirectToAction(nameof(Details), new { id = classId });
                }

                // Check if any students already have codes.
                if (students.Any(s => !string.IsNullOrEmpty(s.Code)))
                {
                    SetErrorMessage("بعض الطلاب لديهم أكواد بالفعل");
                    return RedirectToAction(nameof(Details), new { id = classId });
                }

                // Arrange students alphabetically
                var sortedStudents = students.OrderBy(s => s.Name).ToList();

                // Get the next serial number
                var nextSequenceNumber = await GetNextSequenceNumber();

                // Generate codes
                var currentYear = DateTime.Now.Year % 100; // Last two digits of the year

                for (int i = 0; i < sortedStudents.Count; i++)
                {
                    var sequenceNumber = nextSequenceNumber + i;
                    var studentCode = $"J03{currentYear:D2}{sequenceNumber:D3}";
                    sortedStudents[i].Code = studentCode;

                    await _studentService.UpdateStudentAsync(sortedStudents[i]);
                }

                SetSuccessMessage($"تم توليد الأكواد بنجاح لعدد {sortedStudents.Count} طالب");
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء توليد الأكواد: " + ex.Message);
            }

            return RedirectToAction(nameof(Details), new { id = classId });
        }

        // Function to get the next serial number
        private async Task<int> GetNextSequenceNumber()
        {
            var allStudents = await _studentService.GetAllStudentsAsync();

            // Get the last used serial number
            var lastUsedNumber = 0;
            var currentYear = DateTime.Now.Year % 100;

            foreach (var student in allStudents)
            {
                if (!string.IsNullOrEmpty(student.Code) && student.Code.StartsWith($"J03{currentYear:D2}"))
                {
                    // Extract the serial number from the code
                    var codeString = student.Code.Substring(5); // Ignore J03XX
                    if (int.TryParse(codeString, out int codeNumber))
                    {
                        lastUsedNumber = Math.Max(lastUsedNumber, codeNumber);
                    }
                }
            }

            return lastUsedNumber + 1;
        }

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
                var fields = await _fieldService.GetActiveFieldsAsync();
                ViewBag.FieldId = new SelectList(fields, "Id", "Name");
            }
            catch (Exception ex)
            {
                ViewBag.FieldId = new SelectList(new List<SelectListItem>(), "Value", "Text");
            }
        }
    }
}