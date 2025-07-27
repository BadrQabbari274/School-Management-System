using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class TaskEvaluationService : ITaskEvaluationService
    {
        private readonly ApplicationDbContext _context;

        public TaskEvaluationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskEvaluation>> GetAllTaskEvaluationsAsync()
        {
            return await _context.TaskEvaluations
                .Include(te => te.Student)
                .Include(te => te.Outcome)
                .ThenInclude(o => o.Competence)
                .Include(te => te.Pictures)
                .Where(te => !te.IsDeleted)
                .ToListAsync();
        }

        public async Task<TaskEvaluation> GetTaskEvaluationByIdAsync(int id)
        {
            return await _context.TaskEvaluations
                .Include(te => te.Student)
                .Include(te => te.Outcome)
                .ThenInclude(o => o.Competence)
                .Include(te => te.Pictures)
                .FirstOrDefaultAsync(te => te.Id == id && !te.IsDeleted);
        }

        public async Task<TaskEvaluation> CreateTaskEvaluationAsync(TaskEvaluation taskEvaluation)
        {
            _context.TaskEvaluations.Add(taskEvaluation);
            await _context.SaveChangesAsync();
            return taskEvaluation;
        }

        public async Task<TaskEvaluation> UpdateTaskEvaluationAsync(TaskEvaluation taskEvaluation)
        {
            _context.Entry(taskEvaluation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return taskEvaluation;
        }

        public async Task<bool> DeleteTaskEvaluationAsync(int id)
        {
            var taskEvaluation = await _context.TaskEvaluations.FindAsync(id);
            if (taskEvaluation == null) return false;

            taskEvaluation.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskEvaluation>> GetActiveTaskEvaluationsAsync()
        {
            return await _context.TaskEvaluations
                .Where(te => !te.IsDeleted)
                .Include(te => te.Student)
                .Include(te => te.Outcome)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskEvaluation>> GetTaskEvaluationsByStudentAsync(int studentId)
        {
            return await _context.TaskEvaluations
                .Where(te => te.StudentId == studentId && !te.IsDeleted)
                .Include(te => te.Outcome)
                .ThenInclude(o => o.Competence)
                .Include(te => te.Pictures)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskEvaluation>> GetTaskEvaluationsByOutcomeAsync(int outcomeId)
        {
            return await _context.TaskEvaluations
                .Where(te => te.OutcomeId == outcomeId && !te.IsDeleted)
                .Include(te => te.Student)
                .Include(te => te.Pictures)
                .ToListAsync();
        }
    }
}
