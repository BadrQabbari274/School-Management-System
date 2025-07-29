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

        public async Task<IEnumerable<Grades>> GetAllAcademicYearsAsync()
        {
            return await _context.Grades
                .Include(ay => ay.CreatedBy)
                .Where(ay => ay.IsActive)
                .OrderByDescending(ay => ay.Date)
                .ToListAsync();
        }

        public async Task<Grades> GetAcademicYearByIdAsync(int id)
        {
            return await _context.Grades
                .Include(ay => ay.CreatedBy)
                .FirstOrDefaultAsync(ay => ay.Id == id && ay.IsActive);
        }
        public async Task<Grades> GetAcademicYearByNameAsync(string name)
        {
            return await _context.Grades
                .Include(ay => ay.CreatedBy)
                .FirstOrDefaultAsync(ay => ay.Name.ToLower() == name.ToLower() && ay.IsActive);
        }
        public async Task<Grades> CreateAcademicYearAsync(Grades academicYear)
        {
            academicYear.Date = DateTime.Now;
            _context.Grades.Add(academicYear);
            await _context.SaveChangesAsync();
            return academicYear;
        }

        public async Task<Grades> UpdateAcademicYearAsync(Grades academicYear)
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

        public async Task<IEnumerable<Grades>> GetActiveAcademicYearsAsync()
        {
            return await _context.Grades
                .Where(ay => ay.IsActive)
                .OrderByDescending(ay => ay.Date)
                .ToListAsync();
        }
    }
}
