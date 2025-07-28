using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync(); 
        Task<Department> GetDepartmentByIdAsync(int id); 
        Task<Department> CreateDepartmentAsync(Department department); 
        Task<Department> UpdateDepartmentAsync(Department department); 
        Task<bool> DeleteDepartmentAsync(int id); 
        Task<IEnumerable<Department>> GetActiveDepartmentsAsync(); 
        Task<IEnumerable<Department>> GetDepartmentsByAcademicYearAsync(int academicYearId); 
        Task<bool> AssignUserToDepartmentAsync(int userId, int departmentId); 
        Task<bool> RemoveUserFromDepartmentAsync(int userId, int departmentId); 
    }
}