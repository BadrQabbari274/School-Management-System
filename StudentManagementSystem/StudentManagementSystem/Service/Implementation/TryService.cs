using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services.Interfaces;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Services
{
    public class TryService : ITryService
    {
        private readonly ApplicationDbContext _context;

        public TryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TryIndexViewModel> GetAllTriesAsync(int pageNumber = 1, int pageSize = 10,
            string searchTerm = null, bool? isActiveFilter = null)
        {
            var query = _context.Try
                .Include(t => t.CreatedBy)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.Name.Contains(searchTerm));
            }

            if (isActiveFilter.HasValue)
            {
                query = query.Where(t => t.IsActive == isActiveFilter.Value);
            }

            var totalCount = await query.CountAsync();

            var Try = await query
                .OrderBy(t => t.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TryViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    IsActive = t.IsActive,
                    CreatedBy_Id = t.CreatedBy_Id,
                    CreatedDate = t.CreatedDate,
                    CreatedByName = t.CreatedBy.Name
                })
                .ToListAsync();

            return new TryIndexViewModel
            {
                Tries = Try,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                IsActiveFilter = isActiveFilter
            };
        }

        public async Task<TryViewModel> GetTryByIdAsync(int id)
        {
            var tryItem = await _context.Try
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tryItem == null)
                return null;

            return new TryViewModel
            {
                Id = tryItem.Id,
                Name = tryItem.Name,
                IsActive = tryItem.IsActive,
                CreatedBy_Id = tryItem.CreatedBy_Id,
                CreatedDate = tryItem.CreatedDate,
                CreatedByName = tryItem.CreatedBy?.Name
            };
        }

        public async Task<TryDetailsViewModel> GetTryDetailsAsync(int id)
        {
            var tryItem = await GetTryByIdAsync(id);
            if (tryItem == null)
                return null;

            var studentEvidencesCount = await _context.Student_Tasks
                .Where(se => se.Try_Id == id)
                .CountAsync();

            return new TryDetailsViewModel
            {
                Try = tryItem,
                StudentEvidencesCount = studentEvidencesCount
            };
        }

        public async Task<TryViewModel> CreateTryAsync(TryViewModel model, int currentUserId)
        {
            var tryItem = new Try
            {
                Name = model.Name,
                IsActive = model.IsActive,
                CreatedBy_Id = currentUserId,
                CreatedDate = DateTime.Now
            };

            _context.Try.Add(tryItem);
            await _context.SaveChangesAsync();

            model.Id = tryItem.Id;
            model.CreatedBy_Id = currentUserId;
            model.CreatedDate = tryItem.CreatedDate;

            return model;
        }

        public async Task<TryViewModel> UpdateTryAsync(TryViewModel model, int currentUserId)
        {
            var tryItem = await _context.Try.FindAsync(model.Id);
            if (tryItem == null)
                return null;

            tryItem.Name = model.Name;
            tryItem.IsActive = model.IsActive;

            _context.Try.Update(tryItem);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<bool> DeleteTryAsync(int id)
        {
            var tryItem = await _context.Try.FindAsync(id);
            if (tryItem == null)
                return false;

            // Check if there are any student evidences associated
            var hasStudentEvidences = await _context.Student_Tasks
                .AnyAsync(se => se.Try_Id == id);

            if (hasStudentEvidences)
            {
                // Soft delete - just mark as inactive
                tryItem.IsActive = false;
                _context.Try.Update(tryItem);
            }
            else
            {
                // Hard delete if no dependencies
                _context.Try.Remove(tryItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TryExistsAsync(int id)
        {
            return await _context.Try.AnyAsync(t => t.Id == id);
        }

        public async Task<bool> IsTryNameUniqueAsync(string name, int? excludeId = null)
        {
            var query = _context.Try.Where(t => t.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(t => t.Id != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        public async Task LoadSelectListsAsync(TryViewModel model)
        {
            // Load Employees
            var employees = await GetActiveEmployeesAsync();
            model.Employees = employees.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Name
            }).ToList();
        }

        public async Task<List<Employees>> GetActiveEmployeesAsync()
        {
            return await _context.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<int> GetStudentEvidencesCountAsync(int tryId)
        {
            return await _context.Student_Tasks
                .Where(se => se.Try_Id == tryId)
                .CountAsync();
        }
    }
}