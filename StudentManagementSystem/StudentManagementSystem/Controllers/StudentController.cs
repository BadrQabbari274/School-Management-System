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
        private readonly ISectionService _sectionService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentController(IStudentService studentService, ISectionService sectionService, IWebHostEnvironment webHostEnvironment)
        {
            _studentService = studentService;
           _sectionService = sectionService;
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
                return View(new List<Students>());
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
        public class CreateStudentViewModel
        {
            public Students Student { get; set; }
            public int SelectedSectionId { get; set; }
            public List<SelectListItem> Sections { get; set; }
        }

        // في الـ Controller
        public async Task<IActionResult> Create()
        {
            try
            {
                var sections = await _sectionService.GetActiveSectionsAsync();
                var viewModel = new CreateStudentViewModel
                {
                    Student = new Students(),
                    Sections = sections.Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name_Of_Section
                    }).ToList()
                };

                return View(viewModel);
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
        public async Task<IActionResult> Create(CreateStudentViewModel viewModel, IFormFile pictureProfile, IFormFile birthCertificate)
        {
            try
            {
                // استخراج البيانات من الرقم القومي
                var nationalIdData = ExtractNationalIdData(viewModel.Student.Natrual_Id);
                if (nationalIdData.IsValid)
                {
                    viewModel.Student.Date_of_birth = nationalIdData.BirthDate.ToString();
                    viewModel.Student.Governate = nationalIdData.Governorate;
                }
                else
                {
                    SetErrorMessage("الرقم القومي غير صحيح");

                    // إعادة تحميل البيانات في حالة الخطأ
                    var sections = await _sectionService.GetActiveSectionsAsync();
                    viewModel.Sections = sections.Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name_Of_Section
                    }).ToList();

                    return View(viewModel);
                }

                // Set created by current user
                viewModel.Student.CreatedBy_Id = GetCurrentUserId();
                viewModel.Student.Date = DateTime.Now;

                // إنشاء الطالب أولاً للحصول على الـ ID
                await _studentService.CreateStudentAsync(viewModel.Student);

                // Handle Picture Profile Upload
                if (pictureProfile != null && pictureProfile.Length > 0)
                {
                    var profilePicturePath = await SaveFileAsync(pictureProfile, "profile", viewModel.Student.Id, viewModel.Student.Name);
                    if (profilePicturePath != null)
                    {
                        viewModel.Student.Picture_Profile = profilePicturePath;
                    }
                    else
                    {
                        SetErrorMessage("فشل في رفع صورة الملف الشخصي");

                        // إعادة تحميل البيانات في حالة الخطأ
                        var sections = await _sectionService.GetActiveSectionsAsync();
                        viewModel.Sections = sections.Select(s => new SelectListItem
                        {
                            Value = s.Id.ToString(),
                            Text = s.Name_Of_Section
                        }).ToList();

                        return View(viewModel);
                    }
                }

                // Handle Birth Certificate Upload
                if (birthCertificate != null && birthCertificate.Length > 0)
                {
                    var birthCertificatePath = await SaveFileAsync(birthCertificate, "certificate", viewModel.Student.Id, viewModel.Student.Name);
                    if (birthCertificatePath != null)
                    {
                        viewModel.Student.birth_Certificate = birthCertificatePath;
                    }
                    else
                    {
                        SetErrorMessage("فشل في رفع شهادة الميلاد");

                        // إعادة تحميل البيانات في حالة الخطأ
                        var sections = await _sectionService.GetActiveSectionsAsync();
                        viewModel.Sections = sections.Select(s => new SelectListItem
                        {
                            Value = s.Id.ToString(),
                            Text = s.Name_Of_Section
                        }).ToList();

                        return View(viewModel);
                    }
                }

                // تحديث الطالب مع مسارات الصور
                await _studentService.UpdateStudentAsync(viewModel.Student);

                // إضافة الطالب للقسم المحدد
                var section = await _sectionService.GetSectionByIdAsync(viewModel.SelectedSectionId); // أضافة await
                if (section != null)
                {
                    await _studentService.AddStudentWithoutClassAsync(viewModel.Student.Id, viewModel.SelectedSectionId); // أضافة await
                }

                SetSuccessMessage("تم إضافة الطالب بنجاح");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في حفظ البيانات: {ex.Message}");

                // إعادة تحميل البيانات في حالة الخطأ
                var sections = await _sectionService.GetActiveSectionsAsync();
                viewModel.Sections = sections.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name_Of_Section
                }).ToList();

                return View(viewModel);
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
        public async Task<IActionResult> Edit(Students student, IFormFile pictureProfile, IFormFile birthCertificate)
        {
  
            try
            {
           
                    // Get existing student to preserve file paths if no new files uploaded
                    var existingStudent = await _studentService.GetStudentByIdAsync(student.Id);
                    if (existingStudent == null)
                    {
                        SetErrorMessage("الطالب غير موجود");
                        return RedirectToAction(nameof(Index));
                    }

                    // استخراج البيانات من الرقم القومي
                    var nationalIdData = ExtractNationalIdData(student.Natrual_Id);
                    if (nationalIdData.IsValid)
                    {
                        student.Date_of_birth = nationalIdData.BirthDate.ToString();
                        student.Governate = nationalIdData.Governorate;
                    }
                    else
                    {
                        SetErrorMessage("الرقم القومي غير صحيح");
                        await PopulateViewBag();
                        return View(student);
                    }

                    // Handle Picture Profile Update
                    if (pictureProfile != null && pictureProfile.Length > 0)
                    {
                        // Delete old file if exists
                        DeleteFileIfExists(existingStudent.Picture_Profile);

                        var profilePicturePath = await SaveFileAsync(pictureProfile, "profile", student.Id, student.Name);
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

                        var birthCertificatePath = await SaveFileAsync(birthCertificate, "certificate", student.Id, student.Name);
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
                student.IsActive=true;
                    // Preserve creation info
                    student.CreatedBy = existingStudent.CreatedBy;
                    student.Date = existingStudent.Date;

                    await _studentService.UpdateStudentAsync(student);
                    SetSuccessMessage("تم تحديث بيانات الطالب بنجاح");
                    return RedirectToAction(nameof(Index));
                

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

        // Helper method to save files with student ID and name
        private async Task<string> SaveFileAsync(IFormFile file, string fileType, int studentId, string studentName)
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

                // Create directory structure: upload/studentId/
                var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "upload", studentId.ToString());
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Generate filename: studentId_studentName_fileType.extension
                var cleanStudentName = CleanFileName(studentName);
                var fileName = $"{studentId}_{cleanStudentName}_{fileType}{fileExtension}";
                var filePath = Path.Combine(uploadPath, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return relative path for database storage
                return Path.Combine("upload", studentId.ToString(), fileName).Replace("\\", "/");
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Helper method to clean filename
        private string CleanFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "student";

            var invalidChars = Path.GetInvalidFileNameChars();
            var cleanName = new string(fileName.Where(c => !invalidChars.Contains(c)).ToArray());
            return string.IsNullOrEmpty(cleanName) ? "student" : cleanName.Replace(" ", "_");
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
                var sections = await _sectionService.GetActiveSectionsAsync();
                ViewBag.Sections = sections; // إرسال البيانات الأصلية بدلاً من SelectList
            }
            catch (Exception)
            {
                ViewBag.Sections = new List<Section>(); // قائمة فارغة في حالة الخطأ
            }
        }

        // Helper method to extract data from Egyptian National ID
        private (DateTime BirthDate, string Governorate, bool IsValid) ExtractNationalIdData(string nationalId)
        {
            try
            {
                if (string.IsNullOrEmpty(nationalId) || nationalId.Length != 14 || !nationalId.All(char.IsDigit))
                {
                    return (DateTime.MinValue, "", false);
                }

                // استخراج تاريخ الميلاد (أول 6 أرقام)
                var birthYearStr = nationalId.Substring(1, 2);
                var birthMonthStr = nationalId.Substring(3, 2);
                var birthDayStr = nationalId.Substring(5, 2);

                // تحديد القرن بناء على الرقم الأول - التصحيح هنا
                int century = nationalId[0] == '2' ? 1900 : 2000;
                int year = century + int.Parse(birthYearStr);
                int month = int.Parse(birthMonthStr);
                int day = int.Parse(birthDayStr);

                // التحقق من صحة التاريخ
                if (month < 1 || month > 12 || day < 1 || day > 31)
                {
                    return (DateTime.MinValue, "", false);
                }

                var birthDate = new DateTime(year, month, day);

                // استخراج كود المحافظة (الرقم 8 و 9)
                var governorateCode = nationalId.Substring(7, 2);
                var governorate = GetGovernorateByCode(governorateCode);

                return (birthDate, governorate, true);
            }
            catch
            {
                return (DateTime.MinValue, "", false);
            }
        }
        // Helper method to get governorate name by code
        private string GetGovernorateByCode(string code)
        {
            var governorates = new Dictionary<string, string>
            {
                {"01", "القاهرة"},
                {"02", "الإسكندرية"},
                {"03", "بورسعيد"},
                {"04", "السويس"},
                {"11", "دمياط"},
                {"12", "الدقهلية"},
                {"13", "الشرقية"},
                {"14", "القليوبية"},
                {"15", "كفر الشيخ"},
                {"16", "الغربية"},
                {"17", "المنوفية"},
                {"18", "البحيرة"},
                {"19", "الإسماعيلية"},
                {"21", "الجيزة"},
                {"22", "بني سويف"},
                {"23", "الفيوم"},
                {"24", "المنيا"},
                {"25", "أسيوط"},
                {"26", "سوهاج"},
                {"27", "قنا"},
                {"28", "أسوان"},
                {"29", "الأقصر"},
                {"31", "البحر الأحمر"},
                {"32", "الوادي الجديد"},
                {"33", "مطروح"},
                {"34", "شمال سيناء"},
                {"35", "جنوب سيناء"},
                {"88", "خارج الجمهورية"}
            };

            return governorates.ContainsKey(code) ? governorates[code] : "غير معروف";
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
                    name = s.Name
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