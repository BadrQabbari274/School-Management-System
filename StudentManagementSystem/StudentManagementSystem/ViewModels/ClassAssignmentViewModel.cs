namespace StudentManagementSystem.ViewModels
{
    // ClassAssignmentViewModel.cs
    public class ClassAssignmentViewModel
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public int ClassId { get; set; }
        public List<int> SelectedStudentIds { get; set; } = new List<int>();
        public List<ClassInfoDto> Classes { get; set; }
        public List<StudentClassAssignmentDto> Students { get; set; }
    }

    // ClassInfoDto.cs
    public class ClassInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MaxStudents { get; set; }
        public int CurrentStudents { get; set; }
    }

    // StudentClassAssignmentDto.cs
    public class StudentClassAssignmentDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string SectionName { get; set; }
        public bool IsAssigned { get; set; }
        public string PreviousClassName { get; set; } // For wheeler/senior only
    }
}
