using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(Student student, IFormFile profileImage = null, IFormFile birthCertificate = null);
        Task<Student> UpdateStudentAsync(Student student, IFormFile profileImage = null, IFormFile birthCertificate = null);
        Task<bool> DeleteStudentAsync(int id);
        Task<IEnumerable<Student>> GetActiveStudentsAsync();
        Task<IEnumerable<Student>> GetStudentsByClassAsync(int classId);
        Task<IEnumerable<Student>> GetStudentsWithTasksAsync();
    }
}