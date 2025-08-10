namespace StudentManagementSystem.ViewModels
{
    public class AttendanceStatisticsRequest
    {
        public int ClassId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
