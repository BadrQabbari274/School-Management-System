namespace StudentManagementSystem.ViewModels
{


    public class ExitRequestViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public int? AttendanceId { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
    }
}
