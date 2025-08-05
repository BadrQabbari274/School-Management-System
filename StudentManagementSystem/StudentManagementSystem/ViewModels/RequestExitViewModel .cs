using StudentManagementSystem.Models;
namespace StudentManagementSystem.ViewModels
{
    public class RequestExitViewModel
    {
        public int? Id { get; set; }
        public int AttendanceId { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan ExitTime { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
    }
}