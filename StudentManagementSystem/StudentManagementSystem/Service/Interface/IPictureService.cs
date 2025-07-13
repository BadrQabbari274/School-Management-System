using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IPictureService
    {
        Task<IEnumerable<Picture>> GetAllPicturesAsync();
        Task<Picture> GetPictureByIdAsync(int id);
        Task<Picture> CreatePictureAsync(Picture picture);
        Task<Picture> UpdatePictureAsync(Picture picture);
        Task<bool> DeletePictureAsync(int id);
        Task<IEnumerable<Picture>> GetActivePicturesAsync();
        Task<IEnumerable<Picture>> GetPicturesByStudentAsync(int studentId);
        Task<IEnumerable<Picture>> GetPicturesByTaskAsync(int taskId);
        Task<string> SavePictureAsync(IFormFile file, int? studentId, int? taskId);
        Task<bool> DeletePictureFileAsync(string filePath);
    }
}
