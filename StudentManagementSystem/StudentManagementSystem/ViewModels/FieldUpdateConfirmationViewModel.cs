using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class FieldUpdateConfirmationViewModel
    {
        public Classes Class { get; set; }
        public List<StudentFieldChangeViewModel> StudentsRequiringUpdate { get; set; }
        public List<AbsenceReasons> AvailableReasons { get; set; }
        public string Message { get; set; }
    }
}
