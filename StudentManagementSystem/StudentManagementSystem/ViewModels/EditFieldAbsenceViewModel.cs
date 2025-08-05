using StudentManagementSystem.Models;
namespace StudentManagementSystem.ViewModels
{
    public class EditFieldAbsenceViewModel
    {
        public int AbsenceReasonId { get; set; }
        public string Reason { get; set; }
        public int CreatedById { get; set; }
    }
}