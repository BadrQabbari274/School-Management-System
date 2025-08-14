using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class AbsenceReasonsService : IAbsenceReasonsService2
    {
        private readonly ApplicationDbContext _context;

        public AbsenceReasonsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AbsenceReasons>> GetAllAsync()
        {
            try
            {
                return await _context.AbsenceReasons
                    .Where(ar => !ar.IsDeleted)
                    .Include(ar => ar.CreatedBy)
                    .Include(ar => ar.StudentAbsents)
                    .OrderByDescending(ar => ar.CreatedDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new Exception("حدث خطأ أثناء جلب أسباب الغياب", ex);
            }
        }

        public async Task<AbsenceReasons?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.AbsenceReasons
                    .Where(ar => ar.Id == id && !ar.IsDeleted)
                    .Include(ar => ar.CreatedBy)
                    .Include(ar => ar.StudentAbsents)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new Exception("حدث خطأ أثناء جلب بيانات سبب الغياب", ex);
            }
        }

        public async Task<bool> CreateAsync(AbsenceReasons absenceReason)
        {
            try
            {
                if (absenceReason == null)
                    return false;

                // Check if name already exists
                var existingReason = await _context.AbsenceReasons
                    .Where(ar => ar.Name.Trim().ToLower() == absenceReason.Name.Trim().ToLower() && !ar.IsDeleted)
                    .FirstOrDefaultAsync();

                if (existingReason != null)
                    throw new InvalidOperationException("سبب الغياب موجود بالفعل");

                absenceReason.Name = absenceReason.Name.Trim();
                absenceReason.CreatedDate = DateTime.Now;
                absenceReason.IsDeleted = false;

                _context.AbsenceReasons.Add(absenceReason);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new Exception("حدث خطأ أثناء إضافة سبب الغياب", ex);
            }
        }

        public async Task<bool> UpdateAsync(AbsenceReasons absenceReason)
        {
            try
            {
                if (absenceReason == null)
                    return false;

                var existingReason = await _context.AbsenceReasons
                    .Where(ar => ar.Id == absenceReason.Id && !ar.IsDeleted)
                    .FirstOrDefaultAsync();

                if (existingReason == null)
                    return false;

                // Check if name already exists for other records
                var duplicateName = await _context.AbsenceReasons
                    .Where(ar => ar.Name.Trim().ToLower() == absenceReason.Name.Trim().ToLower()
                                && ar.Id != absenceReason.Id
                                && !ar.IsDeleted)
                    .FirstOrDefaultAsync();

                if (duplicateName != null)
                    throw new InvalidOperationException("سبب الغياب موجود بالفعل");

                existingReason.Name = absenceReason.Name.Trim();

                _context.AbsenceReasons.Update(existingReason);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new Exception("حدث خطأ أثناء تحديث سبب الغياب", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var absenceReason = await _context.AbsenceReasons
                    .Include(ar => ar.StudentAbsents)
                    .Where(ar => ar.Id == id && !ar.IsDeleted)
                    .FirstOrDefaultAsync();

                if (absenceReason == null)
                    return false;

                // Check if the absence reason is being used
                if (absenceReason.StudentAbsents != null && absenceReason.StudentAbsents.Any())
                {
                    throw new InvalidOperationException("لا يمكن حذف سبب الغياب لأنه مُستخدم في سجلات الغياب");
                }

                // Soft delete
                absenceReason.IsDeleted = true;

                _context.AbsenceReasons.Update(absenceReason);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new Exception("حدث خطأ أثناء حذف سبب الغياب", ex);
            }
        }

        public async Task<IEnumerable<AbsenceReasons>> SearchAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return await GetAllAsync();

                searchTerm = searchTerm.Trim().ToLower();

                return await _context.AbsenceReasons
                    .Where(ar => !ar.IsDeleted &&
                                (ar.Name.ToLower().Contains(searchTerm) ||
                                 ar.CreatedBy.Name.ToLower().Contains(searchTerm)))
                    .Include(ar => ar.CreatedBy)
                    .Include(ar => ar.StudentAbsents)
                    .OrderByDescending(ar => ar.CreatedDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new Exception("حدث خطأ أثناء البحث في أسباب الغياب", ex);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.AbsenceReasons
                    .AnyAsync(ar => ar.Id == id && !ar.IsDeleted);
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new Exception("حدث خطأ أثناء التحقق من وجود سبب الغياب", ex);
            }
        }

        public async Task<int> GetUsageCountAsync(int id)
        {
            try
            {
                var absenceReason = await _context.AbsenceReasons
                    .Include(ar => ar.StudentAbsents)
                    .Where(ar => ar.Id == id && !ar.IsDeleted)
                    .FirstOrDefaultAsync();

                return absenceReason?.StudentAbsents?.Count() ?? 0;
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new Exception("حدث خطأ أثناء حساب عدد الاستخدامات", ex);
            }
        }

        public async Task<IEnumerable<AbsenceReasons>> GetActiveAsync()
        {
            try
            {
                return await _context.AbsenceReasons
                    .Where(ar => !ar.IsDeleted)
                    .OrderBy(ar => ar.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new Exception("حدث خطأ أثناء جلب أسباب الغياب النشطة", ex);
            }
        }
    }
}