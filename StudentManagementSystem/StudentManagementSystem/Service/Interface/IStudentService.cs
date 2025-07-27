using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IStudentService
    {
        Task<IEnumerable<Students>> GetAllStudentsAsync();
        Task<Students> GetStudentByIdAsync(int id);
        Task<Students> CreateStudentAsync(Students student, IFormFile profileImage = null, IFormFile birthCertificate = null);
        Task<Students> UpdateStudentAsync(Students student, IFormFile profileImage = null, IFormFile birthCertificate = null);
        Task<bool> DeleteStudentAsync(int id);
        Task<IEnumerable<Students>> GetActiveStudentsAsync();
        Task<IEnumerable<Students>> GetStudentsByClassAsync(int classId);
        Task<IEnumerable<Students>> GetStudentsWithTasksAsync();
    }
}