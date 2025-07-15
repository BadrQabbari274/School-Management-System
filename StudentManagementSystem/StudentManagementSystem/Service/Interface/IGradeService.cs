using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IGradeService
    {
        Task<IEnumerable<Grade>> GetAllAcademicYearsAsync();
        Task<Grade> GetAcademicYearByIdAsync(int id);
        Task<Grade> CreateAcademicYearAsync(Grade academicYear);
        Task<Grade> UpdateAcademicYearAsync(Grade academicYear);
        Task<bool> DeleteAcademicYearAsync(int id);
        Task<IEnumerable<Grade>> GetActiveAcademicYearsAsync();
    }
}
