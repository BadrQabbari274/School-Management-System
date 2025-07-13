using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Major Attendance Methods
        public async Task<IEnumerable<MajorAttendance>> GetAllMajorAttendancesAsync()
        {
            return await _context.MajorAttendances
                .Include(ma => ma.Student)
                .ThenInclude(s => s.Class)
                .Include(ma => ma.AbsenceReason)
                .Include(ma => ma.CreatedByUser)
                .OrderByDescending(ma => ma.Date)
                .ToListAsync();
        }

        public async Task<MajorAttendance> GetMajorAttendanceByIdAsync(int id)
        {
            return await _context.MajorAttendances
                .Include(ma => ma.Student)
                .Include(ma => ma.AbsenceReason)
                .Include(ma => ma.CreatedByUser)
                .FirstOrDefaultAsync(ma => ma.Id == id);
        }

        public async Task<MajorAttendance> CreateMajorAttendanceAsync(MajorAttendance attendance)
        {
            attendance.CreatedDate = DateTime.Now;
            _context.MajorAttendances.Add(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task<MajorAttendance> UpdateMajorAttendanceAsync(MajorAttendance attendance)
        {
            _context.Entry(attendance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task<bool> DeleteMajorAttendanceAsync(int id)
        {
            var attendance = await _context.MajorAttendances.FindAsync(id);
            if (attendance == null) return false;

            _context.MajorAttendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MajorAttendance>> GetMajorAttendancesByStudentAsync(int studentId)
        {
            return await _context.MajorAttendances
                .Where(ma => ma.StudentId == studentId)
                .Include(ma => ma.AbsenceReason)
                .OrderByDescending(ma => ma.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<MajorAttendance>> GetMajorAttendancesByDateAsync(DateTime date)
        {
            return await _context.MajorAttendances
                .Where(ma => ma.Date.Date == date.Date)
                .Include(ma => ma.Student)
                .Include(ma => ma.AbsenceReason)
                .ToListAsync();
        }

        // Student Attendance Methods
        public async Task<IEnumerable<StudentAttendance>> GetAllStudentAttendancesAsync()
        {
            return await _context.StudentAttendances
                .Include(sa => sa.Student)
                .ThenInclude(s => s.Class)
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

        public async Task<StudentAttendance> CreateStudentAttendanceAsync(StudentAttendance attendance)
        {
            attendance.Date = DateTime.Now;
            _context.StudentAttendances.Add(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task<StudentAttendance> UpdateStudentAttendanceAsync(StudentAttendance attendance)
        {
            _context.Entry(attendance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task<bool> DeleteStudentAttendanceAsync(int id)
        {
            var attendance = await _context.StudentAttendances.FindAsync(id);
            if (attendance == null) return false;

            attendance.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<StudentAttendance>> GetStudentAttendancesByStudentAsync(int studentId)
        {
            return await _context.StudentAttendances
                .Where(sa => sa.StudentId == studentId && !sa.IsDeleted)
                .OrderByDescending(sa => sa.Date)
                .ToListAsync();
        }
    }

}
