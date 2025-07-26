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
                .Include(ar => ar.MajorAttendances)
                .FirstOrDefaultAsync(ar => ar.Id == id && !ar.IsDeleted);
        }

        public async Task<AbsenceReason> CreateAbsenceReasonAsync(AbsenceReason absenceReason)
        {
            absenceReason.CreatedDate = DateTime.Now;
            _context.AbsenceReasons.Add(absenceReason);
            await _context.SaveChangesAsync();
            return absenceReason;
        }

        public async Task<AbsenceReason> UpdateAbsenceReasonAsync(AbsenceReason absenceReason)
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

        public async Task<IEnumerable<AbsenceReason>> GetActiveAbsenceReasonsAsync()
        {
            return await _context.AbsenceReasons
                .Where(ar => !ar.IsDeleted)
                .OrderBy(ar => ar.Name)
                .ToListAsync();
        }
    }
}
