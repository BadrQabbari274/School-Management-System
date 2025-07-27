using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IClassService
    {
        Task<IEnumerable<Classes>> GetAllClassesAsync();
        Task<Classes> GetClassByIdAsync(int id);
        Task<Classes> CreateClassAsync(Classes classEntity);
        Task<Classes> UpdateClassAsync(Classes classEntity);
        Task<bool> DeleteClassAsync(int id);
        Task<IEnumerable<Classes>> GetActiveClassesAsync();
        Task<IEnumerable<Classes>> GetClassesByFieldAsync(int fieldId);
    }
}
