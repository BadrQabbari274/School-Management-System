using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class StudentStatusViewModel
    {
        public Students Students { get; set; }
        public bool Status { get; set; } = true;
        public string? AbsenceReason { get; set; }
        public string? CustomReason { get; set; }
        public int? Reason_Id { get; set; }
        public bool IsEditable { get; set; } = true; // إضافة خاصية القابلية للتعديل
    }
}