 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Controllers;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
using StudentManagementSystem.ViewModels;
using System.Security.Claims;


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
        public async Task<Grades> GetJunior()
        {
            var result = await _context.Grades.FirstOrDefaultAsync(s => s.Name.ToLower() == "junior"&& s.IsActive);
            return result;
        }

        public async Task<Grades> GetSenior()
        {
            var result = await _context.Grades.FirstOrDefaultAsync(s => s.Name.ToLower() == "senior"&&s.IsActive);
            return result;
        }

        public async Task<Grades> GetWheeler()
        {
            var result = await _context.Grades.FirstOrDefaultAsync(s => s.Name.ToLower() == "wheeler" &&s.IsActive);
            return result;
        }
        public async Task<int> GatReasonId()
        {
          var reason =await   _context.AbsenceReasons.FirstOrDefaultAsync(r => r.Name =="غياب يومي"&&!r.IsDeleted);
            return reason.Id;
        }
        public async Task<AttendanceViewModel> GetStudentsAsync(int classId)
        {
            try
            {
                // الحصول على السنة الدراسية النشطة
                var activeWorkingYear = await _context.Working_Years
                    .Where(wy => wy.IsActive)
                    .OrderByDescending(wy => wy.Start_date)
                    .FirstOrDefaultAsync();

                if (activeWorkingYear == null)
                    return null;

                // الحصول على معلومات الفصل
                Classes Class = await _context.Classes.FirstOrDefaultAsync(e => e.Id == classId);

                if (Class == null)
                    return null;

                // الحصول على التاريخ بدون الوقت
                var dateOnly = DateTime.Now.Date;

                // الحصول على معرف نوع الحضور اليومي
                var dailyAttendanceTypeId = GetDailyAttendanceTypeId();

                // الحصول على الطلاب في الفصل المحدد خلال السنة النشطة
                var studentsInClass = await _context.Student_Class_Section_Years
                    .Include(scsy => scsy.Student)
                    .Where(scsy => scsy.Class_Id == classId &&
                                  scsy.Working_Year_Id == activeWorkingYear.Id &&
                                  scsy.IsActive)
                    .ToListAsync();

                var studentStatusList = new List<StudentStatusViewModel>();

                foreach (var studentClassSection in studentsInClass)
                {

                    // فحص حالة الطالب في الغياب اليومي لهذا اليوم
                    var dailyAttendance = await _context.StudentAttendances
                        .Where(a => a.StudentClassSectionYear_Student_Id == studentClassSection.Student_Id &&
                                   a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                   a.Class_Id == classId &&
                                   a.StudentClassSectionYear_Section_id == studentClassSection.Section_id &&
                                   a.Date.Date == dateOnly &&
                                   a.AttendanceTypeId == dailyAttendanceTypeId &&
                                   !a.IsDeleted)
                        .FirstOrDefaultAsync();

                    var dailyAbsence = await _context.StudentAbsents
                        .Where(a => a.StudentClassSectionYear_Student_Id == studentClassSection.Student_Id &&
                                   a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                   a.Class_Id == classId &&
                                   a.StudentClassSectionYear_Section_id == studentClassSection.Section_id &&
                                   a.Date.Date == dateOnly &&
                                   a.AttendanceTypeId == dailyAttendanceTypeId &&
                                   !a.IsDeleted)
                        .FirstOrDefaultAsync();

                    // تحديد حالة الطالب بناء على التسجيل
                    bool studentStatus;
                    int? reasonId = null;

                    if (dailyAttendance != null)
                    {
                        // الطالب مسجل حضور
                        studentStatus = true;
                    }
                    else if (dailyAbsence != null)
                    {
                        // الطالب مسجل غياب
                        studentStatus = false;
                        if (dailyAbsence.AbsenceReasonId.HasValue)
                        {
                            reasonId = dailyAbsence.AbsenceReasonId.Value;
                        }
                    }
                    else
                    {
                        // الطالب غير مسجل (لا حضور ولا غياب)
                        studentStatus = false;
                    }

                    var studentStatusViewModel = new StudentStatusViewModel
                    {
                        Students = studentClassSection.Student,
                        Status = studentStatus,
                        Reason_Id = reasonId
                    };

                    studentStatusList.Add(studentStatusViewModel);
                }

                // الحصول على أسباب الغياب من قاعدة البيانات
                var absenceReasons = await _context.AbsenceReasons
                    .Where(r => !r.IsDeleted)
                    .OrderBy(r => r.Name)
                    .ToListAsync();

                var attendanceViewModel = new AttendanceViewModel()
                {
                    Class = Class,
                    StudentStatus = studentStatusList,
                    reasons = absenceReasons
                };

                return attendanceViewModel;
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ
                // _logger.LogError(ex, "Error getting students attendance");
                return null;
            }
        }
        public async Task<AttendanceViewModel> GetStudentsFieldAsync(int classId)
        {
            try
            {
                // الحصول على السنة الدراسية النشطة
                var activeWorkingYear = await _context.Working_Years
                    .Where(wy => wy.IsActive)
                    .OrderByDescending(wy => wy.Start_date)
                    .FirstOrDefaultAsync();

                if (activeWorkingYear == null)
                    return null;

                // الحصول على معلومات الفصل
                Classes Class = await _context.Classes.FirstOrDefaultAsync(e => e.Id == classId);

                if (Class == null)
                    return null;

                // الحصول على التاريخ بدون الوقت
                var dateOnly = DateTime.Now.Date;

                // الحصول على معرف نوع الحضور اليومي والميداني
                var dailyAttendanceTypeId = GetDailyAttendanceTypeId();
                var fieldAttendanceTypeId = GetFieldAttendanceTypeId();

                // التحقق من وجود حضور يومي للفصل في هذا اليوم
                var hasDailyAttendanceToday = await _context.StudentAttendances
                    .AnyAsync(a => a.Class_Id == classId &&
                                  a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                  a.Date.Date == dateOnly &&
                                  a.AttendanceTypeId == dailyAttendanceTypeId &&
                                  !a.IsDeleted) ||
                    await _context.StudentAbsents
                    .AnyAsync(a => a.Class_Id == classId &&
                                  a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                  a.Date.Date == dateOnly &&
                                  a.AttendanceTypeId == dailyAttendanceTypeId &&
                                  !a.IsDeleted);

                // إذا لم يتم تسجيل الحضور اليومي، إرجاع رسالة خطأ
                if (!hasDailyAttendanceToday)
                {
                    return new AttendanceViewModel
                    {
                        Class = Class,
                        StudentStatus = new List<StudentStatusViewModel>(),
                        reasons = new List<AbsenceReasons>(),
                        ErrorMessage = "يجب تسجيل الحضور اليومي أولاً قبل تسجيل الحضور الميداني"
                    };
                }

                // الحصول على جميع الطلاب في الفصل
                var studentsInClass = await _context.Student_Class_Section_Years
                    .Include(scsy => scsy.Student)
                    .Where(scsy => scsy.Class_Id == classId &&
                                  scsy.Working_Year_Id == activeWorkingYear.Id &&
                                  scsy.IsActive)
                    .ToListAsync();

                // فحص إذا كان هناك أي تسجيل غياب ميداني للفصل اليوم
                var hasFieldAttendanceToday = await _context.StudentAttendances
                    .AnyAsync(a => a.Class_Id == classId &&
                                  a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                  a.Date.Date == dateOnly &&
                                  a.AttendanceTypeId == fieldAttendanceTypeId &&
                                  !a.IsDeleted) ||
                    await _context.StudentAbsents
                    .AnyAsync(a => a.Class_Id == classId &&
                                  a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                  a.Date.Date == dateOnly &&
                                  a.AttendanceTypeId == fieldAttendanceTypeId &&
                                  !a.IsDeleted);

                // الحصول على معرف سبب الغياب اليومي
                var dailyAbsenceReasonId = await GatReasonId();
                var dailyAbsenceReason = await _context.AbsenceReasons
                    .FirstOrDefaultAsync(r => r.Id == dailyAbsenceReasonId);

                var studentStatusList = new List<StudentStatusViewModel>();

                foreach (var studentClassSection in studentsInClass)
                {
                    var studentStatus = new StudentStatusViewModel
                    {
                        Students = studentClassSection.Student,
                        Status = true, // افتراضي حاضر
                        Reason_Id = null,
                        IsEditable = true // افتراضي قابل للتعديل
                    };

                    if (hasFieldAttendanceToday)
                    {
                        // إذا كان هناك تسجيل غياب ميداني، استخدم حالة الغياب الميداني
                        var fieldAttendance = await _context.StudentAttendances
                            .Where(a => a.StudentClassSectionYear_Student_Id == studentClassSection.Student_Id &&
                                       a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                       a.Class_Id == classId &&
                                       a.StudentClassSectionYear_Section_id == studentClassSection.Section_id &&
                                       a.Date.Date == dateOnly &&
                                       a.AttendanceTypeId == fieldAttendanceTypeId &&
                                       !a.IsDeleted)
                            .FirstOrDefaultAsync();

                        var fieldAbsence = await _context.StudentAbsents
                            .Include(a => a.AbsenceReason)
                            .Where(a => a.StudentClassSectionYear_Student_Id == studentClassSection.Student_Id &&
                                       a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                       a.Class_Id == classId &&
                                       a.StudentClassSectionYear_Section_id == studentClassSection.Section_id &&
                                       a.Date.Date == dateOnly &&
                                       a.AttendanceTypeId == fieldAttendanceTypeId &&
                                       !a.IsDeleted)
                            .FirstOrDefaultAsync();

                        if (fieldAttendance != null)
                        {
                            studentStatus.Status = true;
                            studentStatus.IsEditable = true;
                        }
                        else if (fieldAbsence != null)
                        {
                            studentStatus.Status = false;
                            studentStatus.Reason_Id = fieldAbsence.AbsenceReasonId;

                            // تحديد الاسم والقابلية للتعديل بناءً على نوع السبب
                            if (fieldAbsence.AbsenceReasonId == dailyAbsenceReasonId)
                            {
                                studentStatus.AbsenceReason = dailyAbsenceReason?.Name ?? "غياب يومي";
                                studentStatus.IsEditable = false; // غير قابل للتعديل
                            }
                            else
                            {
                                studentStatus.AbsenceReason = fieldAbsence.AbsenceReason?.Name;
                                studentStatus.CustomReason = fieldAbsence.CustomReasonDetails;
                                studentStatus.IsEditable = true; // قابل للتعديل
                            }
                        }
                    }
                    else
                    {
                        // إذا لم يكن هناك تسجيل غياب ميداني، استخدم حالة الغياب اليومي
                        var dailyAttendance = await _context.StudentAttendances
                            .Where(a => a.StudentClassSectionYear_Student_Id == studentClassSection.Student_Id &&
                                       a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                       a.Class_Id == classId &&
                                       a.StudentClassSectionYear_Section_id == studentClassSection.Section_id &&
                                       a.Date.Date == dateOnly &&
                                       a.AttendanceTypeId == dailyAttendanceTypeId &&
                                       !a.IsDeleted)
                            .FirstOrDefaultAsync();

                        var dailyAbsence = await _context.StudentAbsents
                            .Include(a => a.AbsenceReason)
                            .Where(a => a.StudentClassSectionYear_Student_Id == studentClassSection.Student_Id &&
                                       a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                       a.Class_Id == classId &&
                                       a.StudentClassSectionYear_Section_id == studentClassSection.Section_id &&
                                       a.Date.Date == dateOnly &&
                                       a.AttendanceTypeId == dailyAttendanceTypeId &&
                                       !a.IsDeleted)
                            .FirstOrDefaultAsync();

                        // تحديد الحالة بناءً على الغياب اليومي
                        if (dailyAttendance != null)
                        {
                            studentStatus.Status = true;
                            studentStatus.IsEditable = true;
                        }
                        else if (dailyAbsence != null)
                        {
                            studentStatus.Status = false;
                            studentStatus.Reason_Id = dailyAbsenceReasonId;
                            studentStatus.AbsenceReason = dailyAbsenceReason?.Name ?? "غياب يومي";
                            studentStatus.IsEditable = false; // غير قابل للتعديل لأنه غياب يومي
                        }
                        else
                        {
                            // لم يتم تسجيل حضور أو غياب يومي - افتراضي حاضر
                            studentStatus.Status = true;
                            studentStatus.IsEditable = true;
                        }
                    }

                    studentStatusList.Add(studentStatus);
                }

                // الحصول على أسباب الغياب من قاعدة البيانات (استثناء الغياب اليومي من القائمة)
                var absenceReasons = await _context.AbsenceReasons
                    .Where(r => !r.IsDeleted && r.Id != dailyAbsenceReasonId)
                    .OrderBy(r => r.Name)
                    .ToListAsync();

                var attendanceViewModel = new AttendanceViewModel()
                {
                    Class = Class,
                    StudentStatus = studentStatusList,
                    reasons = absenceReasons
                };

                return attendanceViewModel;
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ
                return null;
            }
        }
        // دالة للحصول على معرف نوع الحضور الميداني
        private int GetFieldAttendanceTypeId()
        {
            // الحصول على معرف نوع الحضور الميداني من جدول AttendanceTypes
            return _context.AttendanceTypes
                .Where(at => at.Name == "ميداني" || at.Name == "Field")
                .Select(at => at.Id)
                .FirstOrDefault();
        }
        private int GetDailyAttendanceTypeId()
        {
            // الحصول على معرف نوع الحضور اليومي من جدول AttendanceTypes
            // يمكنك إما تخزين هذا المعرف في إعدادات التطبيق أو البحث عنه في قاعدة البيانات
            return _context.AttendanceTypes
                .Where(at => at.Name == "يومي" || at.Name == "Daily")
                .Select(at => at.Id)
                .FirstOrDefault();
        }
        public async Task<(bool success, List<int> changedStudentIds)> SaveAttendanceAsync(AttendanceViewModel model, DateTime attendanceDate, int UserId)
        {
            try
            {
                if (model?.StudentStatus == null || !model.StudentStatus.Any())
                    return (false, new List<int>());

                var dateOnly = attendanceDate.Date;
                var activeWorkingYear = await GetOrCreateActiveWorkingYearAsync();

                if (activeWorkingYear == null)
                    return (false, new List<int>());

                var dailyAttendanceTypeId = GetDailyAttendanceTypeId();
                var fieldAttendanceTypeId = GetFieldAttendanceTypeId();
                var changedStudentIds = new List<int>();

                foreach (var studentStatus in model.StudentStatus)
                {
                    var studentClassSectionYear = await _context.Student_Class_Section_Years
                        .Where(s => s.Student_Id == studentStatus.Students.Id &&
                                   s.Working_Year_Id == activeWorkingYear.Id &&
                                   s.Class_Id == model.Class.Id &&
                                   s.IsActive)
                        .FirstOrDefaultAsync();

                    if (studentClassSectionYear == null)
                        continue;

                    // البحث عن التسجيلات الحالية للحضور اليومي
                    var existingAttendance = await _context.StudentAttendances
                        .Where(a => a.StudentClassSectionYear_Student_Id == studentStatus.Students.Id &&
                                   a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                   a.Class_Id == model.Class.Id &&
                                   a.StudentClassSectionYear_Section_id == studentClassSectionYear.Section_id &&
                                   a.Date.Date == dateOnly &&
                                   a.AttendanceTypeId == dailyAttendanceTypeId &&
                                   !a.IsDeleted)
                        .FirstOrDefaultAsync();

                    var existingAbsence = await _context.StudentAbsents
                        .Where(a => a.StudentClassSectionYear_Student_Id == studentStatus.Students.Id &&
                                   a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                   a.Class_Id == model.Class.Id &&
                                   a.StudentClassSectionYear_Section_id == studentClassSectionYear.Section_id &&
                                   a.AttendanceTypeId == dailyAttendanceTypeId &&
                                   a.Date.Date == dateOnly &&
                                   !a.IsDeleted)
                        .FirstOrDefaultAsync();

                    // تحديد الحالة السابقة
                    bool? previousStatus = null;
                    if (existingAttendance != null)
                        previousStatus = true;
                    else if (existingAbsence != null)
                        previousStatus = false;

                    // تحديد الحالة الحالية
                    bool isCurrentlyPresent = studentStatus.Status;

                    // إذا تغيرت الحالة، أضف الطالب للقائمة
                    if (previousStatus.HasValue && previousStatus.Value != isCurrentlyPresent)
                    {
                        changedStudentIds.Add(studentStatus.Students.Id);

                        // حذف الحضور الميداني الحالي للطالب إذا كان موجود
                        await RemoveExistingFieldAttendanceAsync(studentStatus.Students.Id, model.Class.Id,
                            activeWorkingYear.Id, studentClassSectionYear.Section_id, dateOnly);
                    }

                    // إذا لم تتغير الحالة، تجاهل
                    if (previousStatus.HasValue && previousStatus.Value == isCurrentlyPresent)
                        continue;

                    // حذف التسجيلات السابقة للحضور اليومي
                    if (existingAttendance != null)
                        _context.StudentAttendances.Remove(existingAttendance);
                    if (existingAbsence != null)
                        _context.StudentAbsents.Remove(existingAbsence);

                    // إنشاء تسجيل جديد حسب الحالة
                    if (isCurrentlyPresent)
                    {
                        var attendance = new StudentAttendances
                        {
                            Date = dateOnly,
                            CreatedBy_Id = UserId,
                            AttendanceTypeId = dailyAttendanceTypeId,
                            StudentClassSectionYear_Student_Id = studentStatus.Students.Id,
                            Class_Id = model.Class.Id,
                            StudentClassSectionYear_Working_Year_Id = activeWorkingYear.Id,
                            StudentClassSectionYear_Section_id = studentClassSectionYear.Section_id,
                            IsDeleted = false
                        };
                        _context.StudentAttendances.Add(attendance);
                    }
                    else
                    {
                        var absence = new StudentAbsents
                        {
                            Date = dateOnly,
                            CreatedBy_Id = UserId,
                            AttendanceTypeId = dailyAttendanceTypeId,
                            StudentClassSectionYear_Student_Id = studentStatus.Students.Id,
                            Class_Id = model.Class.Id,
                            StudentClassSectionYear_Working_Year_Id = activeWorkingYear.Id,
                            StudentClassSectionYear_Section_id = studentClassSectionYear.Section_id,
                            IsDeleted = false
                        };

                        int? reasonId = studentStatus.Reason_Id ?? await GatReasonId();
                        if (reasonId != null)
                        {
                            absence.AbsenceReasonId = reasonId;
                        }

                        _context.StudentAbsents.Add(absence);
                    }
                }

                await _context.SaveChangesAsync();
                return (true, changedStudentIds);
            }
            catch (Exception ex)
            {
                return (false, new List<int>());
            }
        }
        public async Task<AbsenceReasons> GetReasonByname(string name)
        {
            var reason = await _context.AbsenceReasons.FirstOrDefaultAsync(r => r.Name ==name);
            return reason;
        }
        public async Task<bool> SaveAttendanceFieldAsync(AttendanceViewModel model, DateTime attendanceDate, int UserId)
        {
            try
            {
                // التأكد من أن النموذج صحيح
                if (model?.StudentStatus == null || !model.StudentStatus.Any())
                    return false;

                // الحصول على التاريخ باليوم فقط (بدون الوقت)
                var dateOnly = attendanceDate.Date;

                // الحصول على آخر سنة دراسية نشطة
                var activeWorkingYear = await GetOrCreateActiveWorkingYearAsync();

                if (activeWorkingYear == null)
                    return false;
                
                // معرف المستخدم الحالي (يجب تمريره من الكونترولر)
                var currentUserId = UserId; // تحتاج لتنفيذ هذه الدالة

                foreach (var studentStatus in model.StudentStatus)
                {
                
                    // البحث عن تسجيل الطالب في الصف والقسم للسنة النشطة
                    var studentClassSectionYear = await _context.Student_Class_Section_Years
                        .Where(s => s.Student_Id == studentStatus.Students.Id &&
                                   s.Working_Year_Id == activeWorkingYear.Id &&
                                   s.Class_Id == model.Class.Id &&
                                   s.IsActive)
                        .FirstOrDefaultAsync();

                    if (studentClassSectionYear == null)
                        continue; // الطالب غير مسجل في هذا الصف

                    // البحث عن تسجيل سابق في نفس اليوم في جدول الحضور
                    var existingAttendance = await _context.StudentAttendances
                        .Where(a => a.StudentClassSectionYear_Student_Id == studentStatus.Students.Id &&
                                   a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                   a.Class_Id == model.Class.Id &&
                                   a.StudentClassSectionYear_Section_id == studentClassSectionYear.Section_id &&
                                   a.Date.Date == dateOnly &&
                                   !a.IsDeleted&&a.AttendanceTypeId == GetFieldAttendanceTypeId())
                        .FirstOrDefaultAsync();

                    // البحث عن تسجيل سابق في نفس اليوم في جدول الغياب
                    var existingAbsence = await _context.StudentAbsents
                        .Where(a => a.StudentClassSectionYear_Student_Id == studentStatus.Students.Id &&
                                   a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                                   a.Class_Id == model.Class.Id &&
                                   a.StudentClassSectionYear_Section_id == studentClassSectionYear.Section_id &&
                                   a.Date.Date == dateOnly &&
                                   !a.IsDeleted && a.AttendanceTypeId == GetFieldAttendanceTypeId())
                        .FirstOrDefaultAsync();

                    // تحديد الحالة الحالية للطالب
                    bool isCurrentlyPresent = studentStatus.Status;

                    // التحقق من الحالة السابقة
                    bool? previousStatus = null;
                    if (existingAttendance != null)
                        previousStatus = true; // كان حاضر
                    else if (existingAbsence != null)
                        previousStatus = false; // كان غائب

                    // إذا لم تتغير الحالة، لا نفعل شيء
                    if (previousStatus.HasValue && previousStatus.Value == isCurrentlyPresent)
                        continue;

                    // حذف التسجيلات السابقة إذا كانت موجودة
                    if (existingAttendance != null)
                    {
                        _context.StudentAttendances.Remove(existingAttendance);
                    }
                    if (existingAbsence != null)
                    {
                        _context.StudentAbsents.Remove(existingAbsence);
                    }

                    // إنشاء تسجيل جديد حسب الحالة
                    if (isCurrentlyPresent)
                    {
                        // الطالب حاضر - تسجيل في جدول الحضور
                        var attendance = new StudentAttendances
                        {
                            Date = dateOnly,
                            CreatedBy_Id = currentUserId,
                            AttendanceTypeId = GetFieldAttendanceTypeId(), // معرف نوع الحضور اليومي
                            StudentClassSectionYear_Student_Id = studentStatus.Students.Id,
                            Class_Id = model.Class.Id,
                            StudentClassSectionYear_Working_Year_Id = activeWorkingYear.Id,
                            StudentClassSectionYear_Section_id = studentClassSectionYear.Section_id
                            
                            ,
                            IsDeleted = false
                        };
                        _context.StudentAttendances.Add(attendance);
                    }
                    else
                    {
                        var SelectedReason = await GetReasonByname(studentStatus.AbsenceReason);
                        // الطالب غائب - تسجيل في جدول الغياب
                        var absence = new StudentAbsents
                        {
                            Date = dateOnly,
                            CreatedBy_Id = currentUserId,
                            AttendanceTypeId = GetFieldAttendanceTypeId(), // معرف نوع الحضور اليومي
                            StudentClassSectionYear_Student_Id = studentStatus.Students.Id,
                            Class_Id = model.Class.Id,
                            StudentClassSectionYear_Working_Year_Id = activeWorkingYear.Id,
                            StudentClassSectionYear_Section_id = studentClassSectionYear.Section_id,
                            //AbsenceReasonId = studentStatus.Reason_Id!=null? studentStatus.Reason_Id: SelectedReason.Id,
                            CustomReasonDetails=studentStatus.CustomReason
                            ,
                            IsDeleted = false

                        };

                        // إضافة سبب الغياب إذا كان موجود
                        int? reasonId = studentStatus.Reason_Id;
                        // إضافة سبب الغياب إذا كان موجود
                        if (reasonId != null)
                        {
                            absence.AbsenceReasonId = reasonId;
                        }
                        else if (SelectedReason!=null)
                        {
                            absence.AbsenceReasonId = SelectedReason.Id;
                        }

                            _context.StudentAbsents.Add(absence);
                    }
                }

                // حفظ التغييرات
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ
                // _logger.LogError(ex, "Error saving attendance");
                return false;
            }
        }
        public async Task<List<SectionWithStudents>> GetStudentsGroupedBySectionAsync()
        {
            var working_year = await GetOrCreateActiveWorkingYearAsync();
            var juniorGrade = await GetJunior();

            if (juniorGrade == null || working_year == null)
                return new List<SectionWithStudents>();

            var result = await _context.Student_Class_Section_Years
                .Include(s => s.Student)
                .ThenInclude(student => student.StudentGrades)
                .Include(s => s.Section)
                .Where(s =>
                    s.Working_Year_Id == working_year.Id &&
                    s.Class_Id == null &&
                    s.Student.StudentGrades.Any(sg =>
                        sg.GradeId == juniorGrade.Id &&
                        sg.Working_Year_Id == working_year.Id)
                )
                .GroupBy(s => s.Section)
                .Select(g => new SectionWithStudents
                {
                    section = g.Key,
                    students = g.Select(s => s.Student).Distinct().ToList()
                })
                .ToListAsync();

            return result;
        }


        public async Task<List<ClassWithStudents>> GetStudentsGroupedByClassAsync(int GradeID)
        {
            var working_year = await GetInActiveWorkingYearAsync(); // السنة الدراسية غير النشطة
            var activeWorkingYear = await GetOrCreateActiveWorkingYearAsync(); // السنة الدراسية النشطة الحالية
            var grade = await _context.Grades.FindAsync(GradeID);

            if (grade == null || working_year == null || activeWorkingYear == null)
                return new List<ClassWithStudents>();

            var wheelerGrade = await GetWheeler();
            if (wheelerGrade == null)
                return new List<ClassWithStudents>();

            int grade_id;
            if (grade.Id == wheelerGrade.Id)
            {
                var juniorGrade = await GetJunior();
                if (juniorGrade == null)
                    return new List<ClassWithStudents>();
                grade_id = juniorGrade.Id;
            }
            else
            {
                grade_id = wheelerGrade.Id;
            }

            var result = await _context.Student_Class_Section_Years
                .Include(s => s.Student)
                .ThenInclude(student => student.StudentGrades)
                .Include(s => s.Class)
                .Where(s =>
                    s.Working_Year_Id == working_year.Id &&
                    s.Class_Id != null &&
                    s.Student.StudentGrades.Any(sg =>
                        sg.GradeId == grade_id &&
                        sg.Working_Year_Id == working_year.Id) &&
                    // استبعاد الطلاب المتسجلين في السنة الدراسية النشطة الحالية
                    !s.Student.StudentClassSectionYears.Any(activeRecord =>
                        activeRecord.Working_Year_Id == activeWorkingYear.Id &&
                        activeRecord.IsActive)
                )
                .GroupBy(s => s.Class)
                .Select(g => new ClassWithStudents
                {
                    classes = g.Key,
                    students = g.Select(s => s.Student).Distinct().ToList()
                })
                .ToListAsync();

            return result;
        }
        private async Task RemoveExistingFieldAttendanceAsync(int studentId, int classId, int workingYearId, int sectionId, DateTime dateOnly)
        {
            var fieldAttendanceTypeId = GetFieldAttendanceTypeId();

            // حذف الحضور الميداني
            var fieldAttendance = await _context.StudentAttendances
                .Where(a => a.StudentClassSectionYear_Student_Id == studentId &&
                           a.StudentClassSectionYear_Working_Year_Id == workingYearId &&
                           a.Class_Id == classId &&
                           a.StudentClassSectionYear_Section_id == sectionId &&
                           a.Date.Date == dateOnly &&
                           a.AttendanceTypeId == fieldAttendanceTypeId &&
                           !a.IsDeleted)
                .FirstOrDefaultAsync();

            if (fieldAttendance != null)
                _context.StudentAttendances.Remove(fieldAttendance);

            // حذف الغياب الميداني
            var fieldAbsence = await _context.StudentAbsents
                .Where(a => a.StudentClassSectionYear_Student_Id == studentId &&
                           a.StudentClassSectionYear_Working_Year_Id == workingYearId &&
                           a.Class_Id == classId &&
                           a.StudentClassSectionYear_Section_id == sectionId &&
                           a.Date.Date == dateOnly &&
                           a.AttendanceTypeId == fieldAttendanceTypeId &&
                           !a.IsDeleted)
                .FirstOrDefaultAsync();

            if (fieldAbsence != null)
                _context.StudentAbsents.Remove(fieldAbsence);
        }

        public async Task<List<StudentFieldChangeViewModel>> GetStudentsRequiringFieldUpdateAsync(int classId, List<int> changedStudentIds)
        {
            var activeWorkingYear = await GetOrCreateActiveWorkingYearAsync();
            if (activeWorkingYear == null || !changedStudentIds.Any())
                return new List<StudentFieldChangeViewModel>();

            var dateOnly = DateTime.Now.Date;
            var dailyAttendanceTypeId = GetDailyAttendanceTypeId();
            var fieldAttendanceTypeId = GetFieldAttendanceTypeId();
            var dailyAbsenceReasonId = await GatReasonId();

            var result = new List<StudentFieldChangeViewModel>();

            foreach (var studentId in changedStudentIds)
            {
                var student = await _context.Students.FindAsync(studentId);
                if (student == null) continue;

                var studentClassSection = await _context.Student_Class_Section_Years
                    .Where(s => s.Student_Id == studentId &&
                               s.Working_Year_Id == activeWorkingYear.Id &&
                               s.Class_Id == classId &&
                               s.IsActive)
                    .FirstOrDefaultAsync();

                if (studentClassSection == null) continue;

                // الحصول على الحالة اليومية الحالية
                var dailyAttendance = await _context.StudentAttendances
                    .Where(a => a.StudentClassSectionYear_Student_Id == studentId &&
                               a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                               a.Class_Id == classId &&
                               a.StudentClassSectionYear_Section_id == studentClassSection.Section_id &&
                               a.Date.Date == dateOnly &&
                               a.AttendanceTypeId == dailyAttendanceTypeId &&
                               !a.IsDeleted)
                    .FirstOrDefaultAsync();

                var dailyAbsence = await _context.StudentAbsents
                    .Where(a => a.StudentClassSectionYear_Student_Id == studentId &&
                               a.StudentClassSectionYear_Working_Year_Id == activeWorkingYear.Id &&
                               a.Class_Id == classId &&
                               a.StudentClassSectionYear_Section_id == studentClassSection.Section_id &&
                               a.Date.Date == dateOnly &&
                               a.AttendanceTypeId == dailyAttendanceTypeId &&
                               !a.IsDeleted)
                    .FirstOrDefaultAsync();

                bool currentDailyStatus = dailyAttendance != null;

                var studentChange = new StudentFieldChangeViewModel
                {
                    Student = student,
                    NewDailyStatus = currentDailyStatus,
                    PreviousDailyStatus = !currentDailyStatus, // العكس لأن الحالة تغيرت
                    CurrentFieldStatus = currentDailyStatus, // افتراضي مثل الحالة اليومية
                    IsFromAttendanceToAbsence = !currentDailyStatus, // من حضور لغياب
                    IsFromAbsenceToAttendance = currentDailyStatus   // من غياب لحضور
                };

                // إذا كان من حضور لغياب، اجعل السبب الافتراضي هو الغياب اليومي
                if (!currentDailyStatus)
                {
                    studentChange.CurrentFieldStatus = false;
                    studentChange.CurrentFieldReasonId = dailyAbsenceReasonId;
                    studentChange.CurrentFieldReason = "غياب يومي";
                }

                result.Add(studentChange);
            }

            return result;
        }

        public async Task<bool> ConfirmFieldAttendanceChangesAsync(int classId, List<int> studentIds, List<bool> newStatuses, int userId)
        {
            try
            {
                var activeWorkingYear = await GetOrCreateActiveWorkingYearAsync();
                if (activeWorkingYear == null) return false;

                var dateOnly = DateTime.Now.Date;
                var fieldAttendanceTypeId = GetFieldAttendanceTypeId();

                for (int i = 0; i < studentIds.Count; i++)
                {
                    var studentId = studentIds[i];
                    var newStatus = newStatuses[i];

                    var studentClassSection = await _context.Student_Class_Section_Years
                        .Where(s => s.Student_Id == studentId &&
                                   s.Working_Year_Id == activeWorkingYear.Id &&
                                   s.Class_Id == classId &&
                                   s.IsActive)
                        .FirstOrDefaultAsync();

                    if (studentClassSection == null) continue;

                    // إنشاء تسجيل ميداني جديد
                    if (newStatus)
                    {
                        var attendance = new StudentAttendances
                        {
                            Date = dateOnly,
                            CreatedBy_Id = userId,
                            AttendanceTypeId = fieldAttendanceTypeId,
                            StudentClassSectionYear_Student_Id = studentId,
                            Class_Id = classId,
                            StudentClassSectionYear_Working_Year_Id = activeWorkingYear.Id,
                            StudentClassSectionYear_Section_id = studentClassSection.Section_id,
                            IsDeleted = false
                        };
                        _context.StudentAttendances.Add(attendance);
                    }
                    else
                    {
                        var absence = new StudentAbsents
                        {
                            Date = dateOnly,
                            CreatedBy_Id = userId,
                            AttendanceTypeId = fieldAttendanceTypeId,
                            StudentClassSectionYear_Student_Id = studentId,
                            Class_Id = classId,
                            StudentClassSectionYear_Working_Year_Id = activeWorkingYear.Id,
                            StudentClassSectionYear_Section_id = studentClassSection.Section_id,
                            AbsenceReasonId = await GatReasonId(), // الغياب اليومي
                            IsDeleted = false
                        };
                        _context.StudentAbsents.Add(absence);
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //// ✅ طريقة أفضل - استخدام Cache للـ Grades
        //private Dictionary<string, Grades> _gradesCache = new Dictionary<string, Grades>();

        //public async Task<Grades> GetGradeByName(string gradeName)
        //{
        //    if (_gradesCache.ContainsKey(gradeName.ToLower()))
        //        return _gradesCache[gradeName.ToLower()];

        //    var grade = await _context.Grades.FirstOrDefaultAsync(s =>
        //        s.Name.ToLower() == gradeName.ToLower() && s.IsActive);

        //    if (grade != null)
        //        _gradesCache[gradeName.ToLower()] = grade;

        //    return grade;
        //}

        //// ✅ استخدام الطريقة المُحسنة
        //public async Task<List<ClassWithStudents>> GetStudentsGroupedByClassAsync_Improved(int GradeID)
        //{
        //    var working_year = await GetInActiveWorkingYearAsync(); // تأكد من الاسم الصحيح
        //    var grade = await _context.Grades.FindAsync(GradeID);

        //    if (grade == null || working_year == null)
        //        return new List<ClassWithStudents>();

        //    var wheelerGrade = await GetGradeByName("wheeler");
        //    if (wheelerGrade == null)
        //        return new List<ClassWithStudents>();

        //    int grade_id;
        //    if (grade.Id == wheelerGrade.Id)
        //    {
        //        var juniorGrade = await GetGradeByName("junior");
        //        if (juniorGrade == null)
        //            return new List<ClassWithStudents>();
        //        grade_id = juniorGrade.Id;
        //    }
        //    else
        //    {
        //        grade_id = wheelerGrade.Id;
        //    }
        //    var activeWorkingYear = await GetOrCreateActiveWorkingYearAsync();

        //    var result = await _context.Student_Class_Section_Years
        //        .Include(s => s.Student)
        //        .ThenInclude(student => student.StudentGrades)
        //        .Include(s => s.Class)
        //        .Where(s =>
        //            s.Working_Year_Id == working_year.Id &&
        //            s.Class_Id != null &&
        //            s.Student.StudentGrades.Any(sg =>
        //                sg.GradeId == grade_id &&
        //                sg.Working_Year_Id == working_year.Id)
        //            &&s.Student.StudentClassSectionYears.Any(sg => sg.Working_Year_Id != activeWorkingYear.Id && sg.Student_Id != )
        //        )
        //        .GroupBy(s => s.Class)
        //        .Select(g => new ClassWithStudents
        //        {
        //            classes = g.Key,
        //            students = g.Select(s => s.Student).Distinct().ToList()
        //        })
        //        .ToListAsync();

        //    return result;
        //}
        public async Task<Students> GetStudentByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.CreatedBy)
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
        public async Task<bool> AddStudentWithoutClassAsync(int studentId, int sectionId,int userid, int? workingYearId = null)
        {
            try
            {
                // جلب آخر سنة دراسية نشطة إذا لم يتم تحديدها
                if (!workingYearId.HasValue)
                {
                    var activeWorkingYear = await GetOrCreateActiveWorkingYearAsync();

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
                    CreatedBy_Id = userid, 
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
        public async Task<Working_Year> GetInActiveWorkingYearAsync()
        {
            // البحث عن سنة دراسية نشطة
            var activeWorkingYear = await _context.Working_Years
                .Where(wy => !wy.IsActive)
                .OrderByDescending(wy => wy.Start_date)
                .FirstOrDefaultAsync();


            return activeWorkingYear;
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
                    CreatedBy_Id = user.Id, // استخدم ID المستخدم الحالي
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

      
        // إضافة طالب مع كل التفاصيل - محدثة
        public async Task<bool> AddStudentWithAllDetailsAsync(int studentId, int sectionId, int classId, int createdById, int? workingYearId = null)
        {
            try
            {
                // جلب آخر سنة دراسية نشطة إذا لم يتم تحديدها
                if (!workingYearId.HasValue)
                {
                    var activeWorkingYear = await GetOrCreateActiveWorkingYearAsync();
                    if (activeWorkingYear == null)
                        return false;
                    workingYearId = activeWorkingYear.Id;
                }

                // التحقق من عدم وجود تسجيل مسبق للطالب في نفس السنة الدراسية
                var existingRecord = await _context.Student_Class_Section_Years
                    .FirstOrDefaultAsync(scss => scss.Student_Id == studentId &&
                                               scss.Working_Year_Id == workingYearId.Value &&
                                               scss.IsActive);

                if (existingRecord != null)
                {
                    // إذا كان موجود، قم بتحديث الفصل والقسم
                    existingRecord.Class_Id = classId;
                    existingRecord.Section_id = sectionId;
                    existingRecord.Date = DateTime.Now;
                    _context.Student_Class_Section_Years.Update(existingRecord);
                }
                else
                {
                    // إنشاء سجل جديد
                    var studentRecord = new Student_Class_Section_Year
                    {
                        Student_Id = studentId,
                        Working_Year_Id = workingYearId.Value,
                        Section_id = sectionId,
                        Class_Id = classId,
                        IsActive = true,
                        CreatedBy_Id = createdById,
                        Date = DateTime.Now
                    };
                    _context.Student_Class_Section_Years.Add(studentRecord);
                }

                // التحقق من سعة الفصل
                var classInfo = await _context.Classes.FindAsync(classId);
                if (classInfo != null && classInfo.MaxStudents.HasValue)
                {
                    var currentStudentsCount = await _context.Student_Class_Section_Years
                        .CountAsync(scss => scss.Class_Id == classId &&
                                          scss.Working_Year_Id == workingYearId.Value &&
                                          scss.IsActive);

                    if (currentStudentsCount > classInfo.MaxStudents.Value)
                    {
                        return false; // الفصل ممتلئ
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // يمكنك إضافة logging هنا
                Console.WriteLine($"Error in AddStudentWithAllDetailsAsync: {ex.Message}");
                return false;
            }
        }
        // جلب الطلاب مجمعين حسب القسم مع ترتيب أبجدي
        public async Task<List<SectionStudentsViewModel>> GetStudentsByDepartmentAsync(int? workingYearId = null)
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
                        return new List<SectionStudentsViewModel>();

                    workingYearId = activeWorkingYear.Id;
                }

                var result = await _context.Student_Class_Section_Years
                    .Where(scss => scss.Working_Year_Id == workingYearId && scss.IsActive)
                    .Include(scss => scss.Student)
                    .Include(scss => scss.Section)
                        .ThenInclude(s => s.Department)
                    .Include(scss => scss.Class)
                    .Include(scss => scss.WorkingYear)
                    .GroupBy(scss => new
                    {
                        DepartmentId = scss.Section.Department_Id,
                        DepartmentName = scss.Section.Department.Name,
                        SectionId = scss.Section_id,
                        SectionName = scss.Section.Name_Of_Section
                    })
                    .Select(g => new SectionStudentsViewModel
                    {
                        DepartmentId = g.Key.DepartmentId,
                        DepartmentName = g.Key.DepartmentName,
                        SectionId = g.Key.SectionId,
                        SectionName = g.Key.SectionName,
                        WorkingYearId = workingYearId.Value,
                        Students = g.Select(scss => new StudentInfoViewModel
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
                return new List<SectionStudentsViewModel>();
            }
        }

        // جلب الطلاب في قسم معين
        public async Task<List<StudentInfoViewModel>> GetStudentsInSectionAsync(int sectionId, int? workingYearId = null)
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
                        return new List<StudentInfoViewModel>();

                    workingYearId = activeWorkingYear.Id;
                }

                var students = await _context.Student_Class_Section_Years
                    .Where(scss => scss.Section_id == sectionId &&
                                  scss.Working_Year_Id == workingYearId &&
                                  scss.IsActive)
                    .Include(scss => scss.Student)
                    .Include(scss => scss.Class)
                    .Select(scss => new StudentInfoViewModel
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
                return new List<StudentInfoViewModel>();
            }
        }


        public async Task<IEnumerable<Classes>> GetClassesByGradeAsync(int gradeId)
        {
            return await _context.Classes
                .Where(c => c.GradeId == gradeId && c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<int> GetActiveStudentsCountAsync()
        {
            return await _context.Students.CountAsync(s => s.IsActive);
        }

    }
    // DTOs للإرجاع
    //public class SectionStudentsViewModel
    //{
    //    public int DepartmentId { get; set; }
    //    public string DepartmentName { get; set; }
    //    public int SectionId { get; set; }
    //    public string SectionName { get; set; }
    //    public int WorkingYearId { get; set; }
    //    public List<StudentInfoViewModel> Students { get; set; } = new List<StudentInfoViewModel>();
    //}

    //public class StudentInfoViewModel
    //{
    //    public int StudentId { get; set; }
    //    public string StudentName { get; set; }
    //    public string StudentCode { get; set; }
    //    public string ClassName { get; set; }
    //    public int? ClassId { get; set; }
    //    public string Phone { get; set; }
    //    public string Email { get; set; }
    //    public string Address { get; set; }
    //}
}
