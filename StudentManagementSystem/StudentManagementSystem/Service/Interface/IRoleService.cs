using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<EmployeeTypes>> GetAllRolesAsync();
        Task<EmployeeTypes> GetRoleByIdAsync(int id);
        Task<EmployeeTypes> CreateRoleAsync(EmployeeTypes role);
        Task<EmployeeTypes> UpdateRoleAsync(EmployeeTypes role);
        Task<bool> DeleteRoleAsync(int id);
        Task<IEnumerable<EmployeeTypes>> GetActiveRolesAsync();
    }
}
