using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        #endregion

        #region Absence Reason Operations

        public async Task<IEnumerable<AbsenceReason>> GetAllAbsenceReasonsAsync()
        {
            return await _context.AbsenceReasons
                .Include(ar => ar.CreatedByUser)
                .Where(ar => !ar.IsDeleted)
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

        #endregion

        #region Attendance Type Operations

        public async Task<IEnumerable<AttendanceType>> GetAllAttendanceTypesAsync()
        {
            return await _context.AttendanceTypes.ToListAsync();
        }

        public async Task<AttendanceType> GetAttendanceTypeByIdAsync(int id)
        {
            return await _context.AttendanceTypes.FindAsync(id);
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

        #region Class Attendance Report

        public async Task<ClassAttendanceReport> GetClassAttendanceReportAsync(int classId, DateTime date)
        {
            // Get all students in the class
            var students = await _context.Students
                .Where(s => s.ClassId == classId && s.IsActive)
                .Include(s => s.Class)
                .ToListAsync();

            // Get present students
            var presentStudents = await _context.StudentAttendances
                .Include(sa => sa.Student)
                .Where(sa => sa.StudentId.HasValue &&
                           students.Select(s => s.Id).Contains(sa.StudentId.Value) &&
                           sa.Date.Date == date.Date &&
                           !sa.IsDeleted)
                .ToListAsync();

            // Get absent students
            var absentStudents = await _context.StudentAbsents
                .Include(sa => sa.Student)
                .Include(sa => sa.AbsenceReason)
                .Include(sa => sa.Teacher)
                .Where(sa => sa.StudentId.HasValue &&
                           students.Select(s => s.Id).Contains(sa.StudentId.Value) &&
                           sa.Date.Date == date.Date &&
                           !sa.IsDeleted)
                .ToListAsync();

            // Create attendance items
            var attendanceItems = new List<AttendanceItem>();

            // Add present students
            foreach (var attendance in presentStudents)
            {
                attendanceItems.Add(new AttendanceItem
                {
                    Student = attendance.Student,
                    AttendanceStatus = AttendanceStatus.Present,
                    State = attendance.State,
                    Date = attendance.Date
                });
            }

            // Add absent students
            foreach (var absent in absentStudents)
            {
                attendanceItems.Add(new AttendanceItem
                {
                    Student = absent.Student,
                    AttendanceStatus = AttendanceStatus.Absent,
                    AbsenceReason = absent.AbsenceReason?.Name,
                    AttendanceType = absent.AttendanceType,
                    Teacher = absent.Teacher?.Name,
                    Date = absent.Date
                });
            }

            // Add students with no attendance record (considered absent)
            var recordedStudentIds = attendanceItems.Select(ai => ai.Student.Id).ToList();
            var unrecordedStudents = students.Where(s => !recordedStudentIds.Contains(s.Id));

            foreach (var student in unrecordedStudents)
            {
                attendanceItems.Add(new AttendanceItem
                {
                    Student = student,
                    AttendanceStatus = AttendanceStatus.Absent,
                    Date = date
                });
            }

            return new ClassAttendanceReport
            {
                ClassId = classId,
                ClassName = students.FirstOrDefault()?.Class?.Name,
                Date = date,
                AttendanceItems = attendanceItems,
                TotalStudents = students.Count,
                PresentCount = presentStudents.Count,
                AbsentCount = absentStudents.Count + unrecordedStudents.Count()
            };
        }

        public async Task<ClassAttendanceReport> GetClassAttendanceReportByDateRangeAsync(int classId, DateTime startDate, DateTime endDate)
        {
            // Get all students in the class
            var students = await _context.Students
                .Where(s => s.ClassId == classId && s.IsActive)
                .Include(s => s.Class)
                .ToListAsync();

            // Get present students in date range
            var presentStudents = await _context.StudentAttendances
                .Include(sa => sa.Student)
                .Where(sa => sa.StudentId.HasValue &&
                           students.Select(s => s.Id).Contains(sa.StudentId.Value) &&
                           sa.Date.Date >= startDate.Date &&
                           sa.Date.Date <= endDate.Date &&
                           !sa.IsDeleted)
                .ToListAsync();

            // Get absent students in date range
            var absentStudents = await _context.StudentAbsents
                .Include(sa => sa.Student)
                .Include(sa => sa.AbsenceReason)
                .Include(sa => sa.Teacher)
                .Where(sa => sa.StudentId.HasValue &&
                           students.Select(s => s.Id).Contains(sa.StudentId.Value) &&
                           sa.Date.Date >= startDate.Date &&
                           sa.Date.Date <= endDate.Date &&
                           !sa.IsDeleted)
                .ToListAsync();

            // Create attendance items
            var attendanceItems = new List<AttendanceItem>();

            // Add present students
            foreach (var attendance in presentStudents)
            {
                attendanceItems.Add(new AttendanceItem
                {
                    Student = attendance.Student,
                    AttendanceStatus = AttendanceStatus.Present,
                    State = attendance.State,
                    Date = attendance.Date
                });
            }

            // Add absent students
            foreach (var absent in absentStudents)
            {
                attendanceItems.Add(new AttendanceItem
                {
                    Student = absent.Student,
                    AttendanceStatus = AttendanceStatus.Absent,
                    AbsenceReason = absent.AbsenceReason?.Name,
                    AttendanceType = absent.AttendanceType,
                    Teacher = absent.Teacher?.Name,
                    Date = absent.Date
                });
            }

            return new ClassAttendanceReport
            {
                ClassId = classId,
                ClassName = students.FirstOrDefault()?.Class?.Name,
                StartDate = startDate,
                EndDate = endDate,
                AttendanceItems = attendanceItems,
                TotalStudents = students.Count,
                PresentCount = presentStudents.Count,
                AbsentCount = absentStudents.Count
            };
        }

        #endregion
    }

    // Helper Classes for Attendance Report
    public class ClassAttendanceReport
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public DateTime Date { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<AttendanceItem> AttendanceItems { get; set; } = new List<AttendanceItem>();
        public int TotalStudents { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public double AttendancePercentage => TotalStudents > 0 ? (double)PresentCount / TotalStudents * 100 : 0;
    }

    public class AttendanceItem
    {
        public Student Student { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; }
        public string State { get; set; } // For present students
        public string AbsenceReason { get; set; } // For absent students
        public int? AttendanceType { get; set; } // For absent students
        public string Teacher { get; set; } // Teacher who marked the absence
        public DateTime Date { get; set; }
    }

    public enum AttendanceStatus
    {
        Present,
        Absent
    }
}

