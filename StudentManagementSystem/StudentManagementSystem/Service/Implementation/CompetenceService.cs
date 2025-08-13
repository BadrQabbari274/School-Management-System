using DocumentFormat.OpenXml.VariantTypes;
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
        // إضافة هذه الدوال في CompetenciesService

        public async Task<EvaluationMatrixViewModel> GetEvaluationMatrixAsync(int classId, int competencyId, int tryId)
        {
            var activeWorkingYear = await GetOrCreateActiveWorkingYearAsync();

            // الحصول على معلومات الفصل
            var classInfo = await _context.Classes.FirstOrDefaultAsync(c => c.Id == classId);
            if (classInfo == null)
                return null;

            // الحصول على الطلاب في الفصل
            var studentsInClass = await _context.Student_Class_Section_Years
                .Include(scsy => scsy.Student)
                .Where(scsy => scsy.Class_Id == classId &&
                              scsy.Working_Year_Id == activeWorkingYear.Id &&
                              scsy.IsActive &&
                              scsy.Student.IsActive)
                .OrderBy(scsy => scsy.Student.Natrual_Id.Length == 14 ?
                         (scsy.Student.Natrual_Id.Substring(12, 1) == "1" || scsy.Student.Natrual_Id.Substring(12, 1) == "3" ? 0 : 1) : 0)
                .ThenBy(scsy => scsy.Student.Name)
                .ToListAsync();

            // الحصول على الأدلة العملية للكفاية
            var practicalEvidences = await GetPracticalEvidencesByCompetencyId(competencyId);

            // إنشاء مصفوفة التقييم
            var evaluationMatrix = new List<StudentEvaluationRowViewModel>();

            foreach (var studentClass in studentsInClass)
            {
                var studentRow = new StudentEvaluationRowViewModel
                {
                    Student = studentClass.Student,
                    EvidenceStatuses = new List<EvidenceStatusViewModel>()
                };

                foreach (var evidence in practicalEvidences)
                {
                    var isEvaluated = await IsStudentEvidenceExistsAsync(studentClass.Student_Id, evidence.Id, tryId);

                    studentRow.EvidenceStatuses.Add(new EvidenceStatusViewModel
                    {
                        EvidenceId = evidence.Id,
                        EvidenceName = evidence.Name,
                        IsEvaluated = isEvaluated,
                        StudentId = studentClass.Student_Id,
                        TryId = tryId
                    });
                }

                evaluationMatrix.Add(studentRow);
            }

            return new EvaluationMatrixViewModel
            {
                ClassId = classId,
                CompetencyId = competencyId,
                TryId = tryId,
                ClassName = classInfo.Name,
                PracticalEvidences = practicalEvidences,
                StudentEvaluationRows = evaluationMatrix
            };
        }

        public async Task<List<Evidence>> GetPracticalEvidencesByCompetencyId(int competencyId)
        {
            return await _context.Evidences
                .Include(e => e.Learning_Outcome)
                .Where(e => e.Learning_Outcome.Competency_Id == competencyId &&
                           e.IsActive &&
                           e.Ispractical)
                .OrderBy(e => e.Number)
                .ThenBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<bool> IsStudentEvidenceExistsAsync(int studentId, int evidenceId, int tryId)
        {
            return await _context.Student_Tasks
                .AnyAsync(se => se.Student_Id == studentId &&
                               se.Task_Id == evidenceId &&
                               se.Try_Id == tryId &&
                               se.IsActive);
        }
        public async Task<Working_Year> GetOrCreateActiveWorkingYearAsync()
        {
            // البحث عن سنة دراسية نشطة
            var activeWorkingYear = await _context.Working_Years
                .Where(wy => wy.IsActive)
                .OrderByDescending(wy => wy.Start_date)
                .FirstOrDefaultAsync();

            // إذا لم توجد، إنشاء واحدة جديدة
            if (activeWorkingYear == null)
            {
                var user = _context.Employees.Include(i => i.Role).FirstOrDefault(e => e.IsActive && e.Role.Name == "Admin");

                var currentYear = DateTime.Now.Year;
                activeWorkingYear = new Working_Year
                {
                    Name = $"العام الدراسي {currentYear}-{currentYear + 1}",
                    Start_date = new DateTime(currentYear, 9, 1), // بداية سبتمبر
                    End_date = new DateTime(currentYear + 1, 6, 30), // نهاية يونيو
                    IsActive = true,
                    CreatedBy_Id = user.Id, // استخدم ID المستخدم الحالي
                    Date = DateTime.Now
                };

                _context.Working_Years.Add(activeWorkingYear);
                await _context.SaveChangesAsync();
            }

            return activeWorkingYear;
        }
        public async Task<Competencies_Outcame_Evidence> GetCompetencies_Outcame_Evidence(int ClassId)
        {
            var workingyear = await GetOrCreateActiveWorkingYearAsync();
           var student=await _context.Student_Class_Section_Years.FirstOrDefaultAsync(s => s.Class_Id == ClassId && s.Working_Year_Id == workingyear.Id&& s.IsActive);
            var section = await _context.Sections.FirstOrDefaultAsync(a => a.Id == student.Section_id&& a.IsActive);
            var Competencies = await _context.Competencies.Where(w => w.Section_Id == section.Id&&w.IsActive).ToListAsync();
            var Outcome = await _context.Outcomes.Where(o => o.IsActive).ToListAsync();
            var Evidences = await _context.Evidences.Where(o => o.IsActive&&o.Ispractical).ToListAsync();
            var Competencies_Outcame_Evidence = new Competencies_Outcame_Evidence() 
            { Competencies = Competencies, 
                LearningOutcomes = Outcome,
                evidences = Evidences };
            return Competencies_Outcame_Evidence;
        }
        public async Task<Competencies_Outcame_Evidence_V2> GetCompetencies_Outcame_Evidence_V2(int ClassId)
        {
            var workingyear = await GetOrCreateActiveWorkingYearAsync();
            var student = await _context.Student_Class_Section_Years.FirstOrDefaultAsync(s => s.Class_Id == ClassId && s.Working_Year_Id == workingyear.Id && s.IsActive);
            var section = await _context.Sections.FirstOrDefaultAsync(a => a.Id == student.Section_id && a.IsActive);
            var Competencies = await _context.Competencies.Where(w => w.Section_Id == section.Id && w.IsActive).ToListAsync();

            var Competencies_Outcame_Evidence = new Competencies_Outcame_Evidence_V2()
            {
                Competencies = Competencies,
   
            };
            return Competencies_Outcame_Evidence;
        }
        public async Task<Competencies_Outcame_Evidence_V2> GetCompetencies(int ClassId)
        {
            var workingyear = await GetOrCreateActiveWorkingYearAsync();
            var student = await _context.Student_Class_Section_Years.FirstOrDefaultAsync(s => s.Class_Id == ClassId && s.Working_Year_Id == workingyear.Id && s.IsActive);
            var section = await _context.Sections.FirstOrDefaultAsync(a => a.Id == student.Section_id && a.IsActive);
            var Competencies = await _context.Competencies.Where(w => w.Section_Id == section.Id && w.IsActive).ToListAsync();
            var Competencies_Outcame_Evidence = new Competencies_Outcame_Evidence_V2()
            {
                Competencies = Competencies,
            };
            return Competencies_Outcame_Evidence;
        }
        // إضافة هذه الطرق في CompetenciesService
        public async Task<List<Try>> GetAllTriesAsync()
        {
            var query =await _context.Try
    .Include(t => t.CreatedBy)
    .ToListAsync();
            return query;
        }
        public async Task<List<Learning_Outcome>> GetLearningOutcomesByCompetencyId(int competencyId)
        {
            return await _context.Outcomes
                .Where(o => o.Competency_Id == competencyId && o.IsActive)
                .OrderBy(o => o.Name)
                .ToListAsync();
        }
        public async Task<List<StudentEvidenceViewModel>> GetStudentToEvidences(CompetenciesSelectionViewModel model)
        {
            var activeWorkingYear = await _context.Working_Years
    .Where(wy => wy.IsActive)
    .OrderByDescending(wy => wy.Start_date)
    .FirstOrDefaultAsync();

            if (activeWorkingYear == null)
                return null;

            // الحصول على معلومات الفصل
            Classes Class = await _context.Classes.FirstOrDefaultAsync(e => e.Id == model.ClassId);

            if (Class == null)
                return null;

            var studentsInClass = await _context.Student_Class_Section_Years
                .Include(scsy => scsy.Student)
                .Where(scsy => scsy.Class_Id == model.ClassId &&
                              scsy.Working_Year_Id == activeWorkingYear.Id &&
                              scsy.IsActive)
                .ToListAsync();
            var studentStatusList = new List<StudentEvidenceViewModel>();

            foreach (var studentClassSection in studentsInClass)
            {
                //var Student = await _context.Student_Evidence.FirstOrDefaultAsync(e => e.Student_Id == studentClassSection.Student_Id && e.Evidence_Id == model.SelectedEvidenceId.Value && e.Try_Id == model.SelectedTryId.Value);
                var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentClassSection.Student_Id && s.IsActive);
                //if (student != null)
                //{
                //    if (Student != null)
                //    {
                        studentStatusList.Add(new StudentEvidenceViewModel { Student = student, status = true });
                    //}
                    //else
                    //{
                    //    studentStatusList.Add(new StudentEvidenceViewModel { Student = student, status = false });
                //    //}
                //}

            }
            return studentStatusList;
        }

        public async Task<List<Evidence>> GetEvidencesByOutcomeId(int outcomeId)
        {
            return await _context.Evidences
                .Where(e => e.Outcome_Id == outcomeId && e.IsActive&&e.Ispractical)
                .OrderBy(e => e.Name)
                .ToListAsync();
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