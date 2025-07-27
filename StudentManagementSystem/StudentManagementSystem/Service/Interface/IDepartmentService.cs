using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllFieldsAsync();
        Task<Department> GetFieldByIdAsync(int id);
        Task<Department> CreateFieldAsync(Department field);
        Task<Department> UpdateFieldAsync(Department field);
        Task<bool> DeleteFieldAsync(int id);
        Task<IEnumerable<Department>> GetActiveFieldsAsync();
        Task<IEnumerable<Department>> GetFieldsByAcademicYearAsync(int academicYearId);
        Task<bool> AssignUserToFieldAsync(int userId, int fieldId);
        Task<bool> RemoveUserFromFieldAsync(int userId, int fieldId);
    }
}
