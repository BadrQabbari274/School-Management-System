using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class Competencies_Outcame_Evidence
    {
        public List<Competencies> Competencies { get; set; }
        public List<Learning_Outcome> LearningOutcomes { get; set; }
        public List<Evidence> evidences { get; set; }
    }
}
