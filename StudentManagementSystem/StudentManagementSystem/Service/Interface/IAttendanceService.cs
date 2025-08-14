using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IAttendanceService
{
    // Regular Attendance (الغياب العادي)
    Task<bool> AddRegularAbsenceAsync(AddRegularAbsenceViewModel dto);
    Task<bool> EditRegularAbsenceAsync(int id, EditRegularAbsenceViewModel dto);
    //Task<List<StudentAbsentsDto>> GetRegularAbsencesAsync(int classId, DateTime date);

    // Field Attendance (الغياب الميداني)
    Task<bool> AddFieldAbsenceAsync(AddFieldAbsenceViewModel dto);
    Task<bool> EditFieldAbsenceAsync(int id, EditFieldAbsenceViewModel dto);
    //Task<List<StudentAbsentsDto>> GetFieldAbsencesAsync(int classId, DateTime date);
    Task<List<ClassDto>> GetClassesByGradeAsync(int? gradeId);
    Task<List<AttendanceTypeDto>> GetAllAttendanceTypesAsync();
    // Request Exit
    Task<bool> RequestExitAsync(RequestExitViewModel dto);
    Task<List<RequestExitViewModel>> GetRequestExitsAsync(int attendanceId);
    Task<ClassAttendanceReportDto> GetClassAttendanceReportAsync(int classId, DateTime startDate, DateTime endDate, int? attendanceTypeId = null);
    Task<TodayAttendanceDto> GetTodayClassAttendanceAsync(int classId);
    Task<StudentAttendanceDetailsDto> GetStudentAttendanceDetailsAsync(int studentId, int classId, DateTime startDate, DateTime endDate, int? attendanceTypeId = null);
    // Helper Methods
    Task<List<StudentForAttendanceViewModel>> GetStudentsForRegularAttendanceAsync(int classId, int workingYearId, int sectionId, DateTime date);
    Task<List<StudentForAttendanceViewModel>> GetStudentsForFieldAttendanceAsync(int classId, int workingYearId, int sectionId, DateTime date);
}