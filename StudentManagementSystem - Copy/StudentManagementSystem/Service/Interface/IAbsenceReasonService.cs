using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IAbsenceReasonService
    {
        Task<IEnumerable<AbsenceReason>> GetAllAbsenceReasonsAsync();
        Task<AbsenceReason> GetAbsenceReasonByIdAsync(int id);
        Task<AbsenceReason> CreateAbsenceReasonAsync(AbsenceReason absenceReason);
        Task<AbsenceReason> UpdateAbsenceReasonAsync(AbsenceReason absenceReason);
        Task<bool> DeleteAbsenceReasonAsync(int id);
        Task<IEnumerable<AbsenceReason>> GetActiveAbsenceReasonsAsync();
    }
}