using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    
    public class PictureService : IPictureService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _uploadPath;

        public PictureService(ApplicationDbContext context)
        {
            _context = context;
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Create directory if it doesn't exist
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<IEnumerable<Pictures>> GetAllPicturesAsync()
        {
            return await _context.Pictures
                .Include(p => p.Student)
                .Include(p => p.Task)
                .Include(p => p.CreatedBy)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Pictures> GetPictureByIdAsync(int id)
        {
            return await _context.Pictures
                .Include(p => p.Student)
                .Include(p => p.Task)
                .Include(p => p.CreatedBy)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<Pictures> CreatePictureAsync(Pictures picture)
        {
            picture.CreatedDate = DateTime.Now;
            _context.Pictures.Add(picture);
            await _context.SaveChangesAsync();
            return picture;
        }

        public async Task<Pictures> UpdatePictureAsync(Pictures picture)
        {
            _context.Entry(picture).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return picture;
        }

        public async Task<bool> DeletePictureAsync(int id)
        {
            var picture = await _context.Pictures.FindAsync(id);
            if (picture == null) return false;

            picture.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Pictures>> GetActivePicturesAsync()
        {
            return await _context.Pictures
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pictures>> GetPicturesByStudentAsync(int studentId)
        {
            return await _context.Pictures
                .Where(p => p.StudentId == studentId && !p.IsDeleted)
                .Include(p => p.Task)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pictures>> GetPicturesByTaskAsync(int taskId)
        {
            return await _context.Pictures
                .Where(p => p.TaskId == taskId && !p.IsDeleted)
                .Include(p => p.Student)
                .ToListAsync();
        }

        public async Task<string> SavePictureAsync(IFormFile file, int? studentId, int? taskId)
        {
            if (file == null || file.Length == 0)
                return null;

            // Create folder structure based on student/task
            string folderPath = _uploadPath;
            if (studentId.HasValue)
            {
                folderPath = Path.Combine(folderPath, "students", studentId.ToString());
            }
            if (taskId.HasValue)
            {
                folderPath = Path.Combine(folderPath, "tasks", taskId.ToString());
            }

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Generate unique filename
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(folderPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path for database storage
            return Path.GetRelativePath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), filePath);
        }

        public async Task<bool> DeletePictureFileAsync(string filePath)
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }

}
