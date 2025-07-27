using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<EmployeeType>> GetAllRolesAsync();
        Task<EmployeeType> GetRoleByIdAsync(int id);
        Task<EmployeeType> CreateRoleAsync(EmployeeType role);
        Task<EmployeeType> UpdateRoleAsync(EmployeeType role);
        Task<bool> DeleteRoleAsync(int id);
        Task<IEnumerable<EmployeeType>> GetActiveRolesAsync();
    }
}
