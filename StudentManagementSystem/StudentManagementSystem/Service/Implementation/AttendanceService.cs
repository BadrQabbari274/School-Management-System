using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Student Attendance Operations

        public async Task<IEnumerable<StudentAttendance>> GetAllStudentAttendancesAsync()
        {
            return await _context.StudentAttendances
                .Include(sa => sa.Student)
                .Include(sa => sa.CreatedByUser)
                .Where(sa => !sa.IsDeleted)
                .OrderByDescending(sa => sa.Date)
                .ToListAsync();
        }

        public async Task<StudentAttendance> GetStudentAttendanceByIdAsync(int id)
        {
            return await _context.StudentAttendances
                .Include(sa => sa.Student)
                .Include(sa => sa.CreatedByUser)
                .FirstOrDefaultAsync(sa => sa.Id == id && !sa.IsDeleted);
        }

        public async Task<StudentAttendance> CreateStudentAttendanceAsync(StudentAttendance studentAttendance)
        {
            _context.StudentAttendances.Add(studentAttendance);
            await _context.SaveChangesAsync();
            return studentAttendance;
        }

        public async Task<StudentAttendance> UpdateStudentAttendanceAsync(StudentAttendance studentAttendance)
        {
            _context.StudentAttendances.Update(studentAttendance);
            await _context.SaveChangesAsync();
            return studentAttendance;
        }

        public async Task<bool> DeleteStudentAttendanceAsync(int id)
        {
            var studentAttendance = await _context.StudentAttendances.FindAsync(id);
            if (studentAttendance != null)
            {
                studentAttendance.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> SaveDailyAttendanceBulkAsync(int classId, DateTime date, List<int> presentStudentIds, int createdBy)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Get all active students in the class
                var allStudents = await _context.Students
                    .Where(s => s.ClassId == classId && s.IsActive)
                    .Select(s => s.Id)
                    .ToListAsync();

                // Remove existing attendance records for the day
                var existingRecords = await _context.StudentAttendances
                    .Where(sa => sa.Date == date.Date && allStudents.Contains(sa.StudentId.Value))
                    .ToListAsync();

                _context.StudentAttendances.RemoveRange(existingRecords);

                // Add new attendance records for present students
                foreach (var studentId in presentStudentIds.Where(id => allStudents.Contains(id)))
                {
                    var attendance = new StudentAttendance
                    {
                        StudentId = studentId,
                        Date = date,
                        State = "Present",
                        CreatedBy = createdBy
                    };
                    _context.StudentAttendances.Add(attendance);
                }

                // Get absent students (students not in present list)
                var absentStudentIds = allStudents.Except(presentStudentIds).ToList();

                // Remove existing absent records for the day (to avoid duplicates)
                var existingAbsentRecords = await _context.StudentAbsents
                    .Where(sa => sa.Date == date.Date &&
                               allStudents.Contains(sa.StudentId.Value) &&
                               !sa.IsFieldAttendance)
                    .ToListAsync();

                _context.StudentAbsents.RemoveRange(existingAbsentRecords);

                // Add absent records for absent students
                foreach (var studentId in absentStudentIds)
                {
                    var Student = _context.Students.Include(e => e.Class).ThenInclude(e => e.Field).ThenInclude(e => e.Grade).FirstOrDefault(f => f.Id == studentId);
                    var absentRecord = new StudentAbsent
                    {
                        StudentId = studentId,
                        StudentGrade =Student.Class.Field.Grade.Id,
                        Date = date,
                        AttendanceType = 0, // Normal absence (not field attendance)
                        IsFieldAttendance = false,
                        CreatedBy = createdBy,
                       TeacherId  = createdBy
                    };
                    _context.StudentAbsents.Add(absentRecord);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception if needed
                return false;
            }
        }
        public async Task<List<StudentAttendance>> GetDailyAttendanceByClassAndDateAsync(int classId, DateTime date)
        {
            var studentIds = await _context.Students
                .Where(s => s.ClassId == classId && s.IsActive)
                .Select(s => s.Id)
                .ToListAsync();

            return await _context.StudentAttendances
                .Include(sa => sa.Student)
                .Where(sa => sa.Date == date.Date &&
                           studentIds.Contains(sa.StudentId.Value) &&
                           !sa.IsDeleted)
                .ToListAsync();
        }

        #endregion

        #region Student Absent Operations

        public async Task<IEnumerable<StudentAbsent>> GetAllStudentAbsentsAsync()
        {
            return await _context.StudentAbsents
                .Include(sa => sa.Student)
                .Include(sa => sa.Teacher)
                .Include(sa => sa.AbsenceReason)
                .Include(sa => sa.CreatedByUser)
                .Where(sa => !sa.IsDeleted)
                .OrderByDescending(sa => sa.Date)
                .ToListAsync();
        }

        public async Task<StudentAbsent> GetStudentAbsentByIdAsync(int id)
        {
            return await _context.StudentAbsents
                .Include(sa => sa.Student)
                .Include(sa => sa.Teacher)
                .Include(sa => sa.AbsenceReason)
                .Include(sa => sa.CreatedByUser)
                .FirstOrDefaultAsync(sa => sa.Id == id && !sa.IsDeleted);
        }

        public async Task<StudentAbsent> CreateStudentAbsentAsync(StudentAbsent studentAbsent)
        {
            _context.StudentAbsents.Add(studentAbsent);
            await _context.SaveChangesAsync();
            return studentAbsent;
        }

        public async Task<StudentAbsent> UpdateStudentAbsentAsync(StudentAbsent studentAbsent)
        {
            _context.StudentAbsents.Update(studentAbsent);
            await _context.SaveChangesAsync();
            return studentAbsent;
        }

        public async Task<bool> DeleteStudentAbsentAsync(int id)
        {
            var studentAbsent = await _context.StudentAbsents.FindAsync(id);
            if (studentAbsent != null)
            {
                studentAbsent.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> SaveFieldAttendanceBulkAsync(int classId, DateTime date, List<FieldAttendanceRecord> fieldRecords, int createdBy)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Get all students in the class
                var allStudents = await _context.Students
                    .Where(s => s.ClassId == classId && s.IsActive)
                    .Select(s => s.Id)
                    .ToListAsync();

                // Remove existing field attendance records for the day
                var existingRecords = await _context.StudentAbsents
                    .Where(sa => sa.Date.Date == date.Date &&
                                allStudents.Contains(sa.StudentId.Value) &&
                                sa.IsFieldAttendance)
                    .ToListAsync();

                _context.StudentAbsents.RemoveRange(existingRecords);

                // Add new field attendance records
                foreach (var record in fieldRecords.Where(r => allStudents.Contains(r.StudentId) && r.IsAbsent))
                {
                    var absent = new StudentAbsent
                    {
                        StudentId = record.StudentId,
                        Date = date,
                        AbsenceReasonId = record.AbsenceReasonId,
                        CustomReasonDetails = record.CustomAbsenceReason,
                        AttendanceType = record.WithoutIncentive ? 1 : 0,
                        IsFieldAttendance = true,
                        CreatedBy = createdBy
                    };

                    _context.StudentAbsents.Add(absent);
                }

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

        public async Task<List<StudentAbsent>> GetFieldAttendanceByClassAndDateAsync(int classId, DateTime date)
        {
            var studentIds = await _context.Students
                .Where(s => s.ClassId == classId && s.IsActive)
                .Select(s => s.Id)
                .ToListAsync();

            return await _context.StudentAbsents
                .Include(sa => sa.Student)
                .Include(sa => sa.AbsenceReason)
                .Where(sa => sa.Date.Date == date.Date &&
                           studentIds.Contains(sa.StudentId.Value) &&
                           sa.IsFieldAttendance &&
                           !sa.IsDeleted)
                .ToListAsync();
        }

        #endregion

        #region Absence Reason Operations

        public async Task<IEnumerable<AbsenceReason>> GetAllAbsenceReasonsAsync()
        {
            return await _context.AbsenceReasons
                .Include(ar => ar.CreatedByUser)
                .Where(ar => !ar.IsDeleted)
                .OrderBy(ar => ar.Name)
                .ToListAsync();
        }

        public async Task<AbsenceReason> GetAbsenceReasonByIdAsync(int id)
        {
            return await _context.AbsenceReasons
                .Include(ar => ar.CreatedByUser)
                .FirstOrDefaultAsync(ar => ar.Id == id && !ar.IsDeleted);
        }

        public async Task<AbsenceReason> CreateAbsenceReasonAsync(AbsenceReason absenceReason)
        {
            _context.AbsenceReasons.Add(absenceReason);
            await _context.SaveChangesAsync();
            return absenceReason;
        }

        public async Task<AbsenceReason> UpdateAbsenceReasonAsync(AbsenceReason absenceReason)
        {
            _context.AbsenceReasons.Update(absenceReason);
            await _context.SaveChangesAsync();
            return absenceReason;
        }

        public async Task<bool> DeleteAbsenceReasonAsync(int id)
        {
            var absenceReason = await _context.AbsenceReasons.FindAsync(id);
            if (absenceReason != null)
            {
                absenceReason.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<AbsenceReason> GetOrCreateOtherReasonAsync(int createdBy)
        {
            var otherReason = await _context.AbsenceReasons
                .FirstOrDefaultAsync(ar => ar.Name == "Other" && !ar.IsDeleted);

            if (otherReason == null)
            {
                otherReason = new AbsenceReason
                {
                    Name = "Other",
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.Now
                };
                _context.AbsenceReasons.Add(otherReason);
                await _context.SaveChangesAsync();
            }

            return otherReason;
        }

        #endregion

        #region Attendance Type Operations

        public async Task<IEnumerable<AttendanceType>> GetAllAttendanceTypesAsync()
        {
            return await _context.AttendanceTypes
                .OrderBy(at => at.Name)
                .ToListAsync();
        }

        public async Task<AttendanceType> GetAttendanceTypeByIdAsync(int id)
        {
            return await _context.AttendanceTypes
                .FirstOrDefaultAsync(at => at.Id == id);
        }

        public async Task<AttendanceType> CreateAttendanceTypeAsync(AttendanceType attendanceType)
        {
            _context.AttendanceTypes.Add(attendanceType);
            await _context.SaveChangesAsync();
            return attendanceType;
        }

        public async Task<AttendanceType> UpdateAttendanceTypeAsync(AttendanceType attendanceType)
        {
            _context.AttendanceTypes.Update(attendanceType);
            await _context.SaveChangesAsync();
            return attendanceType;
        }

        public async Task<bool> DeleteAttendanceTypeAsync(int id)
        {
            var attendanceType = await _context.AttendanceTypes.FindAsync(id);
            if (attendanceType != null)
            {
                _context.AttendanceTypes.Remove(attendanceType);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        #endregion

        #region Exit Request Operations

        public async Task<RequestExit> CreateExitRequestAsync(RequestExit exitRequest)
        {
            exitRequest.Date = DateTime.Now;
            exitRequest.ExitTime = DateTime.Now.TimeOfDay;
            _context.RequestExits.Add(exitRequest);
            await _context.SaveChangesAsync();
            return exitRequest;
        }

        public async Task<IEnumerable<RequestExit>> GetExitRequestsByDateAsync(DateTime date)
        {
            return await _context.RequestExits
                .Include(re => re.Student)
                .Include(re => re.CreatedByUser)
                .Include(re => re.ProcessedByUser)
                .Include(re => re.Attendance)
                .Where(re => re.Date.Date == date.Date && !re.IsDeleted)
                .OrderBy(re => re.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<RequestExit>> GetPendingExitRequestsAsync()
        {
            return await _context.RequestExits
                .Include(re => re.Student)
                .Include(re => re.CreatedByUser)
                .Include(re => re.Attendance)
                .Where(re => re.Status == 0 && !re.IsDeleted)
                .OrderBy(re => re.Date)
                .ToListAsync();
        }

        public async Task<bool> ProcessExitRequestAsync(int exitRequestId, int status, int processedBy, string notes = null)
        {
            var exitRequest = await _context.RequestExits.FindAsync(exitRequestId);
            if (exitRequest != null)
            {
                exitRequest.Status = status;
                exitRequest.ProcessedBy = processedBy;
                exitRequest.ProcessedDate = DateTime.Now;
                exitRequest.ProcessingNotes = notes;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        #endregion

        #region Report Operations

        public async Task<ClassAttendanceReport> GetClassAttendanceReportAsync(int classId, DateTime date)
        {
            var classInfo = await _context.Classes.FindAsync(classId);
            if (classInfo == null) return null;

            var students = await _context.Students
                .Where(s => s.ClassId == classId && s.IsActive)
                .ToListAsync();

            var attendances = await _context.StudentAttendances
                .Include(sa => sa.Student)
                .Where(sa => sa.Date == date.Date &&
                           students.Select(s => s.Id).Contains(sa.StudentId.Value) &&
                           !sa.IsDeleted)
                .ToListAsync();

            var absences = await _context.StudentAbsents
                .Include(sa => sa.Student)
                .Include(sa => sa.AbsenceReason)
                .Where(sa => sa.Date.Date == date.Date &&
                           students.Select(s => s.Id).Contains(sa.StudentId.Value) &&
                           !sa.IsDeleted)
                .ToListAsync();

            var exitRequests = await _context.RequestExits
                .Include(re => re.Student)
                .Where(re => re.Date.Date == date.Date &&
                           students.Select(s => s.Id).Contains(re.StudentId.Value) &&
                           !re.IsDeleted)
                .ToListAsync();

            return new ClassAttendanceReport
            {
                ClassId = classId,
                ClassName = classInfo.Name,
                Date = date,
                TotalStudents = students.Count,
                PresentStudents = attendances.Count,
                AbsentStudents = students.Count - attendances.Count,
                FieldAbsentStudents = absences.Count(a => a.IsFieldAttendance),
                ExitRequestsCount = exitRequests.Count,
                AttendancePercentage = students.Count > 0 ? (double)attendances.Count / students.Count * 100 : 0,
                StudentDetails = students.Select(s => new StudentAttendanceDetail
                {
                    StudentId = s.Id,
                    StudentName = s.Name,
                    StudentCode = s.Code,
                    IsPresent = attendances.Any(a => a.StudentId == s.Id),
                    IsFieldAbsent = absences.Any(a => a.StudentId == s.Id && a.IsFieldAttendance),
                    AbsenceReason = absences.FirstOrDefault(a => a.StudentId == s.Id)?.AbsenceReason?.Name,
                    HasExitRequest = exitRequests.Any(er => er.StudentId == s.Id),
                    ExitRequestStatus = exitRequests.FirstOrDefault(er => er.StudentId == s.Id)?.Status
                }).ToList()
            };
        }

        public async Task<ClassAttendanceReport> GetClassAttendanceReportByDateRangeAsync(int classId, DateTime startDate, DateTime endDate)
        {
            var classInfo = await _context.Classes.FindAsync(classId);
            if (classInfo == null) return null;

            var students = await _context.Students
                .Where(s => s.ClassId == classId && s.IsActive)
                .ToListAsync();

            var attendances = await _context.StudentAttendances
                .Include(sa => sa.Student)
                .Where(sa => sa.Date >= startDate.Date &&
                           sa.Date <= endDate.Date &&
                           students.Select(s => s.Id).Contains(sa.StudentId.Value) &&
                           !sa.IsDeleted)
                .ToListAsync();

            var totalDays = (endDate.Date - startDate.Date).Days + 1;
            var totalPossibleAttendances = students.Count * totalDays;

            return new ClassAttendanceReport
            {
                ClassId = classId,
                ClassName = classInfo.Name,
                Date = startDate,
                EndDate = endDate,
                TotalStudents = students.Count,
                PresentStudents = attendances.Count,
                AbsentStudents = totalPossibleAttendances - attendances.Count,
                AttendancePercentage = totalPossibleAttendances > 0 ? (double)attendances.Count / totalPossibleAttendances * 100 : 0,
                StudentDetails = students.Select(s => new StudentAttendanceDetail
                {
                    StudentId = s.Id,
                    StudentName = s.Name,
                    StudentCode = s.Code,
                    PresentDaysCount = attendances.Count(a => a.StudentId == s.Id),
                    AbsentDaysCount = totalDays - attendances.Count(a => a.StudentId == s.Id),
                    AttendancePercentage = totalDays > 0 ? (double)attendances.Count(a => a.StudentId == s.Id) / totalDays * 100 : 0
                }).ToList()
            };
        }

        #endregion

        #region Utility Methods

        public async Task<bool> IsStudentPresentAsync(int studentId, DateTime date)
        {
            return await _context.StudentAttendances
                .AnyAsync(sa => sa.StudentId == studentId &&
                               sa.Date == date.Date &&
                               !sa.IsDeleted);
        }


        public async Task<StudentAttendanceSummary> GetStudentAttendanceSummaryAsync(int studentId, DateTime startDate, DateTime endDate)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null) return null;

            var attendances = await _context.StudentAttendances
                .Where(sa => sa.StudentId == studentId &&
                           sa.Date >= startDate.Date &&
                           sa.Date <= endDate.Date &&
                           !sa.IsDeleted)
                .ToListAsync();

            var absences = await _context.StudentAbsents
                .Include(sa => sa.AbsenceReason)
                .Where(sa => sa.StudentId == studentId &&
                           sa.Date.Date >= startDate.Date &&
                           sa.Date.Date <= endDate.Date &&
                           !sa.IsDeleted)
                .ToListAsync();

            var totalDays = (endDate.Date - startDate.Date).Days + 1;
            var presentDays = attendances.Count;
            var absentDays = totalDays - presentDays;
            var fieldAbsentDays = absences.Count(a => a.IsFieldAttendance);

            // Create attendance details for each day
            var attendanceDetails = new List<AttendanceDetail>();
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var dayAttendance = attendances.FirstOrDefault(a => a.Date == date);
                var dayAbsence = absences.FirstOrDefault(a => a.Date.Date == date);

                attendanceDetails.Add(new AttendanceDetail
                {
                    Date = date,
                    IsDailyPresent = dayAttendance != null,
                    IsFieldPresent = dayAttendance != null && dayAbsence == null,
                    AbsenceReason = dayAbsence?.AbsenceReason?.Name,
                    WithoutIncentive = dayAbsence?.AttendanceType == 1
                });
            }

            return new StudentAttendanceSummary
            {
                StudentId = studentId,
                StudentName = student.Name,
                StudentCode = student.Code,
                TotalDays = totalDays,
                PresentDays = presentDays,
                AbsentDays = absentDays,
                FieldAbsentDays = fieldAbsentDays,
                AttendancePercentage = totalDays > 0 ? (double)presentDays / totalDays * 100 : 0,
                AttendanceDetails = attendanceDetails
            };
        }

        #endregion
    }

    // Report Models
    public class ClassAttendanceReport
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public DateTime Date { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalStudents { get; set; }
        public int PresentStudents { get; set; }
        public int AbsentStudents { get; set; }
        public int FieldAbsentStudents { get; set; }
        public int ExitRequestsCount { get; set; }
        public double AttendancePercentage { get; set; }
        public List<StudentAttendanceDetail> StudentDetails { get; set; } = new List<StudentAttendanceDetail>();
    }

    public class StudentAttendanceDetail
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public bool IsPresent { get; set; }
        public bool IsFieldAbsent { get; set; }
        public string AbsenceReason { get; set; }
        public bool HasExitRequest { get; set; }
        public int? ExitRequestStatus { get; set; }
        public int PresentDaysCount { get; set; }
        public int AbsentDaysCount { get; set; }
        public double AttendancePercentage { get; set; }
    }
}