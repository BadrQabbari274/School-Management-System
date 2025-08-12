using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Service.Implementation
{
    public class ClassService : IClassService
    {
        private readonly ApplicationDbContext _context;

        public ClassService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Original Class Methods

        public async Task<IEnumerable<Classes>> GetAllClassesAsync()
        {
            return await _context.Classes
                .Include(c => c.CreatedBy)
                .Include(c => c.Grade)
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

         public async Task<ClassDetailsViewModel> GetClassDetailsAsync(int classId)
        {
            var classEntity = await GetClassByIdAsync(classId);

            var currentWorkingYear = await GetCurrentWorkingYearAsync();

            var studentsInClass = await GetStudentsByClassAndWorkingYearAsync(classId, currentWorkingYear.Id);

            // Get grades for the students in the class
            var studentGrades = new List<StudentGrades>();
            foreach (var studentClass in studentsInClass)
            {
                var grade = await GetStudentGradeAsync(studentClass.Student_Id, currentWorkingYear.Id);
                if (grade != null)
                {
                    studentGrades.Add(grade);
                }
            }

            // Count junior students
            var juniorStudentsCount = studentsInClass.Count(s =>
                studentGrades.Any(sg => sg.StudentId == s.Student_Id && sg.Grade.Name.ToLower().Contains("junior")));

            var viewModel = new ClassDetailsViewModel
            {
                Class = classEntity,
                CurrentWorkingYear = currentWorkingYear,
                StudentsInClass = studentsInClass,
                StudentGrades = studentGrades,
                JuniorStudentsCount = juniorStudentsCount
            };

            return viewModel;
        }

        public async Task<Classes> GetClassByIdAsync(int id)
        {
            return await _context.Classes
                .Include(c => c.CreatedBy)
                .Include(c => c.Grade)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<Classes> CreateClassAsync(Classes classEntity)
        {
            classEntity.Date = DateTime.Now;
            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync();
            return classEntity;
        }

        public async Task<Classes> UpdateClassAsync(Classes classEntity)
        {
            _context.Entry(classEntity).State = EntityState.Modified;
            classEntity.IsActive = true;
            await _context.SaveChangesAsync();
            return classEntity;
        }

        public async Task<bool> DeleteClassAsync(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null) return false;
            classEntity.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Classes>> GetActiveClassesAsync()
        {
            return await _context.Classes
                .Include(c => c.Grade)
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Classes>> GetClassesByFieldAsync(int fieldId)
        {
            return await _context.Classes
                .Include(c => c.Grade)
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Classes>> GetJuniorGradeClassesAsync()
        {
            return await _context.Classes
                .Include(c => c.Grade)
                .Where(c => c.IsActive && c.Grade.Name.ToLower().Contains("junior"))
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        #endregion

        #region Student Code Generation Methods

        public async Task<bool> GenerateStudentCodesForClassAsync(int classId)
        {
            try
            {
                // Get current working year
                var currentWorkingYear = await GetCurrentWorkingYearAsync();
                if (currentWorkingYear == null)
                    return false;

                // Get class details
                var classEntity = await GetClassByIdAsync(classId);
                if (classEntity == null)
                    return false;

                // Check if class is junior grade
                if (!classEntity.Grade.Name.ToLower().Contains("junior"))
                    return false;

                // Get students in this class for current working year
                var studentsInClass = await GetStudentsByClassAndWorkingYearAsync(classId, currentWorkingYear.Id);

                if (!studentsInClass.Any())
                    return false;

                // Check if students have junior grade and filter valid students
                var validStudents = new List<Student_Class_Section_Year>();
                foreach (var studentClass in studentsInClass)
                {
                    var studentGrade = await GetStudentGradeAsync(studentClass.Student_Id, currentWorkingYear.Id);
                    if (studentGrade != null && studentGrade.Grade.Name.ToLower().Contains("junior"))
                    {
                        validStudents.Add(studentClass);
                    }
                }

                if (!validStudents.Any())
                    return false;

                // Check if any student already has a code
                if (validStudents.Any(s => !string.IsNullOrEmpty(s.Student.Code)))
                    return false;

                // Sort students by gender (males first) then by name
                var sortedStudents = validStudents
                    .OrderBy(s => GetGenderFromNationalId(s.Student.Natrual_Id)) 
                    .ThenBy(s => s.Student.Name)
                    .ToList();

                // Get base sequence number for this class
                var baseSequenceNumberForClass = await GetNextSequenceNumberForClassAsync(classId, currentWorkingYear.Id);

                // Get current year from working year
                var currentYear = currentWorkingYear.Start_date.Year % 100;

                // Generate codes
                for (int i = 0; i < sortedStudents.Count; i++)
                {
                    var sequenceNumber = baseSequenceNumberForClass + i;
                    var studentCode = $"J03{currentYear:D2}{sequenceNumber:D3}";
                    sortedStudents[i].Student.Code = studentCode;

                    // Update student
                    _context.Entry(sortedStudents[i].Student).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetStudentCodesForClassAsync(int classId)
        {
            try
            {
                var currentWorkingYear = await GetCurrentWorkingYearAsync();
                if (currentWorkingYear == null)
                    return false;

                var classEntity = await GetClassByIdAsync(classId);
                if (classEntity == null)
                    return false;

                if (!classEntity.Grade.Name.ToLower().Contains("junior"))
                    return false;

                var studentsInClass = await GetStudentsByClassAndWorkingYearAsync(classId, currentWorkingYear.Id);

                if (!studentsInClass.Any())
                    return false;

                var validStudents = new List<Student_Class_Section_Year>();
                foreach (var studentClass in studentsInClass)
                {
                    var studentGrade = await GetStudentGradeAsync(studentClass.Student_Id, currentWorkingYear.Id);
                    if (studentGrade != null && studentGrade.Grade.Name.ToLower().Contains("junior"))
                    {
                        validStudents.Add(studentClass);
                    }
                }

                // Reset codes first
                foreach (var studentClass in validStudents)
                {
                    studentClass.Student.Code = null;
                    _context.Entry(studentClass.Student).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();

                // Re-generate codes
                var sortedStudents = validStudents
                    .OrderBy(s => GetGenderFromNationalId(s.Student.Natrual_Id))
                    .ThenBy(s => s.Student.Name)
                    .ToList();

                var baseSequenceNumberForClass = await GetNextSequenceNumberForClassAsync(classId, currentWorkingYear.Id);
                var currentYear = currentWorkingYear.Start_date.Year % 100;

                for (int i = 0; i < sortedStudents.Count; i++)
                {
                    var sequenceNumber = baseSequenceNumberForClass + i;
                    var studentCode = $"J03{currentYear:D2}{sequenceNumber:D3}";
                    sortedStudents[i].Student.Code = studentCode;
                    _context.Entry(sortedStudents[i].Student).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Student_Class_Section_Year>> GetStudentsByClassAndWorkingYearAsync(int classId, int workingYearId)
        {
            return await _context.Student_Class_Section_Years
                .Include(s => s.Student)
                .Include(s => s.Class)
                .Include(s => s.WorkingYear)
                .Where(s => s.Class_Id == classId &&
                           s.Working_Year_Id == workingYearId &&
                           s.IsActive &&
                           s.Student.IsActive)
                .OrderBy(s=> s.Student.Name)
                .ToListAsync();
        }

        public async Task<Working_Year> GetCurrentWorkingYearAsync()
        {
            return await _context.Working_Years
                .Where(wy => wy.IsActive)
                .OrderByDescending(wy => wy.Start_date)
                .FirstOrDefaultAsync();
        }

        public async Task<StudentGrades> GetStudentGradeAsync(int studentId, int workingYearId)
        {
            return await _context.StudentGrades
                .Include(sg => sg.Grade)
                .Include(sg => sg.Student)
                .FirstOrDefaultAsync(sg => sg.StudentId == studentId &&
                                          sg.Working_Year_Id == workingYearId &&
                                          sg.IsActive);
        }

        public async Task<int> GetNextSequenceNumberForClassAsync(int targetClassId, int workingYearId)
        {
            // Get all junior grade classes sorted by name
            var juniorClasses = await GetJuniorGradeClassesAsync();

            int baseSequence = 1;

            foreach (var cls in juniorClasses)
            {
                if (cls.Id == targetClassId)
                {
                    return baseSequence;
                }

                var maxStudentsForClass = cls.MaxStudents ?? 25;
                baseSequence += maxStudentsForClass;
            }

            return baseSequence;
        }

        // Helper method to determine gender from National ID
        private int GetGenderFromNationalId(string nationalId)
        {
            if (string.IsNullOrEmpty(nationalId) || nationalId.Length < 13)
                return 0; // Default to male if invalid ID

            // Get the 13th digit (index 12)
            var genderDigit = nationalId[12];

            // If digit is odd = male (0), if even = female (1)
            return int.Parse(genderDigit.ToString()) % 2 == 1 ? 0 : 1;
        }

        #endregion
    }
}