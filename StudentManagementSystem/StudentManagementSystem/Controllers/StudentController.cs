using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
using System.IO;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentController(IStudentService studentService, IClassService classService, IWebHostEnvironment webHostEnvironment)
        {
            _studentService = studentService;
            _classService = classService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return View(students);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return View(new List<Student>());
            }
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    SetErrorMessage("الطالب غير موجود");
                    return RedirectToAction(nameof(Index));
                }
                return View(student);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Student/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                await PopulateViewBag();
                return View();
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student, IFormFile pictureProfile, IFormFile birthCertificate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Handle Picture Profile Upload
                    if (pictureProfile != null && pictureProfile.Length > 0)
                    {
                        var profilePicturePath = await SaveFileAsync(pictureProfile, "Picture_Profile");
                        if (profilePicturePath != null)
                        {
                            student.Picture_Profile = profilePicturePath;
                        }
                        else
                        {
                            SetErrorMessage("فشل في رفع صورة الملف الشخصي");
                            await PopulateViewBag();
                            return View(student);
                        }
                    }

                    // Handle Birth Certificate Upload
                    if (birthCertificate != null && birthCertificate.Length > 0)
                    {
                        var birthCertificatePath = await SaveFileAsync(birthCertificate, "birth_Certificate");
                        if (birthCertificatePath != null)
                        {
                            student.birth_Certificate = birthCertificatePath;
                        }
                        else
                        {
                            SetErrorMessage("فشل في رفع شهادة الميلاد");
                            await PopulateViewBag();
                            return View(student);
                        }
                    }

                    // Set created by current user
                    student.CreatedBy = GetCurrentUserId();
                    student.Date = DateTime.Now;

                    await _studentService.CreateStudentAsync(student);
                    SetSuccessMessage("تم إضافة الطالب بنجاح");
                    return RedirectToAction(nameof(Index));
                }

                await PopulateViewBag();
                return View(student);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في حفظ البيانات: {ex.Message}");
                await PopulateViewBag();
                return View(student);
            }
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    SetErrorMessage("الطالب غير موجود");
                    return RedirectToAction(nameof(Index));
                }

                await PopulateViewBag();
                return View(student);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student, IFormFile pictureProfile, IFormFile birthCertificate)
        {
            if (id != student.Id)
            {
                SetErrorMessage("خطأ في البيانات المرسلة");
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (ModelState.IsValid)
                {
                    // Get existing student to preserve file paths if no new files uploaded
                    var existingStudent = await _studentService.GetStudentByIdAsync(id);
                    if (existingStudent == null)
                    {
                        SetErrorMessage("الطالب غير موجود");
                        return RedirectToAction(nameof(Index));
                    }

                    // Handle Picture Profile Update
                    if (pictureProfile != null && pictureProfile.Length > 0)
                    {
                        // Delete old file if exists
                        DeleteFileIfExists(existingStudent.Picture_Profile);

                        var profilePicturePath = await SaveFileAsync(pictureProfile, "Picture_Profile");
                        if (profilePicturePath != null)
                        {
                            student.Picture_Profile = profilePicturePath;
                        }
                        else
                        {
                            SetErrorMessage("فشل في رفع صورة الملف الشخصي");
                            await PopulateViewBag();
                            return View(student);
                        }
                    }
                    else
                    {
                        // Keep existing picture if no new file uploaded
                        student.Picture_Profile = existingStudent.Picture_Profile;
                    }

                    // Handle Birth Certificate Update
                    if (birthCertificate != null && birthCertificate.Length > 0)
                    {
                        // Delete old file if exists
                        DeleteFileIfExists(existingStudent.birth_Certificate);

                        var birthCertificatePath = await SaveFileAsync(birthCertificate, "birth_Certificate");
                        if (birthCertificatePath != null)
                        {
                            student.birth_Certificate = birthCertificatePath;
                        }
                        else
                        {
                            SetErrorMessage("فشل في رفع شهادة الميلاد");
                            await PopulateViewBag();
                            return View(student);
                        }
                    }
                    else
                    {
                        // Keep existing certificate if no new file uploaded
                        student.birth_Certificate = existingStudent.birth_Certificate;
                    }

                    // Preserve creation info
                    student.CreatedBy = existingStudent.CreatedBy;
                    student.Date = existingStudent.Date;

                    await _studentService.UpdateStudentAsync(student);
                    SetSuccessMessage("تم تحديث بيانات الطالب بنجاح");
                    return RedirectToAction(nameof(Index));
                }

                await PopulateViewBag();
                return View(student);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحديث البيانات: {ex.Message}");
                await PopulateViewBag();
                return View(student);
            }
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    SetErrorMessage("الطالب غير موجود");
                    return RedirectToAction(nameof(Index));
                }
                return View(student);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _studentService.DeleteStudentAsync(id);
                if (success)
                {
                    SetSuccessMessage("تم حذف الطالب بنجاح");
                }
                else
                {
                    SetErrorMessage("فشل في حذف الطالب");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في حذف البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // Helper method to save files
        private async Task<string> SaveFileAsync(IFormFile file, string subfolder)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return null;

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return null;
                }

                // Create directory if it doesn't exist
                var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "upload", "image", subfolder);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadPath, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return relative path for database storage
                return Path.Combine("upload", "image", subfolder, fileName).Replace("\\", "/");
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Helper method to delete file if exists
        private void DeleteFileIfExists(string filePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }
            catch (Exception)
            {
                // Log error but don't throw exception
            }
        }

        // Helper method to populate ViewBag with dropdown data
        private async Task PopulateViewBag()
        {
            try
            {
                var classes = await _classService.GetAllClassesAsync();
                ViewBag.ClassId = new SelectList(classes, "Id", "Name");
            }
            catch (Exception)
            {
                ViewBag.ClassId = new SelectList(new List<Class>(), "Id", "Name");
            }
        }

        // GET: Student/GetStudentsByClass/5
        [HttpGet]
        public async Task<IActionResult> GetStudentsByClass(int classId)
        {
            try
            {
                var students = await _studentService.GetStudentsByClassAsync(classId);
                return Json(students.Select(s => new {
                    id = s.Id,
                    name = s.Name,
                    code = s.Code
                }));
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // GET: Student/GetStudentsWithTasks
        public async Task<IActionResult> GetStudentsWithTasks()
        {
            try
            {
                var students = await _studentService.GetStudentsWithTasksAsync();
                return View(students);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}