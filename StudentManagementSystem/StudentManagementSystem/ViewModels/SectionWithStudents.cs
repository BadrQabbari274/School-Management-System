using StudentManagementSystem.Models;
using static System.Collections.Specialized.BitVector32;

namespace StudentManagementSystem.ViewModels
{
    public class SectionWithStudents
    {
        public StudentManagementSystem.Models.Section section { get; set; }
        public List<Students> students { get; set; } = new List<Students>();
    }
}
