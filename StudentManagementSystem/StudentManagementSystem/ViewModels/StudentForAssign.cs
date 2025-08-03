using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class StudentForAssign
    {
        public string Type;
        public List<ClassWithStudents>? ClassWithStudents { get; set; }
        public  List<SectionWithStudents>? SectionWithStudents { get; set; }
    }
}
