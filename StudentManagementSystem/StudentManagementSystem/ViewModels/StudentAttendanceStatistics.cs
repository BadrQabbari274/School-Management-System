using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class StudentAttendanceStatistics : ClassAttendanceStatistics
    {
        public Students Student { get; set; }
    }
}
