using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IClassService
    {
        Task<IEnumerable<Class>> GetAllClassesAsync();
        Task<Class> GetClassByIdAsync(int id);
        Task<Class> CreateClassAsync(Class classEntity);
        Task<Class> UpdateClassAsync(Class classEntity);
        Task<bool> DeleteClassAsync(int id);
        Task<IEnumerable<Class>> GetActiveClassesAsync();
        Task<IEnumerable<Class>> GetClassesByFieldAsync(int fieldId);
    }
}
