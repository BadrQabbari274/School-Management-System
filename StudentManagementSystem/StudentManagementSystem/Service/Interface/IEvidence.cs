using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services.Interfaces
{
    public interface IEvidenceService
    {
        Task<IEnumerable<Evidence>> GetAllEvidencesAsync();
        Task<Evidence?> GetEvidenceByIdAsync(int id);
        Task<IEnumerable<Evidence>> GetEvidencesByOutcomeIdAsync(int outcomeId);
        Task<IEnumerable<Evidence>> GetActiveEvidencesAsync();
        Task<IEnumerable<Evidence>> GetPracticalEvidencesAsync();
        Task<Evidence> CreateEvidenceAsync(Evidence evidence);
        Task<Evidence?> UpdateEvidenceAsync(int id, Evidence evidence);
        Task<bool> DeleteEvidenceAsync(int id);
        Task<bool> EvidenceExistsAsync(int id);
        Task<bool> ToggleEvidenceStatusAsync(int id);
    }
}