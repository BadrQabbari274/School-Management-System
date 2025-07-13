using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IAcademicYearService
    {
        Task<IEnumerable<AcademicYear>> GetAllAcademicYearsAsync();
        Task<AcademicYear> GetAcademicYearByIdAsync(int id);
        Task<AcademicYear> CreateAcademicYearAsync(AcademicYear academicYear);
        Task<AcademicYear> UpdateAcademicYearAsync(AcademicYear academicYear);
        Task<bool> DeleteAcademicYearAsync(int id);
        Task<IEnumerable<AcademicYear>> GetActiveAcademicYearsAsync();
    }
}
