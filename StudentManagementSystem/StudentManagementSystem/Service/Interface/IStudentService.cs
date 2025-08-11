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
        Task<bool> ConfirmFieldAttendanceChangesAsync(int classId, List<int> studentIds, List<bool> newStatuses, int userId);
        Task<List<StudentFieldChangeViewModel>> GetStudentsRequiringFieldUpdateAsync(int classId, List<int> changedStudentIds);

        Task<IEnumerable<Students>> GetActiveStudentsAsync();
        Task<AttendanceViewModel> GetStudentsAsync(int classId);
        Task<AttendanceViewModel> GetStudentsFieldAsync(int classId);
        Task<bool> SaveAttendanceFieldAsync(AttendanceViewModel model, DateTime attendanceDate, int UserId);


        Task<bool> SaveAttendanceAsync(AttendanceViewModel model, DateTime attendanceDate, int UserId);
        Task<IEnumerable<Students>> GetStudentsByClassAsync(int classId);

        // إضافة طالب بدون فصل (فقط student_id, working_year_id, section_id)
        Task<bool> AddStudentWithoutClassAsync(int studentId, int sectionId, int userid, int? workingYearId = null);
        Task<List<SectionWithStudents>> GetStudentsGroupedBySectionAsync();
        Task<List<ClassWithStudents>> GetStudentsGroupedByClassAsync(int GradeId);
        // تعيين فصل لطالب
        Task<bool> AssignClassToStudentAsync(int studentId, int classId, int? workingYearId = null);
        Task<bool> AssignGradeToStudentAsync(int studentId, int gradeId);
        Task<Grades?> GetWheeler();
        Task<Grades?> GetSenior();
        Task<Grades?> GetJunior();
        // إضافة طالب مع كل التفاصيل
        Task<bool> AddStudentWithAllDetailsAsync(int studentId, int sectionId, int classId, int createdById, int? workingYearId = null);

        // جلب الطلاب مجمعين حسب القسم مع ترتيب أبجدي
        Task<List<SectionStudentsViewModel>> GetStudentsByDepartmentAsync(int? workingYearId = null);

        // جلب الطلاب في قسم معين
        Task<List<StudentInfoViewModel>> GetStudentsInSectionAsync(int sectionId, int? workingYearId = null);

        Task<IEnumerable<Classes>> GetClassesByGradeAsync(int gradeId);
        // إضافة هذه الدوال إلى IStudentService

        // تسجيل غياب ميداني للطلاب غير المتسجلين
        Task<bool> MarkUnregisteredStudentsAsFieldAbsentAsync(int classId, int userId);

        // احصائيات الفصل
        Task<ClassStatisticsViewModel> GetClassStatisticsAsync(int classId, DateTime startDate, DateTime endDate);

        // احصائيات الطالب
        Task<StudentStatisticsViewModel> GetStudentStatisticsAsync(int studentId, DateTime startDate, DateTime endDate);

        // تصدير احصائيات الفصل إلى Excel
        Task<byte[]> ExportClassStatisticsToExcelAsync(int classId, DateTime startDate, DateTime endDate);

        // تصدير احصائيات الطالب إلى Excel
        Task<byte[]> ExportStudentStatisticsToExcelAsync(int studentId, DateTime startDate, DateTime endDate);

        // احصائيات جميع الطلاب في الفصل مع امكانية التصدير
        Task<List<StudentStatisticsViewModel>> GetAllStudentsStatisticsInClassAsync(int classId, DateTime startDate, DateTime endDate);

        // تصدير احصائيات جميع الطلاب في الفصل
        Task<byte[]> ExportAllStudentsStatisticsToExcelAsync(int classId, DateTime startDate, DateTime endDate);
    }
}