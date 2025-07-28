using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service
{
    public class WorkingYearService : IWorkingYearService
    {
        private readonly ApplicationDbContext _context;

        public WorkingYearService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Working_Year>> GetAllWorkingYearsAsync()
        {
            return await _context.Working_Years
                .Include(wy => wy.CreatedBy).Where(wy =>wy.IsActive)
                .OrderByDescending(wy => wy.Date)
                .ToListAsync();
        }

        public async Task<Working_Year?> GetWorkingYearByIdAsync(int id)
        {
            return await _context.Working_Years
                .Include(wy => wy.CreatedBy)
                .FirstOrDefaultAsync(wy => wy.Id == id&&wy.IsActive);
        }

        public async Task<Working_Year> CreateWorkingYearAsync(Working_Year workingYear)
        {
            workingYear.Date = DateTime.Now;
            workingYear.IsActive = true; // Set default to active

            _context.Working_Years.Add(workingYear);
            await _context.SaveChangesAsync();
            return workingYear;
        }

        public async Task<Working_Year> UpdateWorkingYearAsync(Working_Year workingYear)
        {
            _context.Entry(workingYear).State = EntityState.Modified;

            // Preserve original creation date and created by
            _context.Entry(workingYear).Property(x => x.Date).IsModified = false;
            _context.Entry(workingYear).Property(x => x.CreatedBy_Id).IsModified = false;

            await _context.SaveChangesAsync();
            return workingYear;
        }

        public async Task<bool> DeleteWorkingYearAsync(int id)
        {
            var workingYear = await _context.Working_Years.FindAsync(id);
            if (workingYear == null)
            {
                return false;
            }

           workingYear.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> WorkingYearExistsAsync(int id)
        {
            return await _context.Working_Years.AnyAsync(wy => wy.Id == id);
        }

        public async Task<bool> IsWorkingYearNameUniqueAsync(string name, int? excludeId = null)
        {
            var query = _context.Working_Years.Where(wy => wy.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(wy => wy.Id != excludeId.Value);
            }

            return !await query.AnyAsync();
        }
    }
}