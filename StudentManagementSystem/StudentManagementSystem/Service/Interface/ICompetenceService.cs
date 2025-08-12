using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Services.Interfaces
{
    public interface ICompetenciesService
    {
        Task<CompetenciesIndexViewModel> GetAllCompetenciesAsync(int pageNumber = 1, int pageSize = 10,
            string searchTerm = null, int? sectionFilter = null, bool? isActiveFilter = null);

        Task<CompetenciesViewModel> GetCompetencyByIdAsync(int id);

        Task<CompetenciesDetailsViewModel> GetCompetencyDetailsAsync(int id);

        Task<CompetenciesViewModel> CreateCompetencyAsync(CompetenciesViewModel model, int currentUserId);

        Task<CompetenciesViewModel> UpdateCompetencyAsync(CompetenciesViewModel model, int currentUserId);

        Task<bool> DeleteCompetencyAsync(int id);

        Task<bool> CompetencyExistsAsync(int id);

        Task<bool> IsCompetencyNameUniqueAsync(string name, int? excludeId = null);

        Task LoadSelectListsAsync(CompetenciesViewModel model);

        Task<List<Section>> GetActiveSectionsAsync();

        Task<List<Employees>> GetActiveEmployeesAsync();

        Task<int> GetLearningOutcomesCountAsync(int competencyId);
    }
}