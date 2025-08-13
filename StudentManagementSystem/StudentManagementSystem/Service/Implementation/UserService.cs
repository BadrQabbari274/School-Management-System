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
            return await _context.Employees.Include(e => e.LastEditBy)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == name);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeTypes>> GetAllEmployeeTypesAsync()
        {
            return await _context.EmployeeTypes
                .Where(et => !et.IsDeleted)
                .OrderBy(et => et.Name)
                .ToListAsync();
        }

        public async Task<EmployeeTypes> GetEmployeeTypeByIdAsync(int id)
        {
            return await _context.EmployeeTypes
                .Include(et => et.Employees)
                .FirstOrDefaultAsync(et => et.Id == id && !et.IsDeleted);
        }

        public async Task<EmployeeTypes> CreateEmployeeTypeAsync(EmployeeTypes employeeType)
        {
            var existingEmployeeType = await _context.EmployeeTypes
                .FirstOrDefaultAsync(et => et.Name == employeeType.Name && !et.IsDeleted);

            if (existingEmployeeType != null)
            {
                throw new InvalidOperationException("لا يمكن إنشاء نوع موظف بنفس الاسم لأنه موجود بالفعل.");
            }

            employeeType.CreatedDate = DateTime.Now;
            employeeType.IsDeleted = false;

            _context.EmployeeTypes.Add(employeeType);
            await _context.SaveChangesAsync();

            return employeeType;
        }

        public async Task<EmployeeTypes> UpdateEmployeeTypeAsync(EmployeeTypes employeeType)
        {
            var existingEmployeeType = await _context.EmployeeTypes
                .FirstOrDefaultAsync(et => et.Id == employeeType.Id && !et.IsDeleted);

            if (existingEmployeeType != null)
            {
                var duplicateName = await _context.EmployeeTypes
                    .AnyAsync(et => et.Name == employeeType.Name && et.Id != employeeType.Id && !et.IsDeleted);

                if (duplicateName)
                {
                    throw new InvalidOperationException("لا يمكن تحديث نوع الموظف، يوجد نوع آخر بنفس الاسم بالفعل.");
                }

                existingEmployeeType.Name = employeeType.Name;


                await _context.SaveChangesAsync();
                return existingEmployeeType;
            }

            return null;
        }

        public async Task<bool> DeleteEmployeeTypeAsync(int id)
        {
            var employeeType = await _context.EmployeeTypes
                .FirstOrDefaultAsync(et => et.Id == id && !et.IsDeleted);

            if (employeeType != null)
            {
                var hasEmployees = await _context.Employees
                    .AnyAsync(e => e.RoleId == id && e.IsActive);

                if (hasEmployees)
                {
                    return false; 
                }

                employeeType.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<EmployeeTypes>> GetActiveEmployeeTypesAsync()
        {
            return await _context.EmployeeTypes
                .Include(et => et.Employees)
                .Where(et => !et.IsDeleted)
                .OrderBy(et => et.Name)
                .ToListAsync();
        }

        public async Task<bool> EmployeeTypeExistsAsync(int id)
        {
            return await _context.EmployeeTypes
                .AnyAsync(et => et.Id == id && !et.IsDeleted);
        }
    }
}
