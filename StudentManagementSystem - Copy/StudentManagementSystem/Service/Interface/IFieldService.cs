using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IFieldService
    {
        Task<IEnumerable<Field>> GetAllFieldsAsync();
        Task<Field> GetFieldByIdAsync(int id);
        Task<Field> CreateFieldAsync(Field field);
        Task<Field> UpdateFieldAsync(Field field);
        Task<bool> DeleteFieldAsync(int id);
        Task<IEnumerable<Field>> GetActiveFieldsAsync();
        Task<IEnumerable<Field>> GetFieldsByAcademicYearAsync(int academicYearId);
        Task<bool> AssignUserToFieldAsync(int userId, int fieldId);
        Task<bool> RemoveUserFromFieldAsync(int userId, int fieldId);
    }
}
