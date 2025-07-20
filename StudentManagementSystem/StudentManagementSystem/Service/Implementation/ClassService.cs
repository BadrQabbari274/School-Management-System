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

        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            return await _context.Classes
                .Include(c => c.Students)
                .Include(c => c.CreatedByUser)
                .Include(c => c.Field)
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<Class> GetClassByIdAsync(int id)
        {
            return await _context.Classes
                .Include(c => c.CreatedByUser)
                .Include(c => c.Field)
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<Class> CreateClassAsync(Class classEntity)
        {
            classEntity.Date = DateTime.Now;
            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync();
            return classEntity;
        }

        public async Task<Class> UpdateClassAsync(Class classEntity)
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

        public async Task<IEnumerable<Class>> GetActiveClassesAsync()
        {
            return await _context.Classes
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Class>> GetClassesByFieldAsync(int fieldId)
        {
            return await _context.Classes
                .Where(c => c.FieldId == fieldId && c.IsActive)
                .ToListAsync();
        }
    }

}
