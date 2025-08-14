using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service
{
    public class TasksService : ITasksService
    {
        private readonly ApplicationDbContext _context;

        public TasksService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tasks>> GetAllTasksAsync()
        {
            return await _context.Set<Tasks>()
                .Include(t => t.CreatedBy)
                .Include(t => t.Competencies)
                .Include(t => t.Evidences)
                .Include(t => t.Student_Tasks)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
        }

        public async Task<Tasks?> GetTaskByIdAsync(int id)
        {
            return await _context.Set<Tasks>()
                .Include(t => t.CreatedBy)
                .Include(t => t.Competencies)
                .Include(t => t.Evidences)
                .Include(t => t.Student_Tasks)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tasks> CreateTaskAsync(Tasks task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            task.CreatedDate = DateTime.Now;

            _context.Set<Tasks>().Add(task);
            await _context.SaveChangesAsync();

            return await GetTaskByIdAsync(task.Id);
        }

        public async Task<Tasks?> UpdateTaskAsync(Tasks task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            var existingTask = await _context.Set<Tasks>().FindAsync(task.Id);
            if (existingTask == null)
                return null;

            existingTask.Name = task.Name;
            existingTask.CreatedBy_Id = task.CreatedBy_Id;
            existingTask.Competencies_Id = task.Competencies_Id;
            // Note: CreatedDate usually shouldn't be updated

            _context.Set<Tasks>().Update(existingTask);
            await _context.SaveChangesAsync();

            return await GetTaskByIdAsync(task.Id);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Set<Tasks>().FindAsync(id);
            if (task == null)
                return false;

            _context.Set<Tasks>().Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Tasks>> GetTasksByCreatorIdAsync(int createdById)
        {
            return await _context.Set<Tasks>()
                .Include(t => t.CreatedBy)
                .Include(t => t.Competencies)
                .Where(t => t.CreatedBy_Id == createdById)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tasks>> GetTasksByCompetencyIdAsync(int competencyId)
        {
            return await _context.Set<Tasks>()
                .Include(t => t.CreatedBy)
                .Include(t => t.Competencies)
                .Where(t => t.Competencies_Id == competencyId)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
        }

        public async Task<bool> TaskExistsAsync(int id)
        {
            return await _context.Set<Tasks>().AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Employees>> GetActiveEmployeesAsync()
        {
            return await _context.Set<Employees>()
                .Where(e => e.IsActive == true)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Competencies>> GetActiveCompetenciesAsync()
        {
            return await _context.Set<Competencies>()
                .Where(c => c.IsActive == true)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        // New methods for Learning Outcomes and Evidence
        public async Task<Learning_Outcome?> GetLearningOutcomeByIdAsync(int id)
        {
            return await _context.Set<Learning_Outcome>()
                .FirstOrDefaultAsync(lo => lo.Id == id);
        }

        public async Task<IEnumerable<Evidence>> GetEvidencesByLearningOutcomeIdAsync(int learningOutcomeId)
        {
            return await _context.Set<Evidence>()
                .Where(e => e.Id == learningOutcomeId && e.IsActive == true)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Learning_Outcome>> GetActiveLearningOutcomesAsync()
        {
            return await _context.Set<Learning_Outcome>()
                .Where(lo => lo.IsActive == true)
                .OrderBy(lo => lo.Name)
                .ToListAsync();
        }

        public async Task<Competencies?> GetCompetencyByLearningOutcomeIdAsync(int learningOutcomeId)
        {
            var learningOutcome = await _context.Set<Learning_Outcome>()
                .Include(lo => lo.Competency)
                .FirstOrDefaultAsync(lo => lo.Id == learningOutcomeId);

            return learningOutcome?.Competency;
        }
    }
}