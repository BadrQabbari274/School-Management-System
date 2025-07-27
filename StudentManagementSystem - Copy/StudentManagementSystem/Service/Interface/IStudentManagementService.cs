using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IStudentManagementService
    {
        Task<IEnumerable<Student>> GetStudentsWithCompleteDataAsync();
        Task<IEnumerable<TaskEvaluation>> GetTaskEvaluationsWithPicturesAsync(int studentId);
        Task<bool> SubmitTaskWithPicturesAsync(int taskId, int studentId, IList<IFormFile> pictures, int createdBy);
        Task<IEnumerable<Student>> GetStudentsByFieldAsync(int fieldId);
        Task<Dictionary<string, object>> GetStudentDashboardDataAsync(int studentId);
        Task<Dictionary<string, object>> GetFieldStatisticsAsync(int fieldId);
        Task<bool> BulkUpdateAttendanceAsync(List<StudentAttendance> attendances);
    }
}
