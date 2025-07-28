using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service
{
    public interface IWorkingYearService
    {
        Task<IEnumerable<Working_Year>> GetAllWorkingYearsAsync();
        Task<Working_Year?> GetWorkingYearByIdAsync(int id);
        Task<Working_Year> CreateWorkingYearAsync(Working_Year workingYear);
        Task<Working_Year> UpdateWorkingYearAsync(Working_Year workingYear);
        Task<bool> DeleteWorkingYearAsync(int id);
        Task<bool> WorkingYearExistsAsync(int id);
        Task<bool> IsWorkingYearNameUniqueAsync(string name, int? excludeId = null);
    }
}