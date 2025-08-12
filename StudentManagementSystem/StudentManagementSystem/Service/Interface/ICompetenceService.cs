using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Services.Interfaces
{
    public interface ICompetenciesService
    {
        Task<Competencies_Outcame_Evidence> GetCompetencies_Outcame_Evidence(int ClassId);
        // إضافة هذه الدالة في ICompetenciesService Interface

        Task<EvaluationMatrixViewModel> GetEvaluationMatrixAsync(int classId, int competencyId, int tryId);
        Task<List<Evidence>> GetPracticalEvidencesByCompetencyId(int competencyId);
        Task<bool> IsStudentEvidenceExistsAsync(int studentId, int evidenceId, int tryId);
        Task<Competencies_Outcame_Evidence_V2> GetCompetencies_Outcame_Evidence_V2(int ClassId);
        Task<List<Evidence>> GetEvidencesByOutcomeId(int outcomeId);
        Task<List<Try>> GetAllTriesAsync();
        Task<List<StudentEvidenceViewModel>> GetStudentToEvidences(CompetenciesSelectionViewModel model);
        Task<List<Learning_Outcome>> GetLearningOutcomesByCompetencyId(int competencyId);
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