using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IPictureService
    {
        Task<IEnumerable<Pictures>> GetAllPicturesAsync();
        Task<Pictures> GetPictureByIdAsync(int id);
        Task<Pictures> CreatePictureAsync(Pictures picture);
        Task<Pictures> UpdatePictureAsync(Pictures picture);
        Task<bool> DeletePictureAsync(int id);
        Task<IEnumerable<Pictures>> GetActivePicturesAsync();
        Task<IEnumerable<Pictures>> GetPicturesByStudentAsync(int studentId);
        Task<IEnumerable<Pictures>> GetPicturesByTaskAsync(int taskId);
        Task<string> SavePictureAsync(IFormFile file, int? studentId, int? taskId);
        Task<bool> DeletePictureFileAsync(string filePath);
    }
}
