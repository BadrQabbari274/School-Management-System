using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Implementation;
using StudentManagementSystem.Service.Interface;
using StudentManagementSystem.ViewModels;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace StudentManagementSystem.Controllers
{
    
    public class ClassController : BaseController 
    {
        private readonly IClassService _classService;
        private readonly IUserService _employeeService; 
        private readonly IStudentService _studentService;
        private readonly IGradeService _gradeService;


        public ClassController(IClassService classService, IUserService employeeService, IStudentService studentService, IGradeService gradeService)
        {
            _classService = classService;
            _employeeService = employeeService;
            _studentService = studentService;
            _gradeService = gradeService;
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
        // Inside your Controller
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await _classService.GetClassDetailsAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
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
        public async Task<IActionResult> Create(Classes classEntity)
        {
            try
            {
                // Use the helper method from BaseController
                var currentUserId = GetCurrentUserId();

                if (currentUserId == 0) // Check if user ID is valid (0 if not found)
                {
                    ModelState.AddModelError("", "Unable to identify the current user for CreatedBy. Please log in again.");
                    return View(classEntity);
                }

                // Set CreatedBy automatically
                classEntity.CreatedBy_Id = currentUserId;
                classEntity.Date = DateTime.UtcNow;
                classEntity.IsActive = true;

                await _classService.CreateClassAsync(classEntity);
                // Use the helper method from BaseController
                SetSuccessMessage("Class created successfully!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await PopulateDropDownLists();
                ModelState.AddModelError("", "An error occurred while creating the class: " + ex.Message);
            }

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
        public async Task<IActionResult> Edit(int id, Classes classEntity)
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

                classEntity.CreatedBy_Id = currentUserId; 
                await _classService.UpdateClassAsync(classEntity);
                // Use the helper method from BaseController
                SetSuccessMessage("Class updated successfully!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await PopulateDropDownLists();
                ModelState.AddModelError("", "An error occurred while updating the class: " + ex.Message);
            }

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

        //// Generates unique codes for students within a class, reserving codes based on MaxStudents per class.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public async Task<IActionResult> GenerateStudentCodes(int classId)
        //{
        //    try
        //    {
        //        // Ensure _classService.GetClassByIdAsync(id) loads the Students collection (e.g., using .Include()).
        //        var classEntity = await _classService.GetClassByIdAsync(classId);
        //        if (classEntity == null)
        //        {
        //            SetErrorMessage("الفصل غير موجود");
        //            return RedirectToAction(nameof(Details), new { id = classId });
        //        }

        //        var studentsInClass = classEntity.Students?.Where(s => s.IsActive).ToList();

        //        if (studentsInClass == null || !studentsInClass.Any())
        //        {
        //            SetErrorMessage("لا يوجد طلاب نشطون في هذا الفصل لتوليد الأكواد لهم.");
        //            return RedirectToAction(nameof(Details), new { id = classId });
        //        }

        //        // Prevents regeneration if any student in the class already has a code.
        //        if (studentsInClass.Any(s => !string.IsNullOrEmpty(s.Code)))
        //        {
        //            SetErrorMessage("بعض الطلاب في هذا الفصل لديهم أكواد بالفعل. لا يمكن إعادة توليد الأكواد للفصل بالكامل.");
        //            return RedirectToAction(nameof(Details), new { id = classId });
        //        }

        //        var sortedStudents = studentsInClass.OrderBy(s => s.Name).ToList();

        //        // Gets the base sequence number for this class's block.
        //        var baseSequenceNumberForClass = await GetNextSequenceNumberForClass(classId);

        //        var currentYear = DateTime.Now.Year % 100; // Last two digits of the current year.

        //        for (int i = 0; i < sortedStudents.Count; i++)
        //        {
        //            // Calculates the student's unique sequence number within the class's block.
        //            var sequenceNumber = baseSequenceNumberForClass + i;
        //            // Formats the student code: J03 + last two year digits + 3-digit sequence number.
        //            var studentCode = $"J03{currentYear:D2}{sequenceNumber:D3}";
        //            sortedStudents[i].Code = studentCode;

        //            await _studentService.UpdateStudentAsync(sortedStudents[i]);
        //        }

        //        // Use MaxStudents instead of CodesPerClass constant
        //        var maxStudentsForClass = classEntity.MaxStudents ?? 25; // Default to 25 if MaxStudents is null
        //        SetSuccessMessage($"تم توليد الأكواد بنجاح لعدد {sortedStudents.Count} طالب وتخصيص {maxStudentsForClass} كود للفصل.");
        //    }
        //    catch (Exception ex)
        //    {
        //        SetErrorMessage("حدث خطأ أثناء توليد الأكواد: " + ex.Message);
        //    }

        //    return RedirectToAction(nameof(Details), new { id = classId });
        //}

        //private async Task<int> GetNextSequenceNumberForClass(int targetClassId)
        //{
        //    var allClasses = await _classService.GetAllClassesAsync();

        //    var sortedClasses = allClasses
        //                        .OrderBy(c => c.Name)
        //                        .ThenBy(c => c.Date)
        //                        .ThenBy(c => c.Id)
        //                        .ToList();

        //    int baseSequence = 1;

        //    foreach (var cls in sortedClasses)
        //    {
        //        if (cls.Id == targetClassId)
        //        {
        //            // Found the target class, return its calculated starting sequence.
        //            return baseSequence;
        //        }
        //        // For each preceding class, increment the base sequence by the class's MaxStudents (or default to 25).
        //        var codesForThisClass = cls.MaxStudents ?? 25; // Default to 25 if MaxStudents is null
        //        baseSequence += codesForThisClass;
        //    }

        //    return baseSequence; // Fallback, though targetClassId should always be found.
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public async Task<IActionResult> ResetStudentCodes(int classId)
        //{
        //    try
        //    {
        //        var classEntity = await _classService.GetClassByIdAsync(classId);
        //        if (classEntity == null)
        //        {
        //            SetErrorMessage("الفصل غير موجود");
        //            return RedirectToAction(nameof(Details), new { id = classId });
        //        }
        //        var studentsInClass = classEntity.Students?.Where(s => s.IsActive).ToList();
        //        if (studentsInClass == null || !studentsInClass.Any())
        //        {
        //            SetErrorMessage("لا يوجد طلاب نشطون في هذا الفصل لإعادة تعيين أكوادهم.");
        //            return RedirectToAction(nameof(Details), new { id = classId });
        //        }

        //        foreach (var student in studentsInClass)
        //        {
        //            student.Code = null;
        //            await _studentService.UpdateStudentAsync(student);
        //        }

        //        var sortedStudents = studentsInClass.OrderBy(s => s.Name).ToList();
        //        var baseSequenceNumberForClass = await GetNextSequenceNumberForClass(classId);
        //        var currentYear = DateTime.Now.Year % 100;

        //        for (int i = 0; i < sortedStudents.Count; i++)
        //        {
        //            var sequenceNumber = baseSequenceNumberForClass + i;
        //            var studentCode = $"J03{currentYear:D2}{sequenceNumber:D3}";
        //            sortedStudents[i].Code = studentCode;
        //            await _studentService.UpdateStudentAsync(sortedStudents[i]);
        //        }

        //        SetSuccessMessage($"تم إعادة تعيين الأكواد بنجاح لعدد {sortedStudents.Count} طالب في الفصل.");
        //    }
        //    catch (Exception ex)
        //    {
        //        SetErrorMessage("حدث خطأ أثناء إعادة تعيين الأكواد: " + ex.Message);
        //    }

        //    return RedirectToAction(nameof(Details), new { id = classId });
        //}


        // Updated Controller Methods
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> GenerateStudentCodes(int classId)
        {
            try
            {
                var result = await _classService.GenerateStudentCodesForClassAsync(classId);

                if (result)
                {
                    var classEntity = await _classService.GetClassByIdAsync(classId);
                    var studentsInClass = await _classService.GetStudentsByClassAndWorkingYearAsync(classId,
                        (await _classService.GetCurrentWorkingYearAsync()).Id);
                    var maxStudentsForClass = classEntity?.MaxStudents ?? 25;

                    SetSuccessMessage($"تم توليد الأكواد بنجاح لعدد {studentsInClass.Count()} طالب وتخصيص {maxStudentsForClass} كود للفصل.");
                }
                else
                {
                    // Determine specific error message
                    var currentWorkingYear = await _classService.GetCurrentWorkingYearAsync();
                    if (currentWorkingYear == null)
                    {
                        SetErrorMessage("لا يوجد عام دراسي نشط حالياً");
                    }
                    else
                    {
                        var classEntity = await _classService.GetClassByIdAsync(classId);
                        if (classEntity == null)
                        {
                            SetErrorMessage("الفصل غير موجود");
                        }
                        else if (!classEntity.Grade.Name.ToLower().Contains("junior"))
                        {
                            SetErrorMessage("يمكن توليد الأكواد فقط لفصول المرحلة (Junior)");
                        }
                        else
                        {
                            var studentsInClass = await _classService.GetStudentsByClassAndWorkingYearAsync(classId, currentWorkingYear.Id);
                            if (!studentsInClass.Any())
                            {
                                SetErrorMessage("لا يوجد طلاب نشطون في هذا الفصل لتوليد الأكواد لهم.");
                            }
                            else if (studentsInClass.Any(s => !string.IsNullOrEmpty(s.Student.Code)))
                            {
                                SetErrorMessage("بعض الطلاب في هذا الفصل لديهم أكواد بالفعل. لا يمكن إعادة توليد الأكواد للفصل بالكامل.");
                            }
                            else
                            {
                                SetErrorMessage("لا يوجد طلاب في المرحلة الإعدادية (Junior) في هذا الفصل");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء توليد الأكواد: " + ex.Message);
            }

            return RedirectToAction(nameof(Details), new { id = classId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ResetStudentCodes(int classId)
        {
            try
            {
                var result = await _classService.ResetStudentCodesForClassAsync(classId);

                if (result)
                {
                    var currentWorkingYear = await _classService.GetCurrentWorkingYearAsync();
                    var studentsInClass = await _classService.GetStudentsByClassAndWorkingYearAsync(classId, currentWorkingYear.Id);
                    SetSuccessMessage($"تم إعادة تعيين الأكواد بنجاح لعدد {studentsInClass.Count()} طالب في الفصل.");
                }
                else
                {
                    // Determine specific error message (similar logic as GenerateStudentCodes)
                    var currentWorkingYear = await _classService.GetCurrentWorkingYearAsync();
                    if (currentWorkingYear == null)
                    {
                        SetErrorMessage("لا يوجد عام دراسي نشط حالياً");
                    }
                    else
                    {
                        var classEntity = await _classService.GetClassByIdAsync(classId);
                        if (classEntity == null)
                        {
                            SetErrorMessage("الفصل غير موجود");
                        }
                        else if (!classEntity.Grade.Name.ToLower().Contains("junior"))
                        {
                            SetErrorMessage("يمكن إعادة تعيين الأكواد فقط لفصول المرحلة الإعدادية (Junior)");
                        }
                        else
                        {
                            SetErrorMessage("لا يوجد طلاب نشطون في هذا الفصل لإعادة تعيين أكوادهم.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage("حدث خطأ أثناء إعادة تعيين الأكواد: " + ex.Message);
            }

            return RedirectToAction(nameof(Details), new { id = classId });
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
                var Grades = await _gradeService.GetAllAcademicYearsAsync();
                ViewBag.GradeId = new SelectList(Grades, "Id", "Name");
            }
            catch (Exception ex)
            {
                ViewBag.GradeId = new SelectList(new List<SelectListItem>(), "Value", "Text");
            }
        }
    }
}