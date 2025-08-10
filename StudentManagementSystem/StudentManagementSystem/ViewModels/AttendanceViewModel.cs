using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class AttendanceViewModel
    {
        public Classes Class { get; set; }
        public List<StudentStatusViewModel> StudentStatus { get; set; }
        public List<AbsenceReasons>? reasons { get; set; }
        public string? ErrorMessage { get; set; } // إضافة خاصية رسالة الخطأ
    }
}
