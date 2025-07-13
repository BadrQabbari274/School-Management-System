using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task<Role> CreateRoleAsync(Role role);
        Task<Role> UpdateRoleAsync(Role role);
        Task<bool> DeleteRoleAsync(int id);
        Task<IEnumerable<Role>> GetActiveRolesAsync();
    }
}
