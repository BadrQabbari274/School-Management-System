using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
using StudentManagementSystem.Data;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllUsersAsync()
        {
            return await _context.Employees
                .Include(u => u.Role)
                .Include(u => u.CreatedByUser)
                .ToListAsync();
        }

        public async Task<Employee> GetUserByIdAsync(int id)
        {
            return await _context.Employees
                .Include(u => u.Role)
                .Include(u => u.CreatedByUser)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Employee> CreateUserAsync(Employee user)
        {
            user.Date = DateTime.Now;
            _context.Employees.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        //public async Task<Employee> UpdateUserAsync(Employee user)
        //{
        //    //_context.Entry(user).State = EntityState.Modified;
        //    var User = _context.Employees.FirstOrDefault(e =>e.Id == user.Id);
        //    User = user;
        //    await _context.SaveChangesAsync();
        //    return user;
        //}
        public async Task<Employee> UpdateUserAsync(Employee user)
        {
            var existingUser = await _context.Employees.FirstOrDefaultAsync(e => e.Id == user.Id);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            // Update the properties of the existing entity
            _context.Entry(existingUser).CurrentValues.SetValues(user);

            await _context.SaveChangesAsync();
            return existingUser;
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Employees.FindAsync(id);
            if (user == null) return false;

            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Employee>> GetActiveUsersAsync()
        {
            return await _context.Employees
                .Where(u => u.IsActive)
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<Employee> GetUserByNameAsync(string name)
        {
            return await _context.Employees
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == name);
        }
    }

}
