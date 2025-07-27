using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeTypes>> GetAllRolesAsync()
        {
            return await _context.EmployeeTypes
                .Include(r => r.crea)
                .Where(r => !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<EmployeeType> GetRoleByIdAsync(int id)
        {
            return await _context.EmployeeTypes
                .Include(r => r.CreatedBy)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<EmployeeType> CreateRoleAsync(EmployeeType role)
        {
            role.CreatedDate = DateTime.Now;
            _context.EmployeeTypes.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<EmployeeType> UpdateRoleAsync(EmployeeType role)
        {
            _context.Entry(role).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _context.EmployeeTypes.FindAsync(id);
            if (role == null) return false;

            role.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EmployeeType>> GetActiveRolesAsync()
        {
            return await _context.EmployeeTypes
                .Where(r => !r.IsDeleted)
                .ToListAsync();
        }
    }

}
