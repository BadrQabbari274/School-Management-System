using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class GradeService : IGradeService
    {
        private readonly ApplicationDbContext _context;

        public GradeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Grade>> GetAllAcademicYearsAsync()
        {
            return await _context.Grades
                .Include(ay => ay.CreatedByUser)
                .Where(ay => ay.IsActive)
                .OrderByDescending(ay => ay.Date)
                .ToListAsync();
        }

        public async Task<Grade> GetAcademicYearByIdAsync(int id)
        {
            return await _context.Grades
                .Include(ay => ay.CreatedByUser)
                .Include(ay => ay.Fields)
                .FirstOrDefaultAsync(ay => ay.Id == id && ay.IsActive);
        }

        public async Task<Grade> CreateAcademicYearAsync(Grade academicYear)
        {
            academicYear.Date = DateTime.Now;
            _context.Grades.Add(academicYear);
            await _context.SaveChangesAsync();
            return academicYear;
        }

        public async Task<Grade> UpdateAcademicYearAsync(Grade academicYear)
        {
            _context.Entry(academicYear).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return academicYear;
        }

        public async Task<bool> DeleteAcademicYearAsync(int id)
        {
            var academicYear = await _context.Grades.FindAsync(id);
            if (academicYear == null) return false;

            academicYear.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Grade>> GetActiveAcademicYearsAsync()
        {
            return await _context.Grades
                .Where(ay => ay.IsActive)
                .OrderByDescending(ay => ay.Date)
                .ToListAsync();
        }
    }
}
