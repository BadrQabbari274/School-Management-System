using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class StudentStatusViewModel
    {
        public Students Students { get; set; }
        public bool Status { get; set; } = true;
        public string? AbsenceReason { get; set; } // تم تغيير الاسم ليكون أكثر وضوحًا
        public string? CustomReason { get; set; } // تم فصل السبب المخصص
        public string? Reason_Id { get; set; }
    }
}