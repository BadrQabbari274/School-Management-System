//using Microsoft.EntityFrameworkCore;
//using StudentManagementSystem.Data;
//using StudentManagementSystem.Models;
//using StudentManagementSystem.Service.Implementation;
//using StudentManagementSystem.Service.Interface;
//using StudentManagementSystem.ViewModels;
//using System.Globalization;


//namespace StudentManagementSystem.Service.Interface
//{
//    public interface IAttendanceService
//    {
//        // إعداد أنواع الحضور والغياب
//        Task InitializeAttendanceTypesAsync();

//        // جلب الطلاب
//        Task<List<ClassStudentDto>> GetStudentsByClassAsync(int classId);

//        // تسجيل الحضور العادي
//        Task<bool> RecordRegularAttendanceAsync(RegularAttendanceDto attendanceData);

//        // تسجيل الحضور الميداني
//        Task<List<AbsenceReasonDto>> GetAbsenceReasonsAsync();
//        Task<bool> RecordFieldAttendanceAsync(FieldAttendanceDto attendanceData);

//        // عرض سجلات الحضور
//        Task<AttendanceDisplayDto> GetAttendanceRecordsByDateAsync(int classId, DateTime date, bool isRegular = true);

//        // تعديل سجلات الحضور
//        Task<bool> UpdateAttendanceRecordAsync(UpdateAttendanceDto updateData);

//        // تقارير الطلاب الفردية
//        Task<StudentAttendanceReportDto> GetStudentAttendanceReportAsync(int studentId, DateTime fromDate, DateTime toDate);

//        // طلبات الخروج
//        Task<bool> CreateExitRequestAsync(ExitRequestCreateDto exitRequest);

//        // تقارير الفصول
//        Task<ClassAttendanceReportDto> GetClassAttendanceReportAsync(int classId, DateTime fromDate, DateTime toDate);
//    }


//}