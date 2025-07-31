using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class SectionWithStudents
    {
        public Section section { get; set; }
        public List<Students> students { get; set; }
    }
}
