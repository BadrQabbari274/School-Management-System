using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services.Interfaces;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Services
{
    public class CompetenciesService : ICompetenciesService
    {
        private readonly ApplicationDbContext _context;

        public CompetenciesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CompetenciesIndexViewModel> GetAllCompetenciesAsync(int pageNumber = 1, int pageSize = 10,
            string searchTerm = null, int? sectionFilter = null, bool? isActiveFilter = null)
        {
            var query = _context.Competencies
                .Include(c => c.Section)
                .Include(c => c.CreatedBy)
                .Include(c => c.Learning_Outcomes)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm));
            }

            if (sectionFilter.HasValue)
            {
                query = query.Where(c => c.Section_Id == sectionFilter.Value);
            }

            if (isActiveFilter.HasValue)
            {
                query = query.Where(c => c.IsActive == isActiveFilter.Value);
            }

            var totalCount = await query.CountAsync();

            var competencies = await query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CompetenciesViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Duration = c.Duration,
                    Section_Id = c.Section_Id,
                    Max_Outcome = c.Max_Outcome,
                    IsActive = c.IsActive,
                    CreatedBy_Id = c.CreatedBy_Id,
                    CreatedDate = c.CreatedDate,
                    SectionName = c.Section.Name_Of_Section,
                    CreatedByName = c.CreatedBy.Name
                })
                .ToListAsync();

            return new CompetenciesIndexViewModel
            {
                Competencies = competencies,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                SectionFilter = sectionFilter,
                IsActiveFilter = isActiveFilter
            };
        }

        public async Task<CompetenciesViewModel> GetCompetencyByIdAsync(int id)
        {
            var competency = await _context.Competencies
                .Include(c => c.Section)
                .Include(c => c.CreatedBy)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (competency == null)
                return null;

            return new CompetenciesViewModel
            {
                Id = competency.Id,
                Name = competency.Name,
                Duration = competency.Duration,
                Section_Id = competency.Section_Id,
                Max_Outcome = competency.Max_Outcome,
                IsActive = competency.IsActive,
                CreatedBy_Id = competency.CreatedBy_Id,
                CreatedDate = competency.CreatedDate,
                SectionName = competency.Section?.Name_Of_Section,
                CreatedByName = competency.CreatedBy?.Name
            };
        }

        public async Task<CompetenciesDetailsViewModel> GetCompetencyDetailsAsync(int id)
        {
            var competency = await GetCompetencyByIdAsync(id);
            if (competency == null)
                return null;

            var learningOutcomes = await _context.Outcomes
                .Where(lo => lo.Competency_Id == id)
                .Include(lo => lo.CreatedBy)
                .Select(lo => new LearningOutcomeViewModel
                {
                    Id = lo.Id,
                    Name = lo.Name,
                    IsActive = lo.IsActive,
                    CreatedByName = lo.CreatedBy.Name,
                    CreatedDate = lo.CreatedDate
                })
                .OrderBy(lo => lo.Name)
                .ToListAsync();

            return new CompetenciesDetailsViewModel
            {
                Competency = competency,
                LearningOutcomes = learningOutcomes
            };
        }

        public async Task<CompetenciesViewModel> CreateCompetencyAsync(CompetenciesViewModel model, int currentUserId)
        {
            var competency = new Competencies
            {
                Name = model.Name,
                Duration = model.Duration,
                Section_Id = model.Section_Id,
                Max_Outcome = model.Max_Outcome,
                IsActive = model.IsActive,
                CreatedBy_Id = currentUserId,
                CreatedDate = DateTime.Now
            };

            _context.Competencies.Add(competency);
            await _context.SaveChangesAsync();

            model.Id = competency.Id;
            model.CreatedBy_Id = currentUserId;
            model.CreatedDate = competency.CreatedDate;

            return model;
        }

        public async Task<CompetenciesViewModel> UpdateCompetencyAsync(CompetenciesViewModel model, int currentUserId)
        {
            var competency = await _context.Competencies.FindAsync(model.Id);
            if (competency == null)
                return null;

            competency.Name = model.Name;
            competency.Duration = model.Duration;
            competency.Section_Id = model.Section_Id;
            competency.Max_Outcome = model.Max_Outcome;
            competency.IsActive = model.IsActive;

            _context.Competencies.Update(competency);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<bool> DeleteCompetencyAsync(int id)
        {
            var competency = await _context.Competencies.FindAsync(id);
            if (competency == null)
                return false;

            // Check if there are any learning outcomes associated
            var hasLearningOutcomes = await _context.Outcomes
                .AnyAsync(lo => lo.Competency_Id == id);

            if (hasLearningOutcomes)
            {
                // Soft delete - just mark as inactive
                competency.IsActive = false;
                _context.Competencies.Update(competency);
            }
            else
            {
                // Hard delete if no dependencies
                _context.Competencies.Remove(competency);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompetencyExistsAsync(int id)
        {
            return await _context.Competencies.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> IsCompetencyNameUniqueAsync(string name, int? excludeId = null)
        {
            var query = _context.Competencies.Where(c => c.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        public async Task LoadSelectListsAsync(CompetenciesViewModel model)
        {
            // Load Sections
            var sections = await GetActiveSectionsAsync();
            model.Sections = sections.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name_Of_Section
            }).ToList();

            // Load Employees
            var employees = await GetActiveEmployeesAsync();
            model.Employees = employees.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Name
            }).ToList();
        }

        public async Task<List<Section>> GetActiveSectionsAsync()
        {
            return await _context.Sections
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name_Of_Section)
                .ToListAsync();
        }

        public async Task<List<Employees>> GetActiveEmployeesAsync()
        {
            return await _context.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<int> GetLearningOutcomesCountAsync(int competencyId)
        {
            return await _context.Outcomes
                .Where(lo => lo.Competency_Id == competencyId)
                .CountAsync();
        }
    }
}