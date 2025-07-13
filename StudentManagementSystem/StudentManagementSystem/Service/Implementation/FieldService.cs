using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class FieldService : IFieldService
    {
        private readonly ApplicationDbContext _context;

        public FieldService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Field>> GetAllFieldsAsync()
        {
            return await _context.Fields
                .Include(f => f.CreatedByUser)
                .Include(f => f.AcademicYear)
                .Where(f => f.IsActive)
                .ToListAsync();
        }

        public async Task<Field> GetFieldByIdAsync(int id)
        {
            return await _context.Fields
                .Include(f => f.CreatedByUser)
                .Include(f => f.AcademicYear)
                .Include(f => f.Classes)
                .Include(f => f.Competences)
                .FirstOrDefaultAsync(f => f.Id == id && f.IsActive);
        }

        public async Task<Field> CreateFieldAsync(Field field)
        {
            field.CreatedDate = DateTime.Now;
            _context.Fields.Add(field);
            await _context.SaveChangesAsync();
            return field;
        }

        public async Task<Field> UpdateFieldAsync(Field field)
        {
            _context.Entry(field).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return field;
        }

        public async Task<bool> DeleteFieldAsync(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null) return false;

            field.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Field>> GetActiveFieldsAsync()
        {
            return await _context.Fields
                .Where(f => f.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Field>> GetFieldsByAcademicYearAsync(int academicYearId)
        {
            return await _context.Fields
                .Where(f => f.AcademicYearId == academicYearId && f.IsActive)
                .ToListAsync();
        }

        public async Task<bool> AssignUserToFieldAsync(int userId, int fieldId)
        {
            var existingAssignment = await _context.FieldUsers
                .FirstOrDefaultAsync(fu => fu.UserId == userId && fu.FieldId == fieldId);

            if (existingAssignment != null) return false;

            var fieldUser = new FieldUser
            {
                UserId = userId,
                FieldId = fieldId
            };

            _context.FieldUsers.Add(fieldUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromFieldAsync(int userId, int fieldId)
        {
            var fieldUser = await _context.FieldUsers
                .FirstOrDefaultAsync(fu => fu.UserId == userId && fu.FieldId == fieldId);

            if (fieldUser == null) return false;

            _context.FieldUsers.Remove(fieldUser);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
