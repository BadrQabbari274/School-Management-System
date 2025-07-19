using StudentManagementSystem.Controllers;

namespace StudentManagementSystem.ViewModels
{
    public class FieldAttendanceViewModel
    {
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
        public List<StudentFieldAttendanceItem> Students { get; set; } = new List<StudentFieldAttendanceItem>();
    }
}
