using StudentManagementSystem.Models;
namespace StudentManagementSystem.ViewModels
{
    public class EditRegularAbsenceViewModel
    {
        public int AbsenceReasonId { get; set; }
        public string CustomReasonDetails { get; set; }
        public int CreatedById { get; set; }
    }
}