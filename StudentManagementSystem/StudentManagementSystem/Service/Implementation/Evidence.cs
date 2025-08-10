using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.Services
{
    public class EvidenceService : IEvidenceService
    {
        private readonly ApplicationDbContext _context;

        public EvidenceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Evidence>> GetAllEvidencesAsync()
        {
            return await _context.Evidences
                .Include(e => e.CreatedBy)
                .Include(e => e.Learning_Outcome)
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }

        public async Task<Evidence?> GetEvidenceByIdAsync(int id)
        {
            return await _context.Evidences
                .Include(e => e.CreatedBy)
                .Include(e => e.Learning_Outcome)
                .Include(e => e.Student_Evidences)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Evidence>> GetEvidencesByOutcomeIdAsync(int outcomeId)
        {
            return await _context.Evidences
                .Include(e => e.CreatedBy)
                .Include(e => e.Learning_Outcome)
                .Where(e => e.Outcome_Id == outcomeId)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evidence>> GetActiveEvidencesAsync()
        {
            return await _context.Evidences
                .Include(e => e.CreatedBy)
                .Include(e => e.Learning_Outcome)
                .Where(e => e.IsActive)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evidence>> GetPracticalEvidencesAsync()
        {
            return await _context.Evidences
                .Include(e => e.CreatedBy)
                .Include(e => e.Learning_Outcome)
                .Where(e => e.Ispractical && e.IsActive)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<Evidence> CreateEvidenceAsync(Evidence evidence)
        {
            evidence.CreatedDate = DateTime.Now;

            _context.Evidences.Add(evidence);
            await _context.SaveChangesAsync();

            return await GetEvidenceByIdAsync(evidence.Id);
        }

        public async Task<Evidence?> UpdateEvidenceAsync(int id, Evidence evidence)
        {
            var existingEvidence = await _context.Evidences.FindAsync(id);

            if (existingEvidence == null)
                return null;

            existingEvidence.Name = evidence.Name;
            existingEvidence.Ispractical = evidence.Ispractical;
            existingEvidence.IsActive = evidence.IsActive;
            existingEvidence.Outcome_Id = evidence.Outcome_Id;

            await _context.SaveChangesAsync();

            return await GetEvidenceByIdAsync(id);
        }

        public async Task<bool> DeleteEvidenceAsync(int id)
        {
            var evidence = await _context.Evidences.FindAsync(id);

            if (evidence == null)
                return false;

            evidence.IsActive = false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EvidenceExistsAsync(int id)
        {
            return await _context.Evidences.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> ToggleEvidenceStatusAsync(int id)
        {
            var evidence = await _context.Evidences.FindAsync(id);

            if (evidence == null)
                return false;

            evidence.IsActive = !evidence.IsActive;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}