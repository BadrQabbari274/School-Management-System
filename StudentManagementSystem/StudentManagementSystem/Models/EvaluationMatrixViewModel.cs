using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class EvaluationMatrixViewModel
    {
        public int ClassId { get; set; }
        public int CompetencyId { get; set; }
        public int TryId { get; set; }
        public string ClassName { get; set; }
        public List<Evidence> PracticalEvidences { get; set; } = new List<Evidence>();
        public List<StudentEvaluationRowViewModel> StudentEvaluationRows { get; set; } = new List<StudentEvaluationRowViewModel>();
    }

    public class StudentEvaluationRowViewModel
    {
        public Students Student { get; set; }
        public List<EvidenceStatusViewModel> EvidenceStatuses { get; set; } = new List<EvidenceStatusViewModel>();
    }

    public class EvidenceStatusViewModel
    {
        public int EvidenceId { get; set; }
        public string EvidenceName { get; set; }
        public bool IsEvaluated { get; set; }
        public int StudentId { get; set; }
        public int TryId { get; set; }
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