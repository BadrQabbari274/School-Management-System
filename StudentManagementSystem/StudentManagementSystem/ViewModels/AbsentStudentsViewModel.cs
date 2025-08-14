using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class AbsentStudentsViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string DepartmentName { get; set; }
        public string GradeName { get; set; }
        public int GradeId { get; set; }
        public int ClassId { get; set; }
        public string AbsenceReason { get; set; }
        public string CustomReasonDetails { get; set; }
        public DateTime AbsenceDate { get; set; }
        public string AttendanceTypeName { get; set; }
        public string StudentProfilePicture { get; set; }
    }

    public class AbsentStudentsListViewModel
    {
        public List<AbsentStudentsViewModel> AbsentStudents { get; set; } = new List<AbsentStudentsViewModel>();
        public DateTime AbsenceDate { get; set; }
        public int TotalAbsentCount { get; set; }
    }
}
