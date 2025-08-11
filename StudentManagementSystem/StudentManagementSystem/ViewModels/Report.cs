
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    // ViewModel لإحصائيات الفصل
    public class ClassAttendanceStatsViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalStudents { get; set; }
        public int TotalDays { get; set; }
        public int TotalAttendanceRecords { get; set; }
        public int TotalAbsenceRecords { get; set; }
        public double AttendancePercentage { get; set; }
        public double AbsencePercentage { get; set; }
        public List<AbsenceReasonStatsViewModel> TopAbsenceReasons { get; set; } = new List<AbsenceReasonStatsViewModel>();
        public int WorkingYearId { get; set; }
    }

    // ViewModel لإحصائيات الطالب
    public class StudentAttendanceStatsViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public int? ClassId { get; set; }
        public string ClassName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AttendanceDays { get; set; }
        public int AbsenceDays { get; set; }
        public double AttendancePercentage { get; set; }
        public double AbsencePercentage { get; set; }
        public List<AbsenceReasonStatsViewModel> TopAbsenceReasons { get; set; } = new List<AbsenceReasonStatsViewModel>();
        public int WorkingYearId { get; set; }
    }

    // ViewModel لأسباب الغياب في الإحصائيات
    public class AbsenceReasonStatsViewModel
    {
        public int ReasonId { get; set; }
        public string ReasonName { get; set; }
        public int Count { get; set; }
    }

    // ViewModel لصفحة الإحصائيات
    public class AttendanceStatsFilterViewModel
    {
        [Display(Name = "الفصل")]
        public int? SelectedClassId { get; set; }
        public SelectList ClassesList { get; set; }

        [Display(Name = "الطالب")]
        public int? SelectedStudentId { get; set; }
        public SelectList StudentsList { get; set; }

        [Display(Name = "من تاريخ")]
        [Required(ErrorMessage = "يجب تحديد تاريخ البداية")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-30);

        [Display(Name = "إلى تاريخ")]
        [Required(ErrorMessage = "يجب تحديد تاريخ النهاية")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Now;

        [Display(Name = "نوع التقرير")]
        public string ReportType { get; set; } = "class"; // class or student

        // النتائج
        public ClassAttendanceStatsViewModel ClassStats { get; set; }
        public StudentAttendanceStatsViewModel StudentStats { get; set; }

        // للتحقق من صحة التواريخ
        public bool IsValid()
        {
            return StartDate <= EndDate;
        }
    }

    // ViewModel لعرض قائمة الطلاب غير المسجلين
    public class UnregisteredStudentsViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public DateTime Date { get; set; }
        public List<StudentStatusViewModel> UnregisteredStudents { get; set; } = new List<StudentStatusViewModel>();
        public int TotalUnregisteredCount { get; set; }
    }

    // ViewModel لتأكيد تسجيل الطلاب المتأخرين
    public class ConfirmLateStudentsViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public DateTime Date { get; set; }
        public List<int> StudentIds { get; set; } = new List<int>();
        public List<string> StudentNames { get; set; } = new List<string>();
        public string AbsenceReason { get; set; } = "التأخير عن موعد التدريب";
        public int TotalStudents { get; set; }
    }

    // ViewModel محدث لاختيار الصف مع خيار الإحصائيات


}