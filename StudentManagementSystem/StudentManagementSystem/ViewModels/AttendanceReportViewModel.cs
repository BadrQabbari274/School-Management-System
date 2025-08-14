using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class AttendanceReportViewModel
    {
        public int? SelectedGradeId { get; set; }
        public int? SelectedClassId { get; set; }

        [Display(Name = "تاريخ البداية")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "تاريخ النهاية")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "نوع الحضور/الغياب")]
        public int? SelectedAttendanceTypeId { get; set; }

        public SelectList? GradesList { get; set; }
        public SelectList? AttendanceTypesList { get; set; }
        public List<ClassDto> ClassesResult { get; set; } = new List<ClassDto>();

        public ClassAttendanceReportDto? AttendanceReport { get; set; }
        public TodayAttendanceDto? TodayAttendance { get; set; }
    }

    public class ClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AttendanceTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ClassAttendanceReportDto
    {
        public int ClassId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<StudentAttendanceDto> Students { get; set; } = new List<StudentAttendanceDto>();
        public int TotalStudents { get; set; }
        public int TotalPresentDays { get; set; }
        public int TotalAbsentDays { get; set; }
        public double AverageAttendancePercentage { get; set; }
    }

    public class StudentAttendanceDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string ClassName { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public double AttendancePercentage { get; set; }
        public List<string> AbsenceReasons { get; set; } = new List<string>();
        public List<string> AbsenceTypes { get; set; } = new List<string>(); // إضافة أنواع الغياب
    }

    public class TodayAttendanceDto
    {
        public DateTime Date { get; set; }
        public int ClassId { get; set; }
        public int TotalStudents { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public List<TodayStudentAttendanceDto> PresentStudents { get; set; } = new List<TodayStudentAttendanceDto>();
        public List<TodayStudentAttendanceDto> AbsentStudents { get; set; } = new List<TodayStudentAttendanceDto>();
    }

    public class TodayStudentAttendanceDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string AttendanceType { get; set; } // إضافة نوع الحضور/الغياب
    }

    public class StudentAttendanceDetailsDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public double AttendancePercentage { get; set; }
        public List<StudentAttendanceHistoryDto> AttendanceHistory { get; set; } = new List<StudentAttendanceHistoryDto>();
        public List<AbsenceReasonStatDto> AbsenceReasonStats { get; set; } = new List<AbsenceReasonStatDto>();
        public List<AbsenceTypeStatDto> AbsenceTypeStats { get; set; } = new List<AbsenceTypeStatDto>(); // إضافة إحصائيات أنواع الغياب
    }

    public class StudentAttendanceHistoryDto
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string AttendanceType { get; set; } // إضافة نوع الحضور/الغياب
    }

    public class AbsenceReasonStatDto
    {
        public string ReasonName { get; set; }
        public int Count { get; set; }
    }

    public class AbsenceTypeStatDto
    {
        public string TypeName { get; set; }
        public int Count { get; set; }
    }
}