//using Microsoft.EntityFrameworkCore;
//using StudentManagementSystem.Data;
//using StudentManagementSystem.Models;
//using StudentManagementSystem.Service.Interface;

//namespace StudentManagementSystem.Service.Implementation
//{
//    public class CompetenceService : ICompetenceService
//    {
//        private readonly ApplicationDbContext _context;

//        public CompetenceService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<Competence>> GetAllCompetencesAsync()
//        {
//            return await _context.Competences
//                .Include(c => c.Field)
//                .Include(c => c.CreatedByUser)
//                .Include(c => c.Outcomes)
//                .Where(c => c.IsActive)
//                .ToListAsync();
//        }

//        public async Task<Competence> GetCompetenceByIdAsync(int id)
//        {
//            return await _context.Competences
//                .Include(c => c.Field)
//                .Include(c => c.CreatedByUser)
//                .Include(c => c.Outcomes)
//                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
//        }

//        public async Task<Competence> CreateCompetenceAsync(Competence competence)
//        {
//            competence.CreatedDate = DateTime.Now;
//            _context.Competences.Add(competence);
//            await _context.SaveChangesAsync();
//            return competence;
//        }

//        public async Task<Competence> UpdateCompetenceAsync(Competence competence)
//        {
//            _context.Entry(competence).State = EntityState.Modified;
//            await _context.SaveChangesAsync();
//            return competence;
//        }

//        public async Task<bool> DeleteCompetenceAsync(int id)
//        {
//            var competence = await _context.Competences.FindAsync(id);
//            if (competence == null) return false;

//            competence.IsActive = false;
//            await _context.SaveChangesAsync();
//            return true;
//        }

//        public async Task<IEnumerable<Competence>> GetActiveCompetencesAsync()
//        {
//            return await _context.Competences
//                .Where(c => c.IsActive)
//                .Include(c => c.Field)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Competence>> GetCompetencesByFieldAsync(int fieldId)
//        {
//            return await _context.Competences
//                .Where(c => c.FieldId == fieldId && c.IsActive)
//                .Include(c => c.Outcomes)
//                .ToListAsync();
//        }
//    }

//    // Outcome Service Implementation
//    public class OutcomeService : IOutcomeService
//    {
//        private readonly ApplicationDbContext _context;

//        public OutcomeService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<Outcome>> GetAllOutcomesAsync()
//        {
//            return await _context.Outcomes
//                .Include(o => o.Competence)
//                .ThenInclude(c => c.Field)
//                .Where(o => o.IsActive)
//                .ToListAsync();
//        }

//        public async Task<Outcome> GetOutcomeByIdAsync(int id)
//        {
//            return await _context.Outcomes
//                .Include(o => o.Competence)
//                .ThenInclude(c => c.Field)
//                .Include(o => o.TaskEvaluations)
//                .FirstOrDefaultAsync(o => o.Id == id && o.IsActive);
//        }

//        public async Task<Outcome> CreateOutcomeAsync(Outcome outcome)
//        {
//            _context.Outcomes.Add(outcome);
//            await _context.SaveChangesAsync();
//            return outcome;
//        }

//        public async Task<Outcome> UpdateOutcomeAsync(Outcome outcome)
//        {
//            _context.Entry(outcome).State = EntityState.Modified;
//            await _context.SaveChangesAsync();
//            return outcome;
//        }

//        public async Task<bool> DeleteOutcomeAsync(int id)
//        {
//            var outcome = await _context.Outcomes.FindAsync(id);
//            if (outcome == null) return false;

//            outcome.IsActive = false;
//            await _context.SaveChangesAsync();
//            return true;
//        }

//        public async Task<IEnumerable<Outcome>> GetActiveOutcomesAsync()
//        {
//            return await _context.Outcomes
//                .Where(o => o.IsActive)
//                .Include(o => o.Competence)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Outcome>> GetOutcomesByCompetenceAsync(int competenceId)
//        {
//            return await _context.Outcomes
//                .Where(o => o.CompId == competenceId && o.IsActive)
//                .ToListAsync();
//        }
//    }

//}
