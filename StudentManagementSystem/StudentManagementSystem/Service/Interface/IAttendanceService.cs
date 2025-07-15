using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Service.Interface
{
    public interface IAttendanceService
    {
        // Student Attendance Operations
        Task<IEnumerable<StudentAttendance>> GetAllStudentAttendancesAsync();
        Task<StudentAttendance> GetStudentAttendanceByIdAsync(int id);
        Task<StudentAttendance> CreateStudentAttendanceAsync(StudentAttendance studentAttendance);
        Task<StudentAttendance> UpdateStudentAttendanceAsync(StudentAttendance studentAttendance);
        Task<bool> DeleteStudentAttendanceAsync(int id);

        // Student Absent Operations
        Task<IEnumerable<StudentAbsent>> GetAllStudentAbsentsAsync();
        Task<StudentAbsent> GetStudentAbsentByIdAsync(int id);
        Task<StudentAbsent> CreateStudentAbsentAsync(StudentAbsent studentAbsent);
        Task<StudentAbsent> UpdateStudentAbsentAsync(StudentAbsent studentAbsent);
        Task<bool> DeleteStudentAbsentAsync(int id);

        // Absence Reason Operations
        Task<IEnumerable<AbsenceReason>> GetAllAbsenceReasonsAsync();
        Task<AbsenceReason> GetAbsenceReasonByIdAsync(int id);
        Task<AbsenceReason> CreateAbsenceReasonAsync(AbsenceReason absenceReason);
        Task<AbsenceReason> UpdateAbsenceReasonAsync(AbsenceReason absenceReason);
        Task<bool> DeleteAbsenceReasonAsync(int id);

        // Attendance Type Operations
        Task<IEnumerable<AttendanceType>> GetAllAttendanceTypesAsync();
        Task<AttendanceType> GetAttendanceTypeByIdAsync(int id);
        Task<AttendanceType> CreateAttendanceTypeAsync(AttendanceType attendanceType);
        Task<AttendanceType> UpdateAttendanceTypeAsync(AttendanceType attendanceType);
        Task<bool> DeleteAttendanceTypeAsync(int id);

        // Class Attendance Report
        Task<ClassAttendanceReport> GetClassAttendanceReportAsync(int classId, DateTime date);
        Task<ClassAttendanceReport> GetClassAttendanceReportByDateRangeAsync(int classId, DateTime startDate, DateTime endDate);
    }
}
