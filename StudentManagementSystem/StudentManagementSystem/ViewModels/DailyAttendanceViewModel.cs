using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class DailyAttendanceViewModel
    {
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
        public List<StudentAttendanceItem> Students { get; set; } = new List<StudentAttendanceItem>();
    }
}
