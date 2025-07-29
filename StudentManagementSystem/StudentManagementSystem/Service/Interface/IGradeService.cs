using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IGradeService
    {
        Task<IEnumerable<Grades>> GetAllAcademicYearsAsync();
        Task<Grades> GetAcademicYearByIdAsync(int id);
        Task<Grades> GetAcademicYearByNameAsync(string name);
        Task<Grades> CreateAcademicYearAsync(Grades academicYear);
        Task<Grades> UpdateAcademicYearAsync(Grades academicYear);
        Task<bool> DeleteAcademicYearAsync(int id);
        Task<IEnumerable<Grades>> GetActiveAcademicYearsAsync();
    }
}
