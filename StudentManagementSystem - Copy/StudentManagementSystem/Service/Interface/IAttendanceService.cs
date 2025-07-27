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

        // Bulk attendance operations for daily attendance
        Task<bool> SaveDailyAttendanceBulkAsync(int classId, DateTime date, List<int> presentStudentIds, int createdBy);
        Task<List<StudentAttendance>> GetDailyAttendanceByClassAndDateAsync(int classId, DateTime date);

        // Student Absent Operations
        Task<IEnumerable<StudentAbsent>> GetAllStudentAbsentsAsync();
        Task<StudentAbsent> GetStudentAbsentByIdAsync(int id);
        Task<StudentAbsent> CreateStudentAbsentAsync(StudentAbsent studentAbsent);
        Task<StudentAbsent> UpdateStudentAbsentAsync(StudentAbsent studentAbsent);
        Task<bool> DeleteStudentAbsentAsync(int id);

        // Field attendance operations
        Task<bool> SaveFieldAttendanceBulkAsync(int classId, DateTime date, List<FieldAttendanceRecord> fieldRecords, int createdBy);
        Task<List<StudentAbsent>> GetFieldAttendanceByClassAndDateAsync(int classId, DateTime date);

        // Absence Reason Operations
        Task<IEnumerable<AbsenceReason>> GetAllAbsenceReasonsAsync();
        Task<AbsenceReason> GetAbsenceReasonByIdAsync(int id);
        Task<AbsenceReason> CreateAbsenceReasonAsync(AbsenceReason absenceReason);
        Task<AbsenceReason> UpdateAbsenceReasonAsync(AbsenceReason absenceReason);
        Task<bool> DeleteAbsenceReasonAsync(int id);
        Task<AbsenceReason> GetOrCreateOtherReasonAsync(int createdBy);

        // Attendance Type Operations
        Task<IEnumerable<AttendanceType>> GetAllAttendanceTypesAsync();
        Task<AttendanceType> GetAttendanceTypeByIdAsync(int id);
        Task<AttendanceType> CreateAttendanceTypeAsync(AttendanceType attendanceType);
        Task<AttendanceType> UpdateAttendanceTypeAsync(AttendanceType attendanceType);
        Task<bool> DeleteAttendanceTypeAsync(int id);

        // Exit Request Operations
        Task<RequestExit> CreateExitRequestAsync(RequestExit exitRequest);
        Task<IEnumerable<RequestExit>> GetExitRequestsByDateAsync(DateTime date);
        Task<IEnumerable<RequestExit>> GetPendingExitRequestsAsync();
        Task<bool> ProcessExitRequestAsync(int exitRequestId, int status, int processedBy, string notes = null);

        // Class Attendance Report
        Task<ClassAttendanceReport> GetClassAttendanceReportAsync(int classId, DateTime date);
        Task<ClassAttendanceReport> GetClassAttendanceReportByDateRangeAsync(int classId, DateTime startDate, DateTime endDate);

        // Student-specific attendance methods
        Task<StudentAttendanceSummary> GetStudentAttendanceSummaryAsync(int studentId, DateTime startDate, DateTime endDate);
        Task<bool> IsStudentPresentAsync(int studentId, DateTime date);
    }

    // Helper classes for service operations
    public class FieldAttendanceRecord
    {
        public int StudentId { get; set; }
        public bool IsAbsent { get; set; }
        public int? AbsenceReasonId { get; set; }
        public string CustomAbsenceReason { get; set; }
        public bool WithoutIncentive { get; set; }
    }

    public class StudentAttendanceSummary
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int FieldAbsentDays { get; set; }
        public double AttendancePercentage { get; set; }
        public List<AttendanceDetail> AttendanceDetails { get; set; } = new List<AttendanceDetail>();
    }

    public class AttendanceDetail
    {
        public DateTime Date { get; set; }
        public bool IsDailyPresent { get; set; }
        public bool IsFieldPresent { get; set; }
        public string AbsenceReason { get; set; }
        public bool WithoutIncentive { get; set; }
    }
}