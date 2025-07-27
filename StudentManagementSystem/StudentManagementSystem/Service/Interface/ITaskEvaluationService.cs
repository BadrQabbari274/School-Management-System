using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface ITaskEvaluationService
    {
        Task<IEnumerable<TaskEvaluations>> GetAllTaskEvaluationsAsync();
        Task<TaskEvaluations> GetTaskEvaluationByIdAsync(int id);
        Task<TaskEvaluations> CreateTaskEvaluationAsync(TaskEvaluations taskEvaluation);
        Task<TaskEvaluations> UpdateTaskEvaluationAsync(TaskEvaluations taskEvaluation);
        Task<bool> DeleteTaskEvaluationAsync(int id);
        Task<IEnumerable<TaskEvaluations>> GetActiveTaskEvaluationsAsync();
        Task<IEnumerable<TaskEvaluations>> GetTaskEvaluationsByStudentAsync(int studentId);
        Task<IEnumerable<TaskEvaluations>> GetTaskEvaluationsByOutcomeAsync(int outcomeId);
    }
}
