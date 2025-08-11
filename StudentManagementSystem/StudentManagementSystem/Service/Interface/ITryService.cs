using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Services.Interfaces
{
    public interface ITryService
    {
        Task<TryIndexViewModel> GetAllTriesAsync(int pageNumber = 1, int pageSize = 10,
            string searchTerm = null, bool? isActiveFilter = null);
        Task<TryViewModel> GetTryByIdAsync(int id);
        Task<TryDetailsViewModel> GetTryDetailsAsync(int id);
        Task<TryViewModel> CreateTryAsync(TryViewModel model, int currentUserId);
        Task<TryViewModel> UpdateTryAsync(TryViewModel model, int currentUserId);
        Task<bool> DeleteTryAsync(int id);
        Task<bool> TryExistsAsync(int id);
        Task<bool> IsTryNameUniqueAsync(string name, int? excludeId = null);
        Task LoadSelectListsAsync(TryViewModel model);
        Task<List<Employees>> GetActiveEmployeesAsync();
        Task<int> GetStudentEvidencesCountAsync(int tryId);
    }
}