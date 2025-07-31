using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Implementation;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Service.Interface
{
    public interface IStudentService
    {
        Task<IEnumerable<Students>> GetAllStudentsAsync();
        Task<Students> GetStudentByIdAsync(int id);
        Task<Students> CreateStudentAsync(Students student, IFormFile profileImage = null, IFormFile birthCertificate = null);
        Task<Students> UpdateStudentAsync(Students student/*, IFormFile profileImage = null, IFormFile birthCertificate = null*/);
        Task<bool> DeleteStudentAsync(int id);
        Task<IEnumerable<Students>> GetActiveStudentsAsync();
        Task<IEnumerable<Students>> GetStudentsByClassAsync(int classId);
        Task<IEnumerable<Students>> GetStudentsWithTasksAsync();
        // إضافة طالب بدون فصل (فقط student_id, working_year_id, section_id)
        Task<bool> AddStudentWithoutClassAsync(int studentId, int sectionId, int userid, int? workingYearId = null);
        Task<List<SectionWithStudents>> GetStudentsGroupedBySectionAsync();
        Task<List<ClassWithStudents>> GetStudentsGroupedByClassAsync(int GradeId);
        // تعيين فصل لطالب
        Task<bool> AssignClassToStudentAsync(int studentId, int classId, int? workingYearId = null);
        Task<bool> AssignGradeToStudentAsync(int studentId, int gradeId);

        // إضافة طالب مع كل التفاصيل
        Task<bool> AddStudentWithAllDetailsAsync(int studentId, int workingYearId, int sectionId, int classId, int createdById);

        // جلب الطلاب مجمعين حسب القسم مع ترتيب أبجدي
        Task<List<SectionStudentsDto>> GetStudentsByDepartmentAsync(int? workingYearId = null);

        // جلب الطلاب في قسم معين
        Task<List<StudentInfoDto>> GetStudentsInSectionAsync(int sectionId, int? workingYearId = null);

        Task<IEnumerable<Classes>> GetClassesByGradeAsync(int gradeId);
    }
}