namespace StudentManagementSystem.ViewModels
{
    public class StudentConfirmationViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string CurrentStatus { get; set; }
        public string CurrentStatusType { get; set; }
        public string NewStatus { get; set; }
        public string CurrentReason { get; set; }
        public bool ConfirmChange { get; set; } = true;
    }
}
