using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class AbsenceReasonService : IAbsenceReasonService
    {
        private readonly ApplicationDbContext _context;

        public AbsenceReasonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AbsenceReasons>> GetAllAbsenceReasonsAsync()
        {
            return await _context.AbsenceReasons
                .Include(ar => ar.CreatedBy)
                .Where(ar => !ar.IsDeleted)
                .OrderBy(ar => ar.Name)
                .ToListAsync();
        }

        public async Task<AbsenceReasons> GetAbsenceReasonByIdAsync(int id)
        {
            return await _context.AbsenceReasons
                .Include(ar => ar.CreatedBy)
                .Include(ar => ar.StudentAbsents)
                .FirstOrDefaultAsync(ar => ar.Id == id && !ar.IsDeleted);
        }

        public async Task<AbsenceReasons> CreateAbsenceReasonAsync(AbsenceReasons absenceReason)
        {
            absenceReason.CreatedDate = DateTime.Now;
            _context.AbsenceReasons.Add(absenceReason);
            await _context.SaveChangesAsync();
            return absenceReason;
        }

        public async Task<AbsenceReasons> UpdateAbsenceReasonAsync(AbsenceReasons absenceReason)
        {
            _context.Entry(absenceReason).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return absenceReason;
        }

        public async Task<bool> DeleteAbsenceReasonAsync(int id)
        {
            var absenceReason = await _context.AbsenceReasons.FindAsync(id);
            if (absenceReason == null) return false;

            absenceReason.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AbsenceReasons>> GetActiveAbsenceReasonsAsync()
        {
            return await _context.AbsenceReasons
                .Where(ar => !ar.IsDeleted)
                .OrderBy(ar => ar.Name)
                .ToListAsync();
        }
    }
}
