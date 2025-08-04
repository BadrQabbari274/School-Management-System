//using Microsoft.EntityFrameworkCore;
//using StudentManagementSystem.Data;
//using StudentManagementSystem.Models;
//using StudentManagementSystem.ViewModels;
//using StudentManagementSystem.Service.Interface;

//namespace StudentManagementSystem.Service.Implementation
//{
//    public class ClassRegistrationService : IClassRegistrationService
//    {
//        private readonly ApplicationDbContext _context;

//        public ClassRegistrationService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<List<ClassWithStudentCountDto>> GetClassesByGradeWithStudentCountAsync(int gradeId)
//        {
//            try
//            {
//                var grade = await _context.Grades.FirstOrDefaultAsync(g => g.Id == gradeId && g.IsActive);
//                if (grade == null) return new List<ClassWithStudentCountDto>();

//                // الحصول على السنة الدراسية النشطة
//                var activeWorkingYear = await GetActiveWorkingYearAsync();
//                if (activeWorkingYear == null) return new List<ClassWithStudentCountDto>();

//                // جلب الفصول التي تبدأ بنفس حرف الصف
//                var gradePrefix = grade.Name.Substring(0, 1).ToUpper();

//                var classes = await _context.Classes
//                    .Where(c => c.IsActive && c.Name.StartsWith(gradePrefix))
//                    .Select(c => new ClassWithStudentCountDto
//                    {
//                        ClassId = c.Id,
//                        ClassName = c.Name,
//                        MaxStudents = c.MaxStudents,
//                        CurrentStudentCount = _context.Student_Class_Section_Years
//                            .Count(scsy => scsy.Class_Id == c.Id
//                                         && scsy.Working_Year_Id == activeWorkingYear.Id
//                                         && scsy.IsActive)
//                    })
//                    .OrderBy(c => c.ClassName)
//                    .ToListAsync();

//                return classes;
//            }
//            catch
//            {
//                return new List<ClassWithStudentCountDto>();
//            }
//        }

//        public async Task<ClassRegistrationViewModel> GetJuniorStudentsAsync(int gradeId, int classId)
//        {
//            try
//            {
//                var grade = await _context.Grades.FirstOrDefaultAsync(g => g.Id == gradeId && g.IsActive);
//                var classEntity = await _context.Classes.FirstOrDefaultAsync(c => c.Id == classId && c.IsActive);
//                var activeWorkingYear = await GetActiveWorkingYearAsync();

//                if (grade == null || classEntity == null || activeWorkingYear == null)
//                    return new ClassRegistrationViewModel();

//                // جلب الأقسام النشطة
//                var sections = await _context.Sections
//                    .Include(s => s.Department)
//                    .Where(s => s.IsActive)
//                    .OrderBy(s => s.Name_Of_Section)
//                    .ToListAsync();

//                // جلب الطلاب المسجلين بالفعل في الفصل
//                var registeredStudents = await _context.Student_Class_Section_Years
//                    .Where(scsy => scsy.Class_Id == classId
//                                  && scsy.Working_Year_Id == activeWorkingYear.Id
//                                  && scsy.IsActive)
//                    .Include(scsy => scsy.Student)
//                    .Include(scsy => scsy.Section)
//                    .Select(scsy => new StudentInfoDto
//                    {
//                        StudentId = scsy.Student_Id,
//                        StudentName = scsy.Student.Name,
//                        StudentCode = scsy.Student.Code,
//                        Phone = scsy.Student.Phone_Number,
//                        Email = scsy.Student.Email,
//                        Address = scsy.Student.Adress,
//                        SectionName = scsy.Section.Name_Of_Section,
//                        SectionId = scsy.Section_id,
//                        ClassId = scsy.Class_Id,
//                        ClassName = classEntity.Name
//                    })
//                    .OrderBy(s => s.StudentName)
//                    .ToListAsync();

//                // جلب الطلاب المؤهلين للتسجيل (مسجلين في Junior ولكن بدون فصل)
//                var availableStudents = await _context.Student_Class_Section_Years
//                    .Where(scsy => scsy.Working_Year_Id == activeWorkingYear.Id
//                                  && scsy.IsActive
//                                  && scsy.Class_Id == null) // بدون فصل
//                    .Join(_context.StudentGrades,
//                          scsy => scsy.Student_Id,
//                          sg => sg.StudentId,
//                          (scsy, sg) => new { scsy, sg })
//                    .Where(joined => joined.sg.GradeId == gradeId
//                                    && joined.sg.Working_Year_Id == activeWorkingYear.Id
//                                    && joined.sg.IsActive)
//                    .Include(joined => joined.scsy.Student)
//                    .Include(joined => joined.scsy.Section)
//                    .Select(joined => new StudentInfoDto
//                    {
//                        StudentId = joined.scsy.Student_Id,
//                        StudentName = joined.scsy.Student.Name,
//                        StudentCode = joined.scsy.Student.Code,
//                        Phone = joined.scsy.Student.Phone_Number,
//                        Email = joined.scsy.Student.Email,
//                        Address = joined.scsy.Student.Adress,
//                        SectionName = joined.scsy.Section.Name_Of_Section,
//                        SectionId = joined.scsy.Section_id
//                    })
//                    .OrderBy(s => s.StudentName)
//                    .ToListAsync();

//                return new ClassRegistrationViewModel
//                {
//                    Grade = grade,
//                    Class = classEntity,
//                    AvailableStudents = availableStudents,
//                    RegisteredStudents = registeredStudents,
//                    Sections = sections
//                };
//            }
//            catch
//            {
//                return new ClassRegistrationViewModel();
//            }
//        }

//        public async Task<ClassRegistrationViewModel> GetAdvancedGradeStudentsAsync(int gradeId, int classId)
//        {
//            try
//            {
//                var grade = await _context.Grades.FirstOrDefaultAsync(g => g.Id == gradeId && g.IsActive);
//                var classEntity = await _context.Classes.FirstOrDefaultAsync(c => c.Id == classId && c.IsActive);
//                var activeWorkingYear = await GetActiveWorkingYearAsync();

//                if (grade == null || classEntity == null || activeWorkingYear == null)
//                    return new ClassRegistrationViewModel();

//                // تحديد الصف السابق
//                string previousGradeName = grade.Name.ToLower() == "wheeler" ? "junior" : "wheeler";

//                // جلب الأقسام النشطة
//                var sections = await _context.Sections
//                    .Include(s => s.Department)
//                    .Where(s => s.IsActive)
//                    .OrderBy(s => s.Name_Of_Section)
//                    .ToListAsync();

//                // جلب الطلاب المسجلين بالفعل في الفصل
//                var registeredStudents = await _context.Student_Class_Section_Years
//                    .Where(scsy => scsy.Class_Id == classId
//                                  && scsy.Working_Year_Id == activeWorkingYear.Id
//                                  && scsy.IsActive)
//                    .Include(scsy => scsy.Student)
//                    .Include(scsy => scsy.Section)
//                    .Include(scsy => scsy.Class)
//                    .Select(scsy => new StudentInfoDto
//                    {
//                        StudentId = scsy.Student_Id,
//                        StudentName = scsy.Student.Name,
//                        StudentCode = scsy.Student.Code,
//                        Phone = scsy.Student.Phone_Number,
//                        Email = scsy.Student.Email,
//                        Address = scsy.Student.Adress,
//                        SectionName = scsy.Section.Name_Of_Section,
//                        SectionId = scsy.Section_id,
//                        ClassId = scsy.Class_Id,
//                        ClassName = scsy.Class.Name
//                    })
//                    .OrderBy(s => s.StudentName)
//                    .ToListAsync();

//                // جلب الطلاب المؤهلين (الذين كانوا في الصف السابق في السنة الدراسية السابقة)
//                var availableStudents = await GetEligibleStudentsForAdvancedGradeAsync(gradeId, previousGradeName);

//                return new ClassRegistrationViewModel
//                {
//                    Grade = grade,
//                    Class = classEntity,
//                    AvailableStudents = availableStudents,
//                    RegisteredStudents = registeredStudents,
//                    Sections = sections
//                };
//            }
//            catch
//            {
//                return new ClassRegistrationViewModel();
//            }
//        }

//        public async Task<bool> RegisterJuniorStudentsAsync(List<int> studentIds, int classId, int createdById)
//        {
//            using var transaction = await _context.Database.BeginTransactionAsync();
//            try
//            {
//                var activeWorkingYear = await GetActiveWorkingYearAsync();
//                if (activeWorkingYear == null) return false;

//                foreach (var studentId in studentIds)
//                {
//                    // العثور على سجل الطالب في Student_Class_Section_Year
//                    var studentRecord = await _context.Student_Class_Section_Years
//                        .FirstOrDefaultAsync(scsy => scsy.Student_Id == studentId
//                                                   && scsy.Working_Year_Id == activeWorkingYear.Id
//                                                   && scsy.IsActive
//                                                   && scsy.Class_Id == null);

//                    if (studentRecord != null)
//                    {
//                        // تحديث السجل لإضافة الفصل
//                        studentRecord.Class_Id = classId;
//                        _context.Student_Class_Section_Years.Update(studentRecord);
//                    }
//                }

//                await _context.SaveChangesAsync();
//                await transaction.CommitAsync();
//                return true;
//            }
//            catch
//            {
//                await transaction.RollbackAsync();
//                return false;
//            }
//        }

//        public async Task<bool> RegisterAdvancedGradeStudentsAsync(List<int> studentIds, int gradeId, int classId, int sectionId, int createdById)
//        {
//            using var transaction = await _context.Database.BeginTransactionAsync();
//            try
//            {
//                var activeWorkingYear = await GetActiveWorkingYearAsync();
//                if (activeWorkingYear == null) return false;

//                foreach (var studentId in studentIds)
//                {
//                    // إنشاء سجل جديد في Student_Class_Section_Year
//                    var newRecord = new Student_Class_Section_Year
//                    {
//                        Student_Id = studentId,
//                        Working_Year_Id = activeWorkingYear.Id,
//                        Section_id = sectionId,
//                        Class_Id = classId,
//                        IsActive = true,
//                        CreatedBy_Id = createdById,
//                        Date = DateTime.Now
//                    };

//                    _context.Student_Class_Section_Years.Add(newRecord);

//                    // إنشاء سجل جديد في StudentGrades
//                    var studentGrade = new StudentGrades
//                    {
//                        StudentId = studentId,
//                        GradeId = gradeId,
//                        Working_Year_Id = activeWorkingYear.Id,
//                        Date = DateTime.Now,
//                        IsActive = true
//                    };

//                    _context.StudentGrades.Add(studentGrade);
//                }

//                await _context.SaveChangesAsync();
//                await transaction.CommitAsync();
//                return true;
//            }
//            catch
//            {
//                await transaction.RollbackAsync();
//                return false;
//            }
//        }

//        public async Task<List<StudentInfoDto>> GetStudentsBySectionForAdvancedGradeAsync(int sectionId, int gradeId)
//        {
//            try
//            {
//                var grade = await _context.Grades.FirstOrDefaultAsync(g => g.Id == gradeId && g.IsActive);
//                if (grade == null) return new List<StudentInfoDto>();

//                string previousGradeName = grade.Name.ToLower() == "wheeler" ? "junior" : "wheeler";
//                var eligibleStudents = await GetEligibleStudentsForAdvancedGradeAsync(gradeId, previousGradeName);

//                // فلترة حسب القسم
//                return eligibleStudents.Where(s => s.SectionId == sectionId).ToList();
//            }
//            catch
//            {
//                return new List<StudentInfoDto>();
//            }
//        }

//        public async Task<List<StudentInfoDto>> GetEligibleStudentsForAdvancedGradeAsync(int targetGradeId, string previousGradeName)
//        {
//            try
//            {
//                var activeWorkingYear = await GetActiveWorkingYearAsync();
//                var previousWorkingYear = await GetPreviousWorkingYearAsync();

//                if (activeWorkingYear == null || previousWorkingYear == null)
//                    return new List<StudentInfoDto>();

//                var previousGrade = await _context.Grades
//                    .FirstOrDefaultAsync(g => g.Name.ToLower() == previousGradeName.ToLower() && g.IsActive);

//                if (previousGrade == null) return new List<StudentInfoDto>();

//                // جلب الطلاب الذين كانوا مسجلين في الصف السابق في السنة الدراسية السابقة
//                var eligibleStudents = await _context.Student_Class_Section_Years
//                    .Where(scsy => scsy.Working_Year_Id == previousWorkingYear.Id && scsy.IsActive)
//                    .Join(_context.StudentGrades,
//                          scsy => scsy.Student_Id,
//                          sg => sg.StudentId,
//                          (scsy, sg) => new { scsy, sg })
//                    .Where(joined => joined.sg.GradeId == previousGrade.Id
//                                    && joined.sg.Working_Year_Id == previousWorkingYear.Id
//                                    && joined.sg.IsActive)
//                    .Where(joined => !_context.Student_Class_Section_Years
//                        .Any(currentScsy => currentScsy.Student_Id == joined.scsy.Student_Id
//                                           && currentScsy.Working_Year_Id == activeWorkingYear.Id
//                                           && currentScsy.IsActive)) // لم يتم تسجيلهم في السنة الحالية
//                    .Include(joined => joined.scsy.Student)
//                    .Include(joined => joined.scsy.Section)
//                    .Include(joined => joined.scsy.Class)
//                    .Select(joined => new StudentInfoDto
//                    {
//                        StudentId = joined.scsy.Student_Id,
//                        StudentName = joined.scsy.Student.Name,
//                        StudentCode = joined.scsy.Student.Code,
//                        Phone = joined.scsy.Student.Phone_Number,
//                        Email = joined.scsy.Student.Email,
//                        Address = joined.scsy.Student.Adress,
//                        SectionName = joined.scsy.Section.Name_Of_Section,
//                        SectionId = joined.scsy.Section_id,
//                        PreviousClassName = joined.scsy.Class != null ? joined.scsy.Class.Name : "غير محدد",
//                        PreviousClassId = joined.scsy.Class_Id
//                    })
//                    .OrderBy(s => s.StudentName)
//                    .ToListAsync();

//                return eligibleStudents;
//            }
//            catch
//            {
//                return new List<StudentInfoDto>();
//            }
//        }

//        private async Task<Working_Year> GetActiveWorkingYearAsync()
//        {
//            return await _context.Working_Years
//                .Where(wy => wy.IsActive)
//                .OrderByDescending(wy => wy.Start_date)
//                .FirstOrDefaultAsync();
//        }

//        private async Task<Working_Year> GetPreviousWorkingYearAsync()
//        {
//            return await _context.Working_Years
//                .Where(wy => wy.IsActive)
//                .OrderByDescending(wy => wy.Start_date)
//                .Skip(1)
//                .FirstOrDefaultAsync();
//        }
//    }
//}