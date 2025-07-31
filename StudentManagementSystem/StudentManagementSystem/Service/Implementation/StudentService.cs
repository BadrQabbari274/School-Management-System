using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Controllers;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Service.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public StudentService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IEnumerable<Students>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.CreatedBy)
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<Students> GetStudentByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.CreatedBy)
                .Include(s => s.TaskEvaluations)
                .Include(s => s.Pictures)
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
        }

        public async Task<Students> CreateStudentAsync(Students student, IFormFile profileImage = null, IFormFile birthCertificate = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // تعيين تاريخ الإنشاء
                student.Date = DateTime.Now;
                student.IsActive = true;

                // إضافة الطالب أولاً للحصول على ID
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                // إنشاء مجلد للطالب إذا كانت هناك صور
                if (profileImage != null || birthCertificate != null)
                {
                    await HandleStudentImagesAsync(student, profileImage, birthCertificate);
                }

                await transaction.CommitAsync();
                return student;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<Students> UpdateStudentAsync(Students student)
        {
            var existingStudent = await _context.Students.FindAsync(student.Id);
            if (existingStudent == null)
            {
                throw new InvalidOperationException("Student not found");
            }

            // Update only the properties that should change
            _context.Entry(existingStudent).CurrentValues.SetValues(student);

            // Preserve the file paths if they weren't updated
            if (string.IsNullOrEmpty(student.Picture_Profile))
            {
                student.Picture_Profile = existingStudent.Picture_Profile;
            }
            if (string.IsNullOrEmpty(student.birth_Certificate))
            {
                student.birth_Certificate = existingStudent.birth_Certificate;
            }

            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            student.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Students>> GetActiveStudentsAsync()
        {
            return await _context.Students
                .Where(s => s.IsActive)
         
                .ToListAsync();
        }

        public async Task<IEnumerable<Students>> GetStudentsByClassAsync(int classId)
        {
            return await _context.Students
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Students>> GetStudentsWithTasksAsync()
        {
            return await _context.Students
                .Include(s => s.TaskEvaluations)
                .Where(s => s.IsActive && s.TaskEvaluations.Any())
                .ToListAsync();
        }

        private async Task HandleStudentImagesAsync(Students student, IFormFile profileImage, IFormFile birthCertificate)
        {
            // جلب بيانات الصف والمجال
            var studentWithDetails = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == student.Id);

         
            // إنشاء مسار المجلد: Grade-Field-Class-Student(ID_Name)
            var folderPath = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                //$"Grade-{studentWithDetails.Class.Department.Name}",
                //$"Class-{studentWithDetails.Class.Name}",
                $"Student({studentWithDetails.Id}_{studentWithDetails.Name.Replace(" ", "_")})"
            );

            // إنشاء المجلد إذا لم يكن موجوداً
            Directory.CreateDirectory(folderPath);

            // رفع الصورة الشخصية
            if (profileImage != null && IsValidImageFile(profileImage))
            {
                var profileImagePath = await SaveImageAsync(profileImage, folderPath, "ProfileImage");
                studentWithDetails.Picture_Profile = profileImagePath;
            }

            // رفع شهادة الميلاد
            if (birthCertificate != null && IsValidImageFile(birthCertificate))
            {
                var birthCertPath = await SaveImageAsync(birthCertificate, folderPath, "BirthCertificate");
                studentWithDetails.birth_Certificate = birthCertPath;
            }
        }

        private async Task<string> SaveImageAsync(IFormFile file, string folderPath, string prefix)
        {
            try
            {
                // إنشاء اسم ملف فريد
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = $"{prefix}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
                var fullPath = Path.Combine(folderPath, fileName);

                // حفظ الملف
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // إرجاع المسار النسبي
                return fullPath.Replace(_environment.WebRootPath, "").Replace("\\", "/");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"فشل في رفع الملف {prefix}: {ex.Message}");
            }
        }

        private bool IsValidImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            // فحص امتداد الملف
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                return false;

            // فحص حجم الملف (5MB كحد أقصى)
            if (file.Length > 5 * 1024 * 1024)
                return false;

            // فحص نوع المحتوى
            var allowedMimeTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/bmp" };
            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
                return false;

            return true;
        }
        // إضافة طالب بدون فصل (فقط student_id, working_year_id, section_id)
        public async Task<bool> AddStudentWithoutClassAsync(int studentId, int sectionId, int? workingYearId = null)
        {
            try
            {
                // جلب آخر سنة دراسية نشطة إذا لم يتم تحديدها
                if (!workingYearId.HasValue)
                {
                    var activeWorkingYear = GetOrCreateActiveWorkingYearAsync();

                    if (activeWorkingYear == null)
                        return false;

                    workingYearId = activeWorkingYear.Id;
                }

                //// جلب آخر قسم نشط للطالب إذا لم يتم تحديده
                //if (!sectionId.HasValue)
                //{
                //    var lastSection = await _context.Student_Class_Section_Years
                //        .Where(scss => scss.Student_Id == studentId && scss.IsActive)
                //        .OrderByDescending(scss => scss.Date)
                //        .Select(scss => scss.Section_id)
                //        .FirstOrDefaultAsync();

                //    if (lastSection == 0)
                //        return false;

                //    sectionId = lastSection;
                //}

                // التحقق من عدم وجود تسجيل مسبق
                var existingRecord = await _context.Student_Class_Section_Years
                    .FirstOrDefaultAsync(scss => scss.Student_Id == studentId &&
                                               scss.Working_Year_Id == workingYearId &&
                                               scss.Section_id == sectionId);

                if (existingRecord != null)
                    return false; // الطالب مسجل بالفعل

                var studentRecord = new Student_Class_Section_Year
                {
                    Student_Id = studentId,
                    Working_Year_Id = workingYearId.Value,
                    Section_id = sectionId,
                    IsActive = true,
                    CreatedBy_Id = 1, // يمكن تمرير هذا كمعامل
                    Date = DateTime.Now,
                    Class_Id = null // بدون فصل في البداية
                };

                _context.Student_Class_Section_Years.Add(studentRecord);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // تحقق من وجود سنة دراسية نشطة وإنشاء واحدة إذا لم توجد
        public async Task<Working_Year> GetOrCreateActiveWorkingYearAsync()
        {
            // البحث عن سنة دراسية نشطة
            var activeWorkingYear = await _context.Working_Years
                .Where(wy => wy.IsActive)
                .OrderByDescending(wy => wy.Start_date)
                .FirstOrDefaultAsync();

            // إذا لم توجد، إنشاء واحدة جديدة
            if (activeWorkingYear == null)
            {
                var user = _context.Employees.Include(i => i.Role).FirstOrDefault(e => e.IsActive && e.Role.Name == "Admin");

                var currentYear = DateTime.Now.Year;
                activeWorkingYear = new Working_Year
                {
                    Name = $"العام الدراسي {currentYear}-{currentYear + 1}",
                    Start_date = new DateTime(currentYear, 9, 1), // بداية سبتمبر
                    End_date = new DateTime(currentYear + 1, 6, 30), // نهاية يونيو
                    IsActive = true,
                    CreatedBy_Id = user.Id , // استخدم ID المستخدم الحالي
                    Date = DateTime.Now
                };

                _context.Working_Years.Add(activeWorkingYear);
                await _context.SaveChangesAsync();
            }

            return activeWorkingYear;
        }

        // تحديث AssignGradeToStudentAsync لاستخدام الmethod الجديدة
        public async Task<bool> AssignGradeToStudentAsync(int studentId, int gradeId)
        {
            try
            {
                // التحقق من وجود الطالب
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.Id == studentId && s.IsActive);

                if (student == null)
                {
                    throw new ArgumentException($"الطالب بالرقم {studentId} غير موجود أو غير نشط");
                }

                // التحقق من وجود الصف
                var grade = await _context.Grades
                    .FirstOrDefaultAsync(g => g.Id == gradeId && g.IsActive);

                if (grade == null)
                {
                    throw new ArgumentException($"الصف بالرقم {gradeId} غير موجود أو غير نشط");
                }

                // الحصول على السنة الدراسية النشطة أو إنشاء واحدة
                var activeWorkingYear = await GetOrCreateActiveWorkingYearAsync();

                // التحقق من عدم وجود تسجيل مسبق للطالب في نفس السنة الدراسية
                var existingRecord = await _context.StudentGrades
                    .FirstOrDefaultAsync(sg => sg.StudentId == studentId
                                            && sg.Working_Year_Id == activeWorkingYear.Id
                                            && sg.IsActive);

                if (existingRecord != null)
                {
                    // تحديث الصف الحالي
                    existingRecord.GradeId = gradeId;
                    existingRecord.Date = DateTime.Now;
                    _context.StudentGrades.Update(existingRecord);
                }
                else
                {
                    // إنشاء سجل جديد
                    var studentGrade = new StudentGrades()
                    {
                        StudentId = studentId,
                        GradeId = gradeId,
                        Date = DateTime.Now,
                        Working_Year_Id = activeWorkingYear.Id,
                        IsActive = true
                    };

                    await _context.StudentGrades.AddAsync(studentGrade);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AssignGradeToStudentAsync: {ex.Message}");
                throw;
            }
        }

        // تعيين فصل لطالب
        public async Task<bool> AssignClassToStudentAsync(int studentId, int classId, int? workingYearId = null)
        {
            try
            {
                // جلب آخر سنة دراسية نشطة إذا لم يتم تحديدها
                if (!workingYearId.HasValue)
                {
                    var activeWorkingYear = await _context.Working_Years
                        .Where(wy => wy.IsActive)
                        .OrderByDescending(wy => wy.Start_date)
                        .FirstOrDefaultAsync();

                    if (activeWorkingYear == null)
                        return false;

                    workingYearId = activeWorkingYear.Id;
                }

                // البحث عن تسجيل الطالب الحالي
                var studentRecord = await _context.Student_Class_Section_Years
                    .FirstOrDefaultAsync(scss => scss.Student_Id == studentId &&
                                               scss.Working_Year_Id == workingYearId &&
                                               scss.IsActive);

                if (studentRecord == null)
                    return false;

                // التحقق من سعة الفصل
                var classInfo = await _context.Classes.FindAsync(classId);
                if (classInfo != null && classInfo.MaxStudents.HasValue)
                {
                    var currentStudentsCount = await _context.Student_Class_Section_Years
                        .CountAsync(scss => scss.Class_Id == classId &&
                                          scss.Working_Year_Id == workingYearId &&
                                          scss.IsActive);

                    if (currentStudentsCount >= classInfo.MaxStudents.Value)
                        return false; // الفصل ممتلئ
                }

                // تعيين الفصل
                studentRecord.Class_Id = classId;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // إضافة طالب مع كل التفاصيل
        public async Task<bool> AddStudentWithAllDetailsAsync(int studentId, int workingYearId, int sectionId, int classId, int createdById)
        {
            try
            {
                // التحقق من عدم وجود تسجيل مسبق
                var existingRecord = await _context.Student_Class_Section_Years
                    .FirstOrDefaultAsync(scss => scss.Student_Id == studentId &&
                                               scss.Working_Year_Id == workingYearId &&
                                               scss.Section_id == sectionId);

                if (existingRecord != null)
                    return false;

                // التحقق من سعة الفصل
                var classInfo = await _context.Classes.FindAsync(classId);
                if (classInfo != null && classInfo.MaxStudents.HasValue)
                {
                    var currentStudentsCount = await _context.Student_Class_Section_Years
                        .CountAsync(scss => scss.Class_Id == classId &&
                                          scss.Working_Year_Id == workingYearId &&
                                          scss.IsActive);

                    if (currentStudentsCount >= classInfo.MaxStudents.Value)
                        return false;
                }

                var studentRecord = new Student_Class_Section_Year
                {
                    Student_Id = studentId,
                    Working_Year_Id = workingYearId,
                    Section_id = sectionId,
                    Class_Id = classId,
                    IsActive = true,
                    CreatedBy_Id = createdById,
                    Date = DateTime.Now
                };

                _context.Student_Class_Section_Years.Add(studentRecord);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // جلب الطلاب مجمعين حسب القسم مع ترتيب أبجدي
        public async Task<List<SectionStudentsDto>> GetStudentsByDepartmentAsync(int? workingYearId = null)
        {
            try
            {
                // جلب آخر سنة دراسية نشطة إذا لم يتم تحديدها
                if (!workingYearId.HasValue)
                {
                    var activeWorkingYear = await _context.Working_Years
                        .Where(wy => wy.IsActive)
                        .OrderByDescending(wy => wy.Start_date)
                        .FirstOrDefaultAsync();

                    if (activeWorkingYear == null)
                        return new List<SectionStudentsDto>();

                    workingYearId = activeWorkingYear.Id;
                }

                var result = await _context.Student_Class_Section_Years
                    .Where(scss => scss.Working_Year_Id == workingYearId && scss.IsActive)
                    .Include(scss => scss.Student)
                    .Include(scss => scss.Section)
                        .ThenInclude(s => s.Department)
                    .Include(scss => scss.Class)
                    .Include(scss => scss.WorkingYear)
                    .GroupBy(scss => new {
                        DepartmentId = scss.Section.Department_Id,
                        DepartmentName = scss.Section.Department.Name,
                        SectionId = scss.Section_id,
                        SectionName = scss.Section.Name_Of_Section
                    })
                    .Select(g => new SectionStudentsDto
                    {
                        DepartmentId = g.Key.DepartmentId,
                        DepartmentName = g.Key.DepartmentName,
                        SectionId = g.Key.SectionId,
                        SectionName = g.Key.SectionName,
                        WorkingYearId = workingYearId.Value,
                        Students = g.Select(scss => new StudentInfoDto
                        {
                            StudentId = scss.Student_Id,
                            StudentName = scss.Student.Name,
                            StudentCode = scss.Student.Code,
                            ClassName = scss.Class != null ? scss.Class.Name : "غير محدد",
                            ClassId = scss.Class_Id,
                            Phone = scss.Student.Phone_Number,
                            Email = scss.Student.Email,
                            Address = scss.Student.Adress
                        }).OrderBy(s => s.StudentName).ToList()
                    })
                    .OrderBy(dept => dept.DepartmentName)
                    .ThenBy(dept => dept.SectionName)
                    .ToListAsync();

                return result;
            }
            catch
            {
                return new List<SectionStudentsDto>();
            }
        }

        // جلب الطلاب في قسم معين
        public async Task<List<StudentInfoDto>> GetStudentsInSectionAsync(int sectionId, int? workingYearId = null)
        {
            try
            {
                // جلب آخر سنة دراسية نشطة إذا لم يتم تحديدها
                if (!workingYearId.HasValue)
                {
                    var activeWorkingYear = await _context.Working_Years
                        .Where(wy => wy.IsActive)
                        .OrderByDescending(wy => wy.Start_date)
                        .FirstOrDefaultAsync();

                    if (activeWorkingYear == null)
                        return new List<StudentInfoDto>();

                    workingYearId = activeWorkingYear.Id;
                }

                var students = await _context.Student_Class_Section_Years
                    .Where(scss => scss.Section_id == sectionId &&
                                  scss.Working_Year_Id == workingYearId &&
                                  scss.IsActive)
                    .Include(scss => scss.Student)
                    .Include(scss => scss.Class)
                    .Select(scss => new StudentInfoDto
                    {
                        StudentId = scss.Student_Id,
                        StudentName = scss.Student.Name,
                        StudentCode = scss.Student.Code,
                        ClassName = scss.Class != null ? scss.Class.Name : "غير محدد",
                        ClassId = scss.Class_Id,
                        Phone = scss.Student.Phone_Number,
                        Email = scss.Student.Email,
                        Address = scss.Student.Adress
                    })
                    .OrderBy(s => s.StudentName)
                    .ToListAsync();

                return students;
            }
            catch
            {
                return new List<StudentInfoDto>();
            }
        }
        public async Task<IEnumerable<Classes>> GetClassesByGradeAsync(int gradeId)
        {
            return await _context.Classes
                .Where(c => c.GradeId == gradeId && c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
    }

    // DTOs للإرجاع
    public class SectionStudentsDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int WorkingYearId { get; set; }
        public List<StudentInfoDto> Students { get; set; } = new List<StudentInfoDto>();
    }

    public class StudentInfoDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string ClassName { get; set; }
        public int? ClassId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
