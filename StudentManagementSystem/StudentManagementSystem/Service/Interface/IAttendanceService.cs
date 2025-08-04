using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IAttendanceService
{
    // Regular Attendance (الغياب العادي)
    Task<bool> AddRegularAbsenceAsync(AddRegularAbsenceDto dto);
    Task<bool> EditRegularAbsenceAsync(int id, EditRegularAbsenceDto dto);
    //Task<List<StudentAbsentsDto>> GetRegularAbsencesAsync(int classId, DateTime date);

    // Field Attendance (الغياب الميداني)
    Task<bool> AddFieldAbsenceAsync(AddFieldAbsenceDto dto);
    Task<bool> EditFieldAbsenceAsync(int id, EditFieldAbsenceDto dto);
    //Task<List<StudentAbsentsDto>> GetFieldAbsencesAsync(int classId, DateTime date);

    // Request Exit
    Task<bool> RequestExitAsync(RequestExitDto dto);
    Task<List<RequestExitDto>> GetRequestExitsAsync(int attendanceId);

    // Helper Methods
    Task<List<StudentForAttendanceDto>> GetStudentsForRegularAttendanceAsync(int classId, int workingYearId, int sectionId, DateTime date);
    Task<List<StudentForAttendanceDto>> GetStudentsForFieldAttendanceAsync(int classId, int workingYearId, int sectionId, DateTime date);
}