using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IAbsenceReasonsService
    {
        Task<IEnumerable<AbsenceReasons>> GetAllAsync();
        Task<AbsenceReasons?> GetByIdAsync(int id);
        Task<bool> CreateAsync(AbsenceReasons absenceReason);
        Task<bool> UpdateAsync(AbsenceReasons absenceReason);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<AbsenceReasons>> SearchAsync(string searchTerm);
        Task<bool> ExistsAsync(int id);
        Task<int> GetUsageCountAsync(int id);
        Task<IEnumerable<AbsenceReasons>> GetActiveAsync();
    }
}