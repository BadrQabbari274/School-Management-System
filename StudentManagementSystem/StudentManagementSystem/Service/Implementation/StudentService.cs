//using Microsoft.EntityFrameworkCore;
//using StudentManagementSystem.Data;
//using StudentManagementSystem.Models;
//using StudentManagementSystem.Service.Interface;

//namespace StudentManagementSystem.Service.Implementation
//{
//    public class StudentService : IStudentService
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly IWebHostEnvironment _environment;

//        public StudentService(ApplicationDbContext context, IWebHostEnvironment environment)
//        {
//            _context = context;
//            _environment = environment;
//        }

//        public async Task<IEnumerable<Students>> GetAllStudentsAsync()
//        {
//            return await _context.Students
//                .Include(s => s.CreatedByUser)
//                .Include(s => s.Class)
//                .ThenInclude(c => c.Field)
//                .Where(s => s.IsActive)
//                .ToListAsync();
//        }

//        public async Task<Student> GetStudentByIdAsync(int id)
//        {
//            return await _context.Students
//                .Include(s => s.CreatedByUser)
//                .Include(s => s.Class)
//                .ThenInclude(c => c.Field)
//                .Include(s => s.TaskEvaluations)
//                .Include(s => s.Pictures)
//                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
//        }

//        public async Task<Student> CreateStudentAsync(Student student, IFormFile profileImage = null, IFormFile birthCertificate = null)
//        {
//            using var transaction = await _context.Database.BeginTransactionAsync();
//            try
//            {
//                // تعيين تاريخ الإنشاء
//                student.Date = DateTime.Now;
//                student.IsActive = true;

//                // إضافة الطالب أولاً للحصول على ID
//                _context.Students.Add(student);
//                await _context.SaveChangesAsync();

//                // إنشاء مجلد للطالب إذا كانت هناك صور
//                if (profileImage != null || birthCertificate != null)
//                {
//                    await HandleStudentImagesAsync(student, profileImage, birthCertificate);
//                }

//                await transaction.CommitAsync();
//                return student;
//            }
//            catch (Exception)
//            {
//                await transaction.RollbackAsync();
//                throw;
//            }
//        }

//        public async Task<Student> UpdateStudentAsync(Student student, IFormFile profileImage = null, IFormFile birthCertificate = null)
//        {
//            using var transaction = await _context.Database.BeginTransactionAsync();
//            try
//            {
//                _context.Entry(student).State = EntityState.Modified;

//                // تحديث الصور إذا تم رفع صور جديدة
//                if (profileImage != null || birthCertificate != null)
//                {
//                    await HandleStudentImagesAsync(student, profileImage, birthCertificate);
//                }

//                await _context.SaveChangesAsync();
//                await transaction.CommitAsync();
//                return student;
//            }
//            catch (Exception)
//            {
//                await transaction.RollbackAsync();
//                throw;
//            }
//        }

//        public async Task<bool> DeleteStudentAsync(int id)
//        {
//            var student = await _context.Students.FindAsync(id);
//            if (student == null) return false;

//            student.IsActive = false;
//            await _context.SaveChangesAsync();
//            return true;
//        }

//        public async Task<IEnumerable<Student>> GetActiveStudentsAsync()
//        {
//            return await _context.Students
//                .Where(s => s.IsActive)
//                .Include(s => s.Class)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Student>> GetStudentsByClassAsync(int classId)
//        {
//            return await _context.Students
//                .Where(s => s.ClassId == classId && s.IsActive)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Student>> GetStudentsWithTasksAsync()
//        {
//            return await _context.Students
//                .Include(s => s.TaskEvaluations)
//                .Where(s => s.IsActive && s.TaskEvaluations.Any())
//                .ToListAsync();
//        }

//        private async Task HandleStudentImagesAsync(Students student, IFormFile profileImage, IFormFile birthCertificate)
//        {
//            // جلب بيانات الصف والمجال
//            var studentWithDetails = await _context.Students
//                .Include(s => s.Class)
//                .ThenInclude(c => c.Field)
//                .FirstOrDefaultAsync(s => s.Id == student.Id);

//            if (studentWithDetails?.Class?.Field == null)
//                throw new InvalidOperationException("يجب تحديد الصف والمجال قبل رفع الصور");

//            // إنشاء مسار المجلد: Grade-Field-Class-Student(ID_Name)
//            var folderPath = Path.Combine(
//                _environment.WebRootPath,
//                "uploads",
//                $"Grade-{studentWithDetails.Class.Field.Name}",
//                $"Class-{studentWithDetails.Class.Name}",
//                $"Student({studentWithDetails.Id}_{studentWithDetails.Name.Replace(" ", "_")})"
//            );

//            // إنشاء المجلد إذا لم يكن موجوداً
//            Directory.CreateDirectory(folderPath);

//            // رفع الصورة الشخصية
//            if (profileImage != null && IsValidImageFile(profileImage))
//            {
//                var profileImagePath = await SaveImageAsync(profileImage, folderPath, "ProfileImage");
//                studentWithDetails.Picture_Profile = profileImagePath;
//            }

//            // رفع شهادة الميلاد
//            if (birthCertificate != null && IsValidImageFile(birthCertificate))
//            {
//                var birthCertPath = await SaveImageAsync(birthCertificate, folderPath, "BirthCertificate");
//                studentWithDetails.birth_Certificate = birthCertPath;
//            }
//        }

//        private async Task<string> SaveImageAsync(IFormFile file, string folderPath, string prefix)
//        {
//            try
//            {
//                // إنشاء اسم ملف فريد
//                var fileExtension = Path.GetExtension(file.FileName);
//                var fileName = $"{prefix}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
//                var fullPath = Path.Combine(folderPath, fileName);

//                // حفظ الملف
//                using (var stream = new FileStream(fullPath, FileMode.Create))
//                {
//                    await file.CopyToAsync(stream);
//                }

//                // إرجاع المسار النسبي
//                return fullPath.Replace(_environment.WebRootPath, "").Replace("\\", "/");
//            }
//            catch (Exception ex)
//            {
//                throw new InvalidOperationException($"فشل في رفع الملف {prefix}: {ex.Message}");
//            }
//        }

//        private bool IsValidImageFile(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//                return false;

//            // فحص امتداد الملف
//            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
//            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

//            if (!allowedExtensions.Contains(fileExtension))
//                return false;

//            // فحص حجم الملف (5MB كحد أقصى)
//            if (file.Length > 5 * 1024 * 1024)
//                return false;

//            // فحص نوع المحتوى
//            var allowedMimeTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/bmp" };
//            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
//                return false;

//            return true;
//        }
//    }
//}