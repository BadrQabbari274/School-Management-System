using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    public interface IAttendanceService
    {
        Task<List<Student>> GetStudentsForAttendanceAsync(int classId);
        Task<List<StudentAttendance>> GetTodayRegularAttendanceAsync(int classId);
        Task<List<StudentAttendance>> GetTodayFieldAttendanceAsync(int classId);
        Task<List<StudentAbsent>> GetTodayRegularAbsentAsync(int classId);
        Task<List<StudentAbsent>> GetTodayFieldAbsentAsync(int classId);
        Task<List<AbsenceReason>> GetAbsenceReasonsAsync();
        Task<bool> TakeRegularAttendanceAsync(List<RegularAttendanceDto> attendanceList, int createdBy);
        Task<bool> TakeFieldAttendanceAsync(List<FieldAttendanceDto> attendanceList, int createdBy);
        Task<bool> CanTakeRegularAttendanceAsync();
        Task<bool> HasRegularAttendanceTodayAsync(int classId);
        Task<bool> HasFieldAttendanceTodayAsync(int classId);
        Task<bool> CanModifyAttendanceAsync(DateTime attendanceDate, int currentUserId, bool isAdmin);
        Task<List<Class>> GetActiveClassesAsync();
        Task<bool> CreateExitRequestAsync(ExitRequestDto exitRequest, int createdBy);
        Task<List<RequestExit>> GetTodayExitRequestsAsync(int classId);
    }
}