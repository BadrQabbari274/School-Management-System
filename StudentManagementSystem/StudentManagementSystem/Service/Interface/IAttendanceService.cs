using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IAttendanceService
    {
        Task<IEnumerable<MajorAttendance>> GetAllMajorAttendancesAsync();
        Task<MajorAttendance> GetMajorAttendanceByIdAsync(int id);
        Task<MajorAttendance> CreateMajorAttendanceAsync(MajorAttendance attendance);
        Task<MajorAttendance> UpdateMajorAttendanceAsync(MajorAttendance attendance);
        Task<bool> DeleteMajorAttendanceAsync(int id);
        Task<IEnumerable<MajorAttendance>> GetMajorAttendancesByStudentAsync(int studentId);
        Task<IEnumerable<MajorAttendance>> GetMajorAttendancesByDateAsync(DateTime date);

        Task<IEnumerable<StudentAttendance>> GetAllStudentAttendancesAsync();
        Task<StudentAttendance> GetStudentAttendanceByIdAsync(int id);
        Task<StudentAttendance> CreateStudentAttendanceAsync(StudentAttendance attendance);
        Task<StudentAttendance> UpdateStudentAttendanceAsync(StudentAttendance attendance);
        Task<bool> DeleteStudentAttendanceAsync(int id);
        Task<IEnumerable<StudentAttendance>> GetStudentAttendancesByStudentAsync(int studentId);
    }
}
