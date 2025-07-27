using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IOutcomeService
    {
        Task<IEnumerable<Outcome>> GetAllOutcomesAsync();
        Task<Outcome> GetOutcomeByIdAsync(int id);
        Task<Outcome> CreateOutcomeAsync(Outcome outcome);
        Task<Outcome> UpdateOutcomeAsync(Outcome outcome);
        Task<bool> DeleteOutcomeAsync(int id);
        Task<IEnumerable<Outcome>> GetActiveOutcomesAsync();
        Task<IEnumerable<Outcome>> GetOutcomesByCompetenceAsync(int competenceId);
    }
}
