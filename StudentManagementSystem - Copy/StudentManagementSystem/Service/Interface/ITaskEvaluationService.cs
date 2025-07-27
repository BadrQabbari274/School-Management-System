using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface ITaskEvaluationService
    {
        Task<IEnumerable<TaskEvaluation>> GetAllTaskEvaluationsAsync();
        Task<TaskEvaluation> GetTaskEvaluationByIdAsync(int id);
        Task<TaskEvaluation> CreateTaskEvaluationAsync(TaskEvaluation taskEvaluation);
        Task<TaskEvaluation> UpdateTaskEvaluationAsync(TaskEvaluation taskEvaluation);
        Task<bool> DeleteTaskEvaluationAsync(int id);
        Task<IEnumerable<TaskEvaluation>> GetActiveTaskEvaluationsAsync();
        Task<IEnumerable<TaskEvaluation>> GetTaskEvaluationsByStudentAsync(int studentId);
        Task<IEnumerable<TaskEvaluation>> GetTaskEvaluationsByOutcomeAsync(int outcomeId);
    }
}
