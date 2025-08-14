using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Service.Interface
{
    public interface IClassService
    {
        Task<IEnumerable<Classes>> GetAllClassesAsync();
        Task<Classes> GetClassByIdAsync(int id);
        Task<Classes> CreateClassAsync(Classes classEntity);
        Task<Classes> UpdateClassAsync(Classes classEntity);
        Task<bool> DeleteClassAsync(int id);
        Task<IEnumerable<Classes>> GetActiveClassesAsync();
        Task<IEnumerable<Classes>> GetClassesByFieldAsync(int fieldId);
        Task<IEnumerable<Classes>> GetJuniorGradeClassesAsync();

        // Student Code Generation Methods
        Task<bool> GenerateStudentCodesForClassAsync(int classId);
        Task<bool> ResetStudentCodesForClassAsync(int classId);
        Task<IEnumerable<Student_Class_Section_Year>> GetStudentsByClassAndWorkingYearAsync(int classId, int workingYearId);
        Task<Working_Year> GetCurrentWorkingYearAsync();
        Task<StudentGrades> GetStudentGradeAsync(int studentId, int workingYearId);
        Task<int> GetNextSequenceNumberForClassAsync(int targetClassId, int workingYearId);
        Task<ClassDetailsViewModel> GetClassDetailsAsync(int classId);
        //Task<IEnumerable<object>> GetActiveClassesByGradeAsync(int gradeId);
    }
}
