using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IAbsenceReasonService
    {
        Task<IEnumerable<AbsenceReasons>> GetAllAbsenceReasonsAsync();
        Task<AbsenceReasons> GetAbsenceReasonByIdAsync(int id);
        Task<AbsenceReasons> CreateAbsenceReasonAsync(AbsenceReasons absenceReason);
        Task<AbsenceReasons> UpdateAbsenceReasonAsync(AbsenceReasons absenceReason);
        Task<bool> DeleteAbsenceReasonAsync(int id);
        Task<IEnumerable<AbsenceReasons>> GetActiveAbsenceReasonsAsync();
    }
}