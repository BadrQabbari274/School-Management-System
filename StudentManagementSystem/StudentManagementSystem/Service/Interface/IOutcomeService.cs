using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IOutcomeService
    {
        Task<IEnumerable<Outcomes>> GetAllOutcomesAsync();
        Task<Outcomes> GetOutcomeByIdAsync(int id);
        Task<Outcomes> CreateOutcomeAsync(Outcomes outcome);
        Task<Outcomes> UpdateOutcomeAsync(Outcomes outcome);
        Task<bool> DeleteOutcomeAsync(int id);
        Task<IEnumerable<Outcomes>> GetActiveOutcomesAsync();
        Task<IEnumerable<Outcomes>> GetOutcomesByCompetenceAsync(int competenceId);
    }
}
