using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface ITasksService
    {
        Task<IEnumerable<Tasks>> GetAllTasksAsync();
        Task<Tasks?> GetTaskByIdAsync(int id);
        Task<Tasks> CreateTaskAsync(Tasks task);
        Task<Tasks?> UpdateTaskAsync(Tasks task);
        Task<bool> DeleteTaskAsync(int id);
        Task<IEnumerable<Tasks>> GetTasksByCreatorIdAsync(int createdById);
        Task<IEnumerable<Tasks>> GetTasksByCompetencyIdAsync(int competencyId);
        Task<bool> TaskExistsAsync(int id);
        Task<IEnumerable<Employees>> GetActiveEmployeesAsync();
        Task<IEnumerable<Competencies>> GetActiveCompetenciesAsync();

        // New methods for Learning Outcomes and Evidence
        Task<Learning_Outcome?> GetLearningOutcomeByIdAsync(int id);
        Task<IEnumerable<Evidence>> GetEvidencesByLearningOutcomeIdAsync(int learningOutcomeId);
        Task<IEnumerable<Learning_Outcome>> GetActiveLearningOutcomesAsync();
        Task<Competencies?> GetCompetencyByLearningOutcomeIdAsync(int learningOutcomeId);
    }
}