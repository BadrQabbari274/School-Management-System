using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class ClassService : IClassService
    {
        private readonly ApplicationDbContext _context;

        public ClassService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Classes>> GetAllClassesAsync()
        {
            return await _context.Classes
                .Include(c => c.Students)
                .Include(c => c.CreatedBy)
           
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        public async Task<Classes> GetClassByIdAsync(int id)
        {
            return await _context.Classes
                .Include(c => c.CreatedBy)
              
                .Include(c => c.MaxStudents)
                .Include(c => c.Students.OrderBy(s => s.Name))
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<Classes> CreateClassAsync(Classes classEntity)
        {
            classEntity.Date = DateTime.Now;
            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync();
            return classEntity;
        }

        public async Task<Classes> UpdateClassAsync(Classes classEntity)
        {
            _context.Entry(classEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return classEntity;
        }

        public async Task<bool> DeleteClassAsync(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null) return false;

            classEntity.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Classes>> GetActiveClassesAsync()
        {
            return await _context.Classes
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Classes>> GetClassesByFieldAsync(int fieldId)
        {
            return await _context.Classes
                .Where(c =>  c.IsActive)
                .ToListAsync();
        }
    }

}
