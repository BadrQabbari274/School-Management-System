using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AttendanceService : IAttendanceService
{
    private readonly ApplicationDbContext _context;
    private const int REGULAR_ATTENDANCE_TYPE_ID = 1; // الحضور العادي
    private const int FIELD_ATTENDANCE_TYPE_ID = 2;   // الحضور الميداني

    public AttendanceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ClassDto>> GetClassesByGradeAsync(int? gradeId)
    {
        return await _context.Classes
            .Where(c => c.GradeId == gradeId && c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => new ClassDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }
    #region Regular Attendance (الغياب العادي)

    public async Task<bool> AddRegularAbsenceAsync(AddRegularAbsenceViewModel dto)
    {
        try
        {
            // التحقق من وجود الطالب في النظام
            var studentExists = await _context.Student_Class_Section_Years
                .AnyAsync(s => s.Student_Id == dto.StudentId &&
                              s.Class_Id == dto.ClassId &&
                              s.Working_Year_Id == dto.WorkingYearId &&
                              s.Section_id == dto.SectionId &&
                              s.IsActive);

            if (!studentExists)
                return false;

            // التحقق من عدم وجود غياب مسجل لنفس الطالب في نفس اليوم
            var existingAbsence = await _context.StudentAbsents
                .AnyAsync(sa => sa.StudentClassSectionYear_Student_Id == dto.StudentId &&
                               sa.Class_Id == dto.ClassId &&
                               sa.StudentClassSectionYear_Working_Year_Id == dto.WorkingYearId &&
                               sa.StudentClassSectionYear_Section_id == dto.SectionId &&
                               sa.Date.Date == dto.Date.Date &&
                               !sa.IsDeleted);

            if (existingAbsence)
                return false;

            var absence = new StudentAbsents
            {
                StudentClassSectionYear_Student_Id = dto.StudentId,
                Class_Id = dto.ClassId,
                StudentClassSectionYear_Working_Year_Id = dto.WorkingYearId,
                StudentClassSectionYear_Section_id = dto.SectionId,
                Date = dto.Date,
                AbsenceReasonId = dto.AbsenceReasonId,
                AttendanceTypeId = REGULAR_ATTENDANCE_TYPE_ID,
                CustomReasonDetails = dto.CustomReasonDetails,
                CreatedBy_Id = dto.CreatedById,
                IsDeleted = false
            };

            _context.StudentAbsents.Add(absence);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> EditRegularAbsenceAsync(int id, EditRegularAbsenceViewModel dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // البحث عن السجل القديم وحذفه
            var existingAbsence = await _context.StudentAbsents
                .FirstOrDefaultAsync(sa => sa.Id == id && !sa.IsDeleted);

            if (existingAbsence == null)
            {
                await transaction.RollbackAsync();
                return false;
            }

            // حذف السجل القديم من قاعدة البيانات نهائياً
            _context.StudentAbsents.Remove(existingAbsence);

            // إضافة السجل الجديد بالبيانات المحدثة
            var newAbsence = new StudentAbsents
            {
                StudentClassSectionYear_Student_Id = existingAbsence.StudentClassSectionYear_Student_Id,
                Class_Id = existingAbsence.Class_Id,
                StudentClassSectionYear_Working_Year_Id = existingAbsence.StudentClassSectionYear_Working_Year_Id,
                StudentClassSectionYear_Section_id = existingAbsence.StudentClassSectionYear_Section_id,
                Date = existingAbsence.Date,
                AbsenceReasonId = dto.AbsenceReasonId,
                AttendanceTypeId = REGULAR_ATTENDANCE_TYPE_ID,
                CustomReasonDetails = dto.CustomReasonDetails,
                CreatedBy_Id = dto.CreatedById,
                IsDeleted = false
            };

            _context.StudentAbsents.Add(newAbsence);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    //public async Task<List<StudentAbsentsDto>> GetRegularAbsencesAsync(int classId, DateTime date)
    //{
    //    return await _context.StudentAbsents
    //        .Where(sa => sa.Class_Id == classId &&
    //                    sa.Date.Date == date.Date &&
    //                    sa.AttendanceTypeId == REGULAR_ATTENDANCE_TYPE_ID &&
    //                    !sa.IsDeleted)
    //        .Include(sa => sa.StudentClassSectionYear)
    //            .ThenInclude(scs => scs.Student)
    //        .Include(sa => sa.AbsenceReason)
    //        .Select(sa => new StudentAbsentsDto
    //        {
    //            Id = sa.Id,
    //            StudentId = sa.StudentClassSectionYear_Student_Id,
    //            StudentName = sa.StudentClassSectionYear.Student.Name,
    //            Date = sa.Date,
    //            AbsenceReasonId = sa.AbsenceReasonId,
    //            AbsenceReasonName = sa.AbsenceReason.Name,
    //            CustomReasonDetails = sa.CustomReasonDetails
    //        })
    //        .ToListAsync();
    //}

    #endregion

    #region Field Attendance (الغياب الميداني)

    public async Task<bool> AddFieldAbsenceAsync(AddFieldAbsenceViewModel dto)
    {
        try
        {
            // التحقق من وجود الطالب في النظام
            var studentExists = await _context.Student_Class_Section_Years
                .AnyAsync(s => s.Student_Id == dto.StudentId &&
                              s.Class_Id == dto.ClassId &&
                              s.Working_Year_Id == dto.WorkingYearId &&
                              s.Section_id == dto.SectionId &&
                              s.IsActive);

            if (!studentExists)
                return false;

            // التحقق من عدم وجود غياب عادي لنفس الطالب في نفس اليوم
            var hasRegularAbsence = await _context.StudentAbsents
                .AnyAsync(sa => sa.StudentClassSectionYear_Student_Id == dto.StudentId &&
                               sa.Class_Id == dto.ClassId &&
                               sa.StudentClassSectionYear_Working_Year_Id == dto.WorkingYearId &&
                               sa.StudentClassSectionYear_Section_id == dto.SectionId &&
                               sa.Date.Date == dto.Date.Date &&
                               sa.AttendanceTypeId == REGULAR_ATTENDANCE_TYPE_ID &&
                               !sa.IsDeleted);

            if (hasRegularAbsence)
                return false; // لا يمكن إضافة غياب ميداني إذا كان غائب في الغياب العادي

            // التحقق من عدم وجود غياب ميداني مسجل لنفس الطالب في نفس اليوم
            var existingFieldAbsence = await _context.StudentAbsents
                .AnyAsync(sa => sa.StudentClassSectionYear_Student_Id == dto.StudentId &&
                               sa.Class_Id == dto.ClassId &&
                               sa.StudentClassSectionYear_Working_Year_Id == dto.WorkingYearId &&
                               sa.StudentClassSectionYear_Section_id == dto.SectionId &&
                               sa.Date.Date == dto.Date.Date &&
                               sa.AttendanceTypeId == FIELD_ATTENDANCE_TYPE_ID &&
                               !sa.IsDeleted);

            if (existingFieldAbsence)
                return false;

            var absence = new StudentAbsents
            {
                StudentClassSectionYear_Student_Id = dto.StudentId,
                Class_Id = dto.ClassId,
                StudentClassSectionYear_Working_Year_Id = dto.WorkingYearId,
                StudentClassSectionYear_Section_id = dto.SectionId,
                Date = dto.Date,
                AbsenceReasonId = dto.AbsenceReasonId,
                AttendanceTypeId = FIELD_ATTENDANCE_TYPE_ID,
                CustomReasonDetails = dto.Reason, // استخدام الـ Reason بدلاً من CustomReasonDetails
                CreatedBy_Id = dto.CreatedById,
                IsDeleted = false
            };

            _context.StudentAbsents.Add(absence);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> EditFieldAbsenceAsync(int id, EditFieldAbsenceViewModel dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // البحث عن السجل القديم وحذفه
            var existingAbsence = await _context.StudentAbsents
                .FirstOrDefaultAsync(sa => sa.Id == id &&
                                          sa.AttendanceTypeId == FIELD_ATTENDANCE_TYPE_ID &&
                                          !sa.IsDeleted);

            if (existingAbsence == null)
            {
                await transaction.RollbackAsync();
                return false;
            }

            // حذف السجل القديم من قاعدة البيانات نهائياً
            _context.StudentAbsents.Remove(existingAbsence);

            // إضافة السجل الجديد بالبيانات المحدثة
            var newAbsence = new StudentAbsents
            {
                StudentClassSectionYear_Student_Id = existingAbsence.StudentClassSectionYear_Student_Id,
                Class_Id = existingAbsence.Class_Id,
                StudentClassSectionYear_Working_Year_Id = existingAbsence.StudentClassSectionYear_Working_Year_Id,
                StudentClassSectionYear_Section_id = existingAbsence.StudentClassSectionYear_Section_id,
                Date = existingAbsence.Date,
                AbsenceReasonId = dto.AbsenceReasonId,
                AttendanceTypeId = FIELD_ATTENDANCE_TYPE_ID,
                CustomReasonDetails = dto.Reason,
                CreatedBy_Id = dto.CreatedById,
                IsDeleted = false
            };

            _context.StudentAbsents.Add(newAbsence);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    //public async Task<List<StudentAbsentsDto>> GetFieldAbsencesAsync(int classId, DateTime date)
    //{
    //    return await _context.StudentAbsents
    //        .Where(sa => sa.Class_Id == classId &&
    //                    sa.Date.Date == date.Date &&
    //                    sa.AttendanceTypeId == FIELD_ATTENDANCE_TYPE_ID &&
    //                    !sa.IsDeleted)
    //        .Include(sa => sa.StudentClassSectionYear)
    //            .ThenInclude(scs => scs.Student)
    //        .Include(sa => sa.AbsenceReason)
    //        .Select(sa => new StudentAbsentsDto
    //        {
    //            Id = sa.Id,
    //            StudentId = sa.StudentClassSectionYear_Student_Id,
    //            StudentName = sa.StudentClassSectionYear.Student.Name,
    //            Date = sa.Date,
    //            AbsenceReasonId = sa.AbsenceReasonId,
    //            AbsenceReasonName = sa.AbsenceReason.Name,
    //            CustomReasonDetails = sa.CustomReasonDetails
    //        })
    //        .ToListAsync();
    //}

    #endregion
    public async Task<ClassAttendanceReportDto> GetClassAttendanceReportAsync(int classId, DateTime startDate, DateTime endDate, int? attendanceTypeId = null)
    {
        var studentsInClass = await _context.Student_Class_Section_Years
            .Where(sc => sc.Class_Id == classId && sc.IsActive)
            .Include(sc => sc.Student)
            .Include(sc => sc.Class)
            .Select(sc => new
            {
                StudentId = sc.Student_Id,
                StudentName = sc.Student.Name,
                StudentCode = sc.Student.Code,
                ClassName = sc.Class.Name,
                Student_Class_Section_Year = sc
            })
            .ToListAsync();

        var students = new List<StudentAttendanceDto>();

        foreach (var studentInClass in studentsInClass)
        {
            // جلب الحضور
            var attendancesQuery = _context.StudentAttendances
                .Where(a => a.StudentClassSectionYear_Student_Id == studentInClass.StudentId &&
                           a.Class_Id == classId &&
                           a.Date >= startDate && a.Date <= endDate &&
                           !a.IsDeleted);

            // تطبيق فلتر نوع الحضور/الغياب إذا تم تحديده
            if (attendanceTypeId.HasValue)
            {
                attendancesQuery = attendancesQuery.Where(a => a.AttendanceTypeId == attendanceTypeId.Value);
            }

            var attendances = await attendancesQuery
                .Include(a => a.AttendanceType)
                .ToListAsync();

            // جلب الغياب
            var absencesQuery = _context.StudentAbsents
                .Where(a => a.StudentClassSectionYear_Student_Id == studentInClass.StudentId &&
                           a.Class_Id == classId &&
                           a.Date >= startDate && a.Date <= endDate &&
                           !a.IsDeleted);

            // تطبيق فلتر نوع الحضور/الغياب إذا تم تحديده
            if (attendanceTypeId.HasValue)
            {
                absencesQuery = absencesQuery.Where(a => a.AttendanceTypeId == attendanceTypeId.Value);
            }

            var absences = await absencesQuery
                .Include(a => a.AbsenceReason)
                .Include(a => a.AttendanceType)
                .ToListAsync();

            // حساب أيام الحضور والغياب
            var presentDays = attendances.Count();
            var absentDays = absences.Count();
            var totalDays = presentDays + absentDays;
            var attendancePercentage = totalDays > 0 ? (double)presentDays / totalDays * 100 : 0;

            // جمع أسباب الغياب
            var absenceReasons = absences
                .Select(a => a.AbsenceReason?.Name ?? a.CustomReasonDetails ?? "أخرى")
                .Where(r => !string.IsNullOrEmpty(r))
                .Distinct()
                .ToList();

            // جمع أنواع الغياب
            var absenceTypes = absences
                .Select(a => a.AttendanceType?.Name)
                .Where(t => !string.IsNullOrEmpty(t))
                .Distinct()
                .ToList();

            students.Add(new StudentAttendanceDto
            {
                StudentId = studentInClass.StudentId,
                StudentName = studentInClass.StudentName,
                StudentCode = studentInClass.StudentCode ?? "غير محدد",
                ClassName = studentInClass.ClassName,
                PresentDays = presentDays,
                AbsentDays = absentDays,
                AttendancePercentage = attendancePercentage,
                AbsenceReasons = absenceReasons,
                AbsenceTypes = absenceTypes
            });
        }

        // حساب الإحصائيات العامة
        var totalStudents = students.Count;
        var totalPresentDays = students.Sum(s => s.PresentDays);
        var totalAbsentDays = students.Sum(s => s.AbsentDays);
        var averageAttendance = totalStudents > 0 ? students.Average(s => s.AttendancePercentage) : 0;

        return new ClassAttendanceReportDto
        {
            ClassId = classId,
            StartDate = startDate,
            EndDate = endDate,
            Students = students,
            TotalStudents = totalStudents,
            TotalPresentDays = totalPresentDays,
            TotalAbsentDays = totalAbsentDays,
            AverageAttendancePercentage = averageAttendance
        };
    }

    public async Task<TodayAttendanceDto> GetTodayClassAttendanceAsync(int classId)
    {
        var today = DateTime.Today;

        var studentsInClass = await _context.Student_Class_Section_Years
            .Where(sc => sc.Class_Id == classId && sc.IsActive)
            .Include(sc => sc.Student)
            .Select(sc => new
            {
                StudentId = sc.Student_Id,
                StudentName = sc.Student.Name,
                StudentCode = sc.Student.Code
            })
            .ToListAsync();

        // جلب الحضور لليوم
        var todayAttendances = await _context.StudentAttendances
            .Where(a => a.Class_Id == classId &&
                       a.Date.Date == today &&
                       !a.IsDeleted)
            .Include(a => a.AttendanceType)
            .ToListAsync();

        // جلب الغياب لليوم
        var todayAbsences = await _context.StudentAbsents
            .Where(a => a.Class_Id == classId &&
                       a.Date.Date == today &&
                       !a.IsDeleted)
            .Include(a => a.AbsenceReason)
            .Include(a => a.AttendanceType)
            .ToListAsync();

        var presentStudents = new List<TodayStudentAttendanceDto>();
        var absentStudents = new List<TodayStudentAttendanceDto>();

        foreach (var student in studentsInClass)
        {
            var attendance = todayAttendances.FirstOrDefault(a => a.StudentClassSectionYear_Student_Id == student.StudentId);
            var absence = todayAbsences.FirstOrDefault(a => a.StudentClassSectionYear_Student_Id == student.StudentId);

            if (attendance != null)
            {
                presentStudents.Add(new TodayStudentAttendanceDto
                {
                    StudentId = student.StudentId,
                    StudentName = student.StudentName,
                    StudentCode = student.StudentCode ?? "غير محدد",
                    Status = "حاضر",
                    Reason = "",
                    AttendanceType = attendance.AttendanceType?.Name ?? ""
                });
            }
            else if (absence != null)
            {
                absentStudents.Add(new TodayStudentAttendanceDto
                {
                    StudentId = student.StudentId,
                    StudentName = student.StudentName,
                    StudentCode = student.StudentCode ?? "غير محدد",
                    Status = "غائب",
                    Reason = absence.AbsenceReason?.Name ?? absence.CustomReasonDetails ?? "أخرى",
                    AttendanceType = absence.AttendanceType?.Name ?? ""
                });
            }
        }

        return new TodayAttendanceDto
        {
            Date = today,
            ClassId = classId,
            TotalStudents = studentsInClass.Count,
            PresentCount = presentStudents.Count,
            AbsentCount = absentStudents.Count,
            PresentStudents = presentStudents,
            AbsentStudents = absentStudents
        };
    }

    public async Task<StudentAttendanceDetailsDto> GetStudentAttendanceDetailsAsync(int studentId, int classId, DateTime startDate, DateTime endDate, int? attendanceTypeId = null)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
            throw new Exception("الطالب غير موجود");

        // جلب الحضور
        var attendancesQuery = _context.StudentAttendances
            .Where(a => a.StudentClassSectionYear_Student_Id == studentId &&
                       a.Class_Id == classId &&
                       a.Date >= startDate && a.Date <= endDate &&
                       !a.IsDeleted);

        if (attendanceTypeId.HasValue)
        {
            attendancesQuery = attendancesQuery.Where(a => a.AttendanceTypeId == attendanceTypeId.Value);
        }

        var attendances = await attendancesQuery
            .Include(a => a.AttendanceType)
            .OrderByDescending(a => a.Date)
            .ToListAsync();

        // جلب الغياب
        var absencesQuery = _context.StudentAbsents
            .Where(a => a.StudentClassSectionYear_Student_Id == studentId &&
                       a.Class_Id == classId &&
                       a.Date >= startDate && a.Date <= endDate &&
                       !a.IsDeleted);

        if (attendanceTypeId.HasValue)
        {
            absencesQuery = absencesQuery.Where(a => a.AttendanceTypeId == attendanceTypeId.Value);
        }

        var absences = await absencesQuery
            .Include(a => a.AbsenceReason)
            .Include(a => a.AttendanceType)
            .OrderByDescending(a => a.Date)
            .ToListAsync();

        var presentDays = attendances.Count();
        var absentDays = absences.Count();
        var totalDays = presentDays + absentDays;
        var attendancePercentage = totalDays > 0 ? (double)presentDays / totalDays * 100 : 0;

        // تجميع البيانات حسب التاريخ
        var attendanceHistory = new List<StudentAttendanceHistoryDto>();

        foreach (var attendance in attendances)
        {
            attendanceHistory.Add(new StudentAttendanceHistoryDto
            {
                Date = attendance.Date,
                Status = "حاضر",
                Reason = "",
                AttendanceType = attendance.AttendanceType?.Name ?? ""
            });
        }

        foreach (var absence in absences)
        {
            attendanceHistory.Add(new StudentAttendanceHistoryDto
            {
                Date = absence.Date,
                Status = "غائب",
                Reason = absence.AbsenceReason?.Name ?? absence.CustomReasonDetails ?? "أخرى",
                AttendanceType = absence.AttendanceType?.Name ?? ""
            });
        }

        attendanceHistory = attendanceHistory.OrderByDescending(h => h.Date).ToList();

        // إحصائيات الغياب حسب السبب
        var absenceReasonStats = absences
            .GroupBy(a => a.AbsenceReason?.Name ?? a.CustomReasonDetails ?? "أخرى")
            .Select(g => new AbsenceReasonStatDto
            {
                ReasonName = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(r => r.Count)
            .ToList();

        // إحصائيات أنواع الغياب
        var absenceTypeStats = absences
            .GroupBy(a => a.AttendanceType?.Name ?? "غير محدد")
            .Select(g => new AbsenceTypeStatDto
            {
                TypeName = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(r => r.Count)
            .ToList();

        return new StudentAttendanceDetailsDto
        {
            StudentId = studentId,
            StudentName = student.Name,
            StudentCode = student.Code ?? "غير محدد",
            StartDate = startDate,
            EndDate = endDate,
            PresentDays = presentDays,
            AbsentDays = absentDays,
            AttendancePercentage = attendancePercentage,
            AttendanceHistory = attendanceHistory,
            AbsenceReasonStats = absenceReasonStats,
            AbsenceTypeStats = absenceTypeStats
        };
    }
    public async Task<List<AttendanceTypeDto>> GetAllAttendanceTypesAsync()
    {
        return await _context.AttendanceTypes
            .Select(at => new AttendanceTypeDto
            {
                Id = at.Id,
                Name = at.Name,
                Description = at.Description
            })
            .OrderBy(at => at.Name)
            .ToListAsync();
    }
    #region Request Exit

    public async Task<bool> RequestExitAsync(RequestExitViewModel dto)
    {
        try
        {
            var requestExit = new RequestExits
            {
                AttendanceId = dto.AttendanceId,
                Reason = dto.Reason,
                Date = dto.Date,
                ExitTime = dto.ExitTime,
                CreatedBy_Id = dto.CreatedById,
                IsDeleted = false
            };

            _context.RequestExits.Add(requestExit);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<RequestExitViewModel>> GetRequestExitsAsync(int attendanceId)
    {
        return await _context.RequestExits
            .Where(re => re.AttendanceId == attendanceId && !re.IsDeleted)
            .Include(re => re.CreatedBy)
            .Select(re => new RequestExitViewModel
            {
                Id = re.Id,
                AttendanceId = re.AttendanceId,
                Reason = re.Reason,
                Date = re.Date,
                ExitTime = re.ExitTime,
                CreatedById = re.CreatedBy_Id,
                CreatedByName = re.CreatedBy.Name
            })
            .ToListAsync();
    }

    #endregion

    #region Helper Methods

    public async Task<List<StudentForAttendanceViewModel>> GetStudentsForRegularAttendanceAsync(int classId, int workingYearId, int sectionId, DateTime date)
    {
        var students = await _context.Student_Class_Section_Years
            .Where(scs => scs.Class_Id == classId &&
                         scs.Working_Year_Id == workingYearId &&
                         scs.Section_id == sectionId &&
                         scs.IsActive)
            .Include(scs => scs.Student)
            .Select(scs => new StudentForAttendanceViewModel
            {
                StudentId = scs.Student_Id,
                StudentName = scs.Student.Name,
                IsAbsent = _context.StudentAbsents.Any(sa =>
                    sa.StudentClassSectionYear_Student_Id == scs.Student_Id &&
                    sa.Class_Id == classId &&
                    sa.Date.Date == date.Date &&
                    sa.AttendanceTypeId == REGULAR_ATTENDANCE_TYPE_ID &&
                    !sa.IsDeleted)
            })
            .ToListAsync();

        return students;
    }

    public async Task<List<StudentForAttendanceViewModel>> GetStudentsForFieldAttendanceAsync(int classId, int workingYearId, int sectionId, DateTime date)
    {
        // الطلاب المتاحين للغياب الميداني (ليسوا غائبين في الغياب العادي)
        var students = await _context.Student_Class_Section_Years
            .Where(scs => scs.Class_Id == classId &&
                         scs.Working_Year_Id == workingYearId &&
                         scs.Section_id == sectionId &&
                         scs.IsActive &&
                         !_context.StudentAbsents.Any(sa =>
                             sa.StudentClassSectionYear_Student_Id == scs.Student_Id &&
                             sa.Class_Id == classId &&
                             sa.Date.Date == date.Date &&
                             sa.AttendanceTypeId == REGULAR_ATTENDANCE_TYPE_ID &&
                             !sa.IsDeleted))
            .Include(scs => scs.Student)
            .Select(scs => new StudentForAttendanceViewModel
            {
                StudentId = scs.Student_Id,
                StudentName = scs.Student.Name,
                IsAbsent = _context.StudentAbsents.Any(sa =>
                    sa.StudentClassSectionYear_Student_Id == scs.Student_Id &&
                    sa.Class_Id == classId &&
                    sa.Date.Date == date.Date &&
                    sa.AttendanceTypeId == FIELD_ATTENDANCE_TYPE_ID &&
                    !sa.IsDeleted)
            })
            .ToListAsync();

        return students;
    }

    #endregion
}

#region DTOs

public class AddRegularAbsenceViewModel
{
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public int WorkingYearId { get; set; }
    public int SectionId { get; set; }
    public DateTime Date { get; set; }
    public int AbsenceReasonId { get; set; }
    public string CustomReasonDetails { get; set; }
    public int CreatedById { get; set; }
}

public class EditRegularAbsenceViewModel
{
    public int AbsenceReasonId { get; set; }
    public string CustomReasonDetails { get; set; }
    public int CreatedById { get; set; }
}

public class AddFieldAbsenceViewModel
{
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public int WorkingYearId { get; set; }
    public int SectionId { get; set; }
    public DateTime Date { get; set; }
    public int AbsenceReasonId { get; set; }
    public string Reason { get; set; } // للغياب الميداني نستخدم Reason
    public int CreatedById { get; set; }
}

public class EditFieldAbsenceViewModel
{
    public int AbsenceReasonId { get; set; }
    public string Reason { get; set; }
    public int CreatedById { get; set; }
}

public class RequestExitViewModel 
{
    public int? Id { get; set; }
    public int AttendanceId { get; set; }
    public string Reason { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan ExitTime { get; set; }
    public int CreatedById { get; set; }
    public string CreatedByName { get; set; }
}

public class StudentAbsentsViewModel
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public DateTime Date { get; set; }
    public int AbsenceReasonId { get; set; }
    public string AbsenceReasonName { get; set; }
    public string CustomReasonDetails { get; set; }
}

public class StudentForAttendanceViewModel
{
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public bool IsAbsent { get; set; }
}

#endregion