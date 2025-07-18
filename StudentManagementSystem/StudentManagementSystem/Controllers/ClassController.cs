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

            try
            {
                // Get the current user's ID from claims
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(currentUserId))
                {
                    ModelState.AddModelError("", "Unable to identify the current user for CreatedBy. Please log in again.");
                    await PopulateDropDownLists();
                    return View(classEntity);
                }

                // Set CreatedBy automatically
                classEntity.CreatedBy = int.Parse(currentUserId);
                classEntity.Date = DateTime.UtcNow;

                await _classService.CreateClassAsync(classEntity);
                TempData["SuccessMessage"] = "Class created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the class: " + ex.Message);
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

            try
            {
                // Get the current user's ID from claims for UpdatedBy
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(currentUserId))
                {
                    ModelState.AddModelError("", "Unable to identify the current user for UpdatedBy. Please log in again.");
                    await PopulateDropDownLists();
                    return View(classEntity);
                }

                // Set UpdatedBy automatically
                classEntity.CreatedBy = int.Parse(currentUserId);
                // classEntity.Date = DateTime.UtcNow;

                await _classService.UpdateClassAsync(classEntity);
                TempData["SuccessMessage"] = "Class updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the class: " + ex.Message);
            }


            await PopulateDropDownLists();
            return View(classEntity);
        }


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
                    TempData["ErrorMessage"] = "الفصل غير موجود";
                    return RedirectToAction(nameof(Details), new { id = classId });
                }

                var students = classEntity.Students?.Where(s => s.IsActive).ToList();
                if (students == null || !students.Any())
                {
                    TempData["ErrorMessage"] = "لا يوجد طلاب في هذا الفصل";
                    return RedirectToAction(nameof(Details), new { id = classId });
                }

                // Check if any students already have codes.
                if (students.Any(s => !string.IsNullOrEmpty(s.Code)))
                {
                    TempData["ErrorMessage"] = "بعض الطلاب لديهم أكواد بالفعل";
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

                TempData["SuccessMessage"] = $"تم توليد الأكواد بنجاح لعدد {sortedStudents.Count} طالب";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء توليد الأكواد: " + ex.Message;
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

                // "CreatedBy" is no longer a dropdown, so we remove the related code.
                // var employees = await _employeeService.GetActiveUsersAsync();
                // ViewBag.CreatedBy = new SelectList(employees, "Id", "Name");
            }
            catch (Exception ex)
            {

                ViewBag.FieldId = new SelectList(new List<SelectListItem>(), "Value", "Text");
            }
        }
    }
}