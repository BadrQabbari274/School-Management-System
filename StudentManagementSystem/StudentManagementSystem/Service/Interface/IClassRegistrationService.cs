//using StudentManagementSystem.Models;

//namespace StudentManagementSystem.Service.Interface
//{
//    public interface IClassRegistrationService
//    {
//        Task<List<ClassWithStudentCountDto>> GetClassesByGradeWithStudentCountAsync(int gradeId);
//        Task<ClassRegistrationViewModel> GetJuniorStudentsAsync(int gradeId, int classId);
//        Task<ClassRegistrationViewModel> GetAdvancedGradeStudentsAsync(int gradeId, int classId);
//        Task<bool> RegisterJuniorStudentsAsync(List<int> studentIds, int classId, int createdById);
//        Task<bool> RegisterAdvancedGradeStudentsAsync(List<int> studentIds, int gradeId, int classId, int sectionId, int createdById);
//        Task<List<StudentInfoDto>> GetStudentsBySectionForAdvancedGradeAsync(int sectionId, int gradeId);
//        Task<List<StudentInfoDto>> GetEligibleStudentsForAdvancedGradeAsync(int targetGradeId, string previousGradeName);
//    }

//    // DTOs
//    public class ClassWithStudentCountDto
//    {
//        public int ClassId { get; set; }
//        public string ClassName { get; set; }
//        public int? MaxStudents { get; set; }
//        public int CurrentStudentCount { get; set; }
//        public bool IsFull => MaxStudents.HasValue && CurrentStudentCount >= MaxStudents.Value;
//        public string ClassPrefix => !string.IsNullOrEmpty(ClassName) ? ClassName.Substring(0, 1).ToUpper() : "";
//        public string AvailabilityText => MaxStudents.HasValue
//            ? $"{CurrentStudentCount}/{MaxStudents.Value}"
//            : CurrentStudentCount.ToString();
//    }

//    public class ClassRegistrationViewModel
//    {
//        public Grades Grade { get; set; }
//        public Classes Class { get; set; }
//        public List<StudentInfoDto> AvailableStudents { get; set; } = new List<StudentInfoDto>();
//        public List<StudentInfoDto> RegisteredStudents { get; set; } = new List<StudentInfoDto>();
//        public List<Section> Sections { get; set; } = new List<Section>();
//        public bool IsJuniorGrade => Grade?.Name?.ToLower() == "junior";
//        public string GradeDisplayName => Grade?.Name ?? "";
//        public string ClassDisplayName => Class?.Name ?? "";
//        public int? MaxStudents => Class?.MaxStudents;
//        public int CurrentRegisteredCount => RegisteredStudents?.Count ?? 0;
//        public int AvailableSlots => MaxStudents.HasValue ? Math.Max(0, MaxStudents.Value - CurrentRegisteredCount) : int.MaxValue;
//    }

//    public class StudentInfoDto
//    {
//        public int StudentId { get; set; }
//        public string StudentName { get; set; }
//        public string StudentCode { get; set; }
//        public string ClassName { get; set; }
//        public int? ClassId { get; set; }
//        public string Phone { get; set; }
//        public string Email { get; set; }
//        public string Address { get; set; }
//        public string SectionName { get; set; }
//        public int? SectionId { get; set; }
//        public string PreviousClassName { get; set; }
//        public int? PreviousClassId { get; set; }
//        public string WorkingYearName { get; set; }
//        public int? WorkingYearId { get; set; }
//    }
//}