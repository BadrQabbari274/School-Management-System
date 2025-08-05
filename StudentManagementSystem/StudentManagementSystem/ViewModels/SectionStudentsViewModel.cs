using StudentManagementSystem.Service.Implementation;

namespace StudentManagementSystem.ViewModels
{
    public class SectionStudentsViewModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int WorkingYearId { get; set; }
        public List<StudentInfoViewModel> Students { get; set; } = new List<StudentInfoViewModel>();
    }
}
