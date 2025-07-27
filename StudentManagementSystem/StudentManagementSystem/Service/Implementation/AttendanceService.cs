//using Microsoft.EntityFrameworkCore;
//using StudentManagementSystem.Data;
//using StudentManagementSystem.Models;

//namespace StudentManagementSystem.Services
//{

//    public class AttendanceService : IAttendanceService
//    {
//        private readonly ApplicationDbContext _context;

//        public AttendanceService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<List<Student>> GetStudentsForAttendanceAsync(int classId)
//        {
//            return await _context.Students
//                .Where(s => s.ClassId == classId && s.IsActive)
//                .Include(s => s.Class)
//                .OrderBy(s => s.Name)
//                .ToListAsync();
//        }

//        public async Task<List<StudentAttendance>> GetTodayRegularAttendanceAsync(int classId)
//        {
//            var today = DateTime.Today;
//            return await _context.StudentAttendances
//                .Where(sa => sa.Date.Date == today &&
//                           sa.Student.ClassId == classId &&
//                           !sa.IsFieldAttendance &&
//                           !sa.IsDeleted)
//                .Include(sa => sa.Student)
//                .Include(sa => sa.CreatedByUser)
//                .ToListAsync();
//        }

//        public async Task<List<StudentAttendance>> GetTodayFieldAttendanceAsync(int classId)
//        {
//            var today = DateTime.Today;
//            return await _context.StudentAttendances
//                .Where(sa => sa.Date.Date == today &&
//                           sa.Student.ClassId == classId &&
//                           sa.IsFieldAttendance &&
//                           !sa.IsDeleted)
//                .Include(sa => sa.Student)
//                .Include(sa => sa.CreatedByUser)
//                .ToListAsync();
//        }

//        public async Task<List<StudentAbsent>> GetTodayRegularAbsentAsync(int classId)
//        {
//            var today = DateTime.Today;
//            return await _context.StudentAbsents
//                .Where(sa => sa.Date.Date == today &&
//                           sa.Student.ClassId == classId &&
//                           !sa.IsFieldAttendance &&
//                           !sa.IsDeleted)
//                .Include(sa => sa.Student)
//                .Include(sa => sa.AbsenceReason)
//                .Include(sa => sa.CreatedByUser)
//                .ToListAsync();
//        }

//        public async Task<List<StudentAbsent>> GetTodayFieldAbsentAsync(int classId)
//        {
//            var today = DateTime.Today;
//            return await _context.StudentAbsents
//                .Where(sa => sa.Date.Date == today &&
//                           sa.Student.ClassId == classId &&
//                           sa.IsFieldAttendance &&
//                           !sa.IsDeleted)
//                .Include(sa => sa.Student)
//                .Include(sa => sa.AbsenceReason)
//                .Include(sa => sa.CreatedByUser)
//                .ToListAsync();
//        }

//        public async Task<List<AbsenceReasons>> GetAbsenceReasonsAsync()
//        {
//            return await _context.AbsenceReasons
//                .Where(ar => !ar.IsDeleted)
//                .OrderBy(ar => ar.Name)
//                .ToListAsync();
//        }

//        public async Task<bool> TakeRegularAttendanceAsync(List<RegularAttendanceDto> attendanceList, int createdBy)
//        {
//            try
//            {
//                var today = DateTime.Today;

//                foreach (var item in attendanceList)
//                {
//                    // Check if student already has regular attendance today
//                    var existingAttendance = await _context.StudentAttendances
//                        .FirstOrDefaultAsync(sa => sa.StudentId == item.StudentId &&
//                                           sa.Date.Date == today &&
//                                           !sa.IsFieldAttendance &&
//                                           !sa.IsDeleted);

//                    var existingAbsent = await _context.StudentAbsents
//                        .FirstOrDefaultAsync(sa => sa.StudentId == item.StudentId &&
//                                           sa.Date.Date == today &&
//                                           !sa.IsFieldAttendance &&
//                                           !sa.IsDeleted);

//                    if (existingAttendance != null || existingAbsent != null)
//                        continue; // Skip if already recorded

//                    if (item.IsPresent)
//                    {
//                        var attendance = new StudentAttendance
//                        {
//                            StudentId = item.StudentId,
//                            StudentGrade = item.StudentGrade,
//                            Date = today,
//                            CreatedBy = createdBy,
//                            IsFieldAttendance = false,
//                            Notes = item.Notes
//                        };
//                        _context.StudentAttendances.Add(attendance);
//                    }
//                    else
//                    {
//                        var absent = new StudentAbsent
//                        {
//                            StudentId = item.StudentId,
//                            StudentGrade = item.StudentGrade,
//                            Date = today,
//                            CreatedBy = createdBy,
//                            IsFieldAttendance = false,
//                            AbsenceReasonId = item.AbsenceReasonId,
//                            CustomReasonDetails = item.CustomReasonDetails,
//                            Notes = item.Notes
//                        };
//                        _context.StudentAbsents.Add(absent);
//                    }
//                }

//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public async Task<bool> TakeFieldAttendanceAsync(List<FieldAttendanceDto> attendanceList, int createdBy)
//        {
//            try
//            {
//                var today = DateTime.Today;

//                foreach (var item in attendanceList)
//                {
//                    // Check if student is present in regular attendance today
//                    var hasRegularAttendance = await _context.StudentAttendances
//                        .AnyAsync(sa => sa.StudentId == item.StudentId &&
//                                  sa.Date.Date == today &&
//                                  !sa.IsFieldAttendance &&
//                                  !sa.IsDeleted);

//                    if (!hasRegularAttendance)
//                        continue; // Only students with regular attendance can have field attendance

//                    // Check if student already has field attendance today
//                    var existingFieldAttendance = await _context.StudentAttendances
//                        .FirstOrDefaultAsync(sa => sa.StudentId == item.StudentId &&
//                                           sa.Date.Date == today &&
//                                           sa.IsFieldAttendance &&
//                                           !sa.IsDeleted);

//                    var existingFieldAbsent = await _context.StudentAbsents
//                        .FirstOrDefaultAsync(sa => sa.StudentId == item.StudentId &&
//                                           sa.Date.Date == today &&
//                                           sa.IsFieldAttendance &&
//                                           !sa.IsDeleted);

//                    // Remove existing field records
//                    if (existingFieldAttendance != null)
//                    {
//                        _context.StudentAttendances.Remove(existingFieldAttendance);
//                    }
//                    if (existingFieldAbsent != null)
//                    {
//                        _context.StudentAbsents.Remove(existingFieldAbsent);
//                    }

//                    if (item.IsPresent)
//                    {
//                        var fieldAttendance = new StudentAttendance
//                        {
//                            StudentId = item.StudentId,
//                            StudentGrade = item.StudentGrade,
//                            Date = today,
//                            CreatedBy = createdBy,
//                            IsFieldAttendance = true,
//                            Without_Incentive = item.WithoutIncentive,
//                            Notes = item.Notes
//                        };
//                        _context.StudentAttendances.Add(fieldAttendance);
//                    }
//                    else
//                    {
//                        var fieldAbsent = new StudentAbsent
//                        {
//                            StudentId = item.StudentId,
//                            StudentGrade = item.StudentGrade,
//                            Date = today,
//                            CreatedBy = createdBy,
//                            IsFieldAttendance = true,
//                            AbsenceReasonId = item.AbsenceReasonId,
//                            CustomReasonDetails = item.CustomReasonDetails,
//                            Notes = item.Notes
//                        };
//                        _context.StudentAbsents.Add(fieldAbsent);
//                    }
//                }

//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public async Task<bool> CanTakeRegularAttendanceAsync()
//        {
//            var today = DateTime.Today;
//            var dayOfWeek = today.DayOfWeek;

//            // Can't take attendance on Friday and Saturday
//            return dayOfWeek != DayOfWeek.Friday && dayOfWeek != DayOfWeek.Saturday;
//        }

//        public async Task<bool> HasRegularAttendanceTodayAsync(int classId)
//        {
//            var today = DateTime.Today;
//            return await _context.StudentAttendances
//                .AnyAsync(sa => sa.Date.Date == today &&
//                          sa.Student.ClassId == classId &&
//                          !sa.IsFieldAttendance &&
//                          !sa.IsDeleted) ||
//                   await _context.StudentAbsents
//                .AnyAsync(sa => sa.Date.Date == today &&
//                          sa.Student.ClassId == classId &&
//                          !sa.IsFieldAttendance &&
//                          !sa.IsDeleted);
//        }

//        public async Task<bool> HasFieldAttendanceTodayAsync(int classId)
//        {
//            var today = DateTime.Today;
//            return await _context.StudentAttendances
//                .AnyAsync(sa => sa.Date.Date == today &&
//                          sa.Student.ClassId == classId &&
//                          sa.IsFieldAttendance &&
//                          !sa.IsDeleted) ||
//                   await _context.StudentAbsents
//                .AnyAsync(sa => sa.Date.Date == today &&
//                          sa.Student.ClassId == classId &&
//                          sa.IsFieldAttendance &&
//                          !sa.IsDeleted);
//        }

//        public async Task<bool> CanModifyAttendanceAsync(DateTime attendanceDate, int currentUserId, bool isAdmin)
//        {
//            var today = DateTime.Today;

//            // Admin can modify any date
//            if (isAdmin)
//                return true;

//            // Regular users can only modify today's attendance
//            return attendanceDate.Date == today;
//        }

//        public async Task<List<Class>> GetActiveClassesAsync()
//        {
//            return await _context.Classes
//                .Where(c => c.IsActive)
//                .Include(c => c.Field)
//                .OrderBy(c => c.Name)
//                .ToListAsync();
//        }

//        public async Task<bool> CreateExitRequestAsync(ExitRequestDto exitRequest, int createdBy)
//        {
//            try
//            {
//                var today = DateTime.Today;

//                // Check if student has regular attendance today
//                var attendanceId = await _context.StudentAttendances
//                    .Where(sa => sa.StudentId == exitRequest.StudentId &&
//                               sa.Date.Date == today &&
//                               !sa.IsFieldAttendance &&
//                               !sa.IsDeleted)
//                    .Select(sa => sa.Id)
//                    .FirstOrDefaultAsync();

//                if (attendanceId == 0)
//                    return false; // Student must be present to request exit

//                var request = new RequestExit
//                {
//                    StudentId = exitRequest.StudentId,
//                    AttendanceId = attendanceId,
//                    Reason = exitRequest.Reason,
//                    ExitTime = exitRequest.ExitTime,
//                    ExpectedReturnTime = exitRequest.ExpectedReturnTime,
//                    CreatedBy = createdBy,
//                    Date = DateTime.Now
//                };

//                _context.RequestExits.Add(request);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public async Task<List<RequestExit>> GetTodayExitRequestsAsync(int classId)
//        {
//            var today = DateTime.Today;
//            return await _context.RequestExits
//                .Where(re => re.Date.Date == today &&
//                           re.Student.ClassId == classId &&
//                           !re.IsDeleted)
//                .Include(re => re.Student)
//                .Include(re => re.CreatedByUser)
//                .Include(re => re.ProcessedByUser)
//                .OrderByDescending(re => re.Date)
//                .ToListAsync();
//        }
//    }

//    // DTOs
//    public class RegularAttendanceDto
//    {
//        public int StudentId { get; set; }
//        public int? StudentGrade { get; set; }
//        public bool IsPresent { get; set; }
//        public int? AbsenceReasonId { get; set; }
//        public string? CustomReasonDetails { get; set; }
//        public string? Notes { get; set; }
//    }

//    public class FieldAttendanceDto
//    {
//        public int StudentId { get; set; }
//        public int? StudentGrade { get; set; }
//        public bool IsPresent { get; set; }
//        public bool? WithoutIncentive { get; set; }
//        public int? AbsenceReasonId { get; set; }
//        public string? CustomReasonDetails { get; set; }
//        public string? Notes { get; set; }
//    }

//    public class ExitRequestDto
//    {
//        public int StudentId { get; set; }
//        public string Reason { get; set; }
//        public TimeSpan ExitTime { get; set; }
//        public TimeSpan? ExpectedReturnTime { get; set; }
//    }
//}