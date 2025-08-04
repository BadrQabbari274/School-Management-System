using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
        public class ClassDetailsViewModel
        {
            public Classes Class { get; set; }
            public Working_Year CurrentWorkingYear { get; set; }
            public IEnumerable<Student_Class_Section_Year> StudentsInClass { get; set; }
            public IEnumerable<StudentGrades> StudentGrades { get; set; }
            public int JuniorStudentsCount { get; set; }

            public int GetGenderFromNationalId(string nationalId)
            {
                if (string.IsNullOrEmpty(nationalId) || nationalId.Length < 13)
                    return 0; 
                var genderDigit = nationalId[12];   
                return int.Parse(genderDigit.ToString()) % 2 == 1 ? 0 : 1;
            }
        }
}