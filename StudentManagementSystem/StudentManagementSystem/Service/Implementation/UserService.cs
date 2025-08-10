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

        public async Task<IEnumerable<Employees>> GetAllUsersAsync()
        {
            return await _context.Employees
                .Include(u => u.Role)
                .Include(u => u.CreatedBy)
                .ToListAsync();
        }

        public async Task<Employees> GetUserByIdAsync(int id)
        {
            return await _context.Employees
                .Include(u => u.Role)
                .Include(u => u.CreatedBy)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Employees> CreateUserAsync(Employees user)
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
        public async Task<Employees> UpdateUserAsync(Employees user)
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

        public async Task<IEnumerable<Employees>> GetActiveUsersAsync()
        {
            return await _context.Employees
                .Where(u => u.IsActive)
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<Employees> GetUserByNameAsync(string name)
        {
            return await _context.Employees.Include(e =>e.LastEditBy)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == name);
        }
    }

}
