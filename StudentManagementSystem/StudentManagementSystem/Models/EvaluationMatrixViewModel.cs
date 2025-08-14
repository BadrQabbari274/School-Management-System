using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    // ViewModel للصفحة الرئيسية
    public class StudentsTasksEvaluationViewModel
    {
        public int ClassId { get; set; }
        public int SelectedTryId { get; set; }
        public string ClassName { get; set; }
        public string TryName { get; set; }

        // قائمة المهام (عناوين الأعمدة)
        public List<TaskHeaderViewModel> Tasks { get; set; } = new List<TaskHeaderViewModel>();

        // قائمة الطلاب مع حالة كل مهمة
        public List<StudentTasksRowViewModel> StudentsRows { get; set; } = new List<StudentTasksRowViewModel>();
    }

    // ViewModel لعناوين المهام
    public class TaskHeaderViewModel
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
    }

    // ViewModel لصف كل طالب
    public class StudentTasksRowViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }

        // حالة كل مهمة للطالب
        public List<StudentTaskStatusViewModel> TasksStatus { get; set; } = new List<StudentTaskStatusViewModel>();
    }

    // ViewModel لحالة كل مهمة للطالب
    public class StudentTaskStatusViewModel
    {
        public int TaskId { get; set; }
        public int StudentId { get; set; }
        public bool IsEvaluated { get; set; } // هل تم التقييم أم لا
        public string Status { get; set; } // "تقييم الآن" أو "تم التقييم"
        public DateTime? EvaluationDate { get; set; }
    }

    // ViewModel لإرسال بيانات التقييم
    public class EvaluateStudentTaskViewModel
    {
        public int StudentId { get; set; }
        public int TaskId { get; set; }
        public int TryId { get; set; }
        public IFormFile EvaluationImage { get; set; }
        public int ClassId { get; set; }
    }

    public class CompetenciesSelectionViewModel_V2
    {
        public int ClassId { get; set; }
        public int? SelectedCompetencyId { get; set; }
        public int? SelectedOutcomeId { get; set; }
        public int? SelectedTryId { get; set; }
        public int? SelectedEvidenceId { get; set; }

        // Lists for dropdowns
        public List<SelectListItem> Competencies { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Outcomes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Evidences { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Tries { get; set; } = new List<SelectListItem>();
    }
}