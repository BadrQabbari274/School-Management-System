using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface ISectionService
    {
        Task<IEnumerable<Section>> GetAllSectionsAsync();
        Task<Section> GetSectionByIdAsync(int id);
        Task<Section> CreateSectionAsync(Section section);
        Task<Section> UpdateSectionAsync(Section section);
        Task<bool> DeleteSectionAsync(int id);
        Task<IEnumerable<Section>> GetActiveSectionsAsync();
        Task<IEnumerable<Section>> GetSectionsByDepartmentAsync(int departmentId);
        Task<bool> SectionExistsAsync(int id);
        Task<bool> IsSectionNameUniqueAsync(string sectionName, int departmentId, int? excludeId = null);
    }
}