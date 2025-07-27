using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IStudentManagementService
    {
        Task<IEnumerable<Students>> GetStudentsWithCompleteDataAsync();
        Task<IEnumerable<TaskEvaluations>> GetTaskEvaluationsWithPicturesAsync(int studentId);
        Task<bool> SubmitTaskWithPicturesAsync(int taskId, int studentId, IList<IFormFile> pictures, int createdBy);
        Task<IEnumerable<Students>> GetStudentsByFieldAsync(int fieldId);
        Task<Dictionary<string, object>> GetStudentDashboardDataAsync(int studentId);
        Task<Dictionary<string, object>> GetFieldStatisticsAsync(int fieldId);
        Task<bool> BulkUpdateAttendanceAsync(List<StudentAttendances> attendances);
    }
}
