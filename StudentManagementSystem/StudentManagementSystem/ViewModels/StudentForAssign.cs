using StudentManagementSystem.Models;
using static System.Collections.Specialized.BitVector32;

namespace StudentManagementSystem.ViewModels
{
    public class StudentForAssign
    {
        public string Type { get; set; } // "junior" أو "W&S"
        public List<SectionWithStudents> SectionWithStudents { get; set; } = new List<SectionWithStudents>();
        public List<ClassWithStudents> ClassWithStudents { get; set; } = new List<ClassWithStudents>();
        public IEnumerable<StudentManagementSystem.Models.Section> sections { get; set; } = new List<StudentManagementSystem.Models.Section>();
        public int ClassId { get; set; }
        public int GradeId { get; set; }
    }
}
