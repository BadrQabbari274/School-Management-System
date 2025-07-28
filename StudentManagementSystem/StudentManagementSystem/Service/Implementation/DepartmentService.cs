using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments
                .Include(d => d.CreatedBy)
                .Where(d => d.IsActive)
                .ToListAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.CreatedBy)

                .Include(d => d.Competences)
                .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);
        }

        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            department.CreatedDate = DateTime.Now;
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department> UpdateDepartmentAsync(Department department)
        {
            _context.Entry(department).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return false;

            department.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Department>> GetActiveDepartmentsAsync()
        {
            return await _context.Departments
                .Where(d => d.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetDepartmentsByAcademicYearAsync(int academicYearId)
        {
            return await _context.Departments
                .Where(d => d.GradeId == academicYearId && d.IsActive)
                .ToListAsync();
        }

        public async Task<bool> AssignUserToDepartmentAsync(int userId, int departmentId)
        {
            var existingAssignment = await _context.Employee_Departments
                .FirstOrDefaultAsync(de => de.UserId == userId && de.Department_Id == departmentId);

            if (existingAssignment != null) return false;

            var departmentEmployee = new Employee_Department
            {
                UserId = userId,
                Department_Id = departmentId
            };

            _context.Employee_Departments.Add(departmentEmployee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromDepartmentAsync(int userId, int departmentId)
        {
            var departmentEmployee = await _context.Employee_Departments
                .FirstOrDefaultAsync(de => de.UserId == userId && de.Department_Id == departmentId);

            if (departmentEmployee == null) return false;

            _context.Employee_Departments.Remove(departmentEmployee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
