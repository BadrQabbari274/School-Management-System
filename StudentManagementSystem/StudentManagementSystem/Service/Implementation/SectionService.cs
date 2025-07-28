using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class SectionService : ISectionService
    {
        private readonly ApplicationDbContext _context;

        public SectionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Section>> GetAllSectionsAsync()
        {
            return await _context.Sections
                .Include(s => s.Department)
                .Include(s => s.CreatedBy)
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name_Of_Section)
                .ToListAsync();
        }

        public async Task<Section> GetSectionByIdAsync(int id)
        {
            return await _context.Sections
                .Include(s => s.Department)
                .Include(s => s.CreatedBy)
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
        }

        public async Task<Section> CreateSectionAsync(Section section)
        {
            section.Date = DateTime.Now;
            section.IsActive = true;

            _context.Sections.Add(section);
            await _context.SaveChangesAsync();

            // Load navigation properties
            await _context.Entry(section)
                .Reference(s => s.Department)
                .LoadAsync();
            await _context.Entry(section)
                .Reference(s => s.CreatedBy)
                .LoadAsync();

            return section;
        }

        public async Task<Section> UpdateSectionAsync(Section section)
        {
            var existingSection = await _context.Sections.FindAsync(section.Id);
            if (existingSection == null)
                return null;

            existingSection.Name_Of_Section = section.Name_Of_Section;
            existingSection.Department_Id = section.Department_Id;

            _context.Entry(existingSection).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Load navigation properties
            await _context.Entry(existingSection)
                .Reference(s => s.Department)
                .LoadAsync();
            await _context.Entry(existingSection)
                .Reference(s => s.CreatedBy)
                .LoadAsync();

            return existingSection;
        }

        public async Task<bool> DeleteSectionAsync(int id)
        {
            var section = await _context.Sections.FindAsync(id);
            if (section == null)
                return false;

            // Check if section is being used in Student_Class_Section_Year
            var isUsed = await _context.Student_Class_Section_Years
                .AnyAsync(scsy => scsy.Section_id == id);

            if (isUsed)
            {
                // Soft delete if used
                section.IsActive = false;
            }
            else
            {
                // Hard delete if not used
                _context.Sections.Remove(section);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Section>> GetActiveSectionsAsync()
        {
            return await _context.Sections
                .Include(s => s.Department)
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name_Of_Section)
                .ToListAsync();
        }

        public async Task<IEnumerable<Section>> GetSectionsByDepartmentAsync(int departmentId)
        {
            return await _context.Sections
                .Include(s => s.Department)
                .Include(s => s.CreatedBy)
                .Where(s => s.Department_Id == departmentId && s.IsActive)
                .OrderBy(s => s.Name_Of_Section)
                .ToListAsync();
        }

        public async Task<bool> SectionExistsAsync(int id)
        {
            return await _context.Sections
                .AnyAsync(s => s.Id == id && s.IsActive);
        }

        public async Task<bool> IsSectionNameUniqueAsync(string sectionName, int departmentId, int? excludeId = null)
        {
            var query = _context.Sections
                .Where(s => s.Name_Of_Section.ToLower() == sectionName.ToLower()
                         && s.Department_Id == departmentId
                         && s.IsActive);

            if (excludeId.HasValue)
            {
                query = query.Where(s => s.Id != excludeId.Value);
            }

            return !await query.AnyAsync();
        }
    }
}