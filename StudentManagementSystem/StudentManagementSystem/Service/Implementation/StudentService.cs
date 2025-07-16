using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.CreatedByUser)
                .Include(s => s.Class)
                .ThenInclude(c => c.Field)
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.CreatedByUser)
                .Include(s => s.Class)
                .ThenInclude(c => c.Field)
                .Include(s => s.TaskEvaluations)
                .Include(s => s.Pictures)
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            student.Date = DateTime.Now;
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            student.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Student>> GetActiveStudentsAsync()
        {
            return await _context.Students
                .Where(s => s.IsActive)
                .Include(s => s.Class)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsByClassAsync(int classId)
        {
            return await _context.Students
                .Where(s => s.ClassId == classId && s.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsWithTasksAsync()
        {
            return await _context.Students
                .Include(s => s.TaskEvaluations)
                .Where(s => s.IsActive && s.TaskEvaluations.Any())
                .ToListAsync();
        }
    }
}
