using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class ClassWithStudents
    {
        public Classes classes { get; set; }
        public List<Students> students { get; set; }
    }
}
