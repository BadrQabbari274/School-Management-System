using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class AcademicYearService : IAcademicYearService
    {
        private readonly ApplicationDbContext _context;

        public AcademicYearService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AcademicYear>> GetAllAcademicYearsAsync()
        {
            return await _context.AcademicYears
                .Include(ay => ay.CreatedByUser)
                .Where(ay => ay.IsActive)
                .OrderByDescending(ay => ay.Date)
                .ToListAsync();
        }

        public async Task<AcademicYear> GetAcademicYearByIdAsync(int id)
        {
            return await _context.AcademicYears
                .Include(ay => ay.CreatedByUser)
                .Include(ay => ay.Fields)
                .FirstOrDefaultAsync(ay => ay.Id == id && ay.IsActive);
        }

        public async Task<AcademicYear> CreateAcademicYearAsync(AcademicYear academicYear)
        {
            academicYear.Date = DateTime.Now;
            _context.AcademicYears.Add(academicYear);
            await _context.SaveChangesAsync();
            return academicYear;
        }

        public async Task<AcademicYear> UpdateAcademicYearAsync(AcademicYear academicYear)
        {
            _context.Entry(academicYear).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return academicYear;
        }

        public async Task<bool> DeleteAcademicYearAsync(int id)
        {
            var academicYear = await _context.AcademicYears.FindAsync(id);
            if (academicYear == null) return false;

            academicYear.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AcademicYear>> GetActiveAcademicYearsAsync()
        {
            return await _context.AcademicYears
                .Where(ay => ay.IsActive)
                .OrderByDescending(ay => ay.Date)
                .ToListAsync();
        }
    }
}
