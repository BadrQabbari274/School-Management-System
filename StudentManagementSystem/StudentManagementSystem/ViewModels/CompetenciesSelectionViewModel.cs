using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class CompetenciesSelectionViewModel
    {
        public int ClassId { get; set; }
        public int? SelectedCompetencyId { get; set; }
        public int? SelectedOutcomeId { get; set; }
        public int? SelectedEvidenceId { get; set; }
        public int? SelectedTryId { get; set; }

        public List<SelectListItem> CompetenciesList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> TrysList { get; set; } = new List<SelectListItem>();
        public List<Learning_Outcome> LearningOutcomes { get; set; } = new List<Learning_Outcome>();
        public List<Evidence> Evidences { get; set; } = new List<Evidence>();

        // للعرض الديناميكي
        public List<Learning_Outcome> FilteredOutcomes { get; set; } = new List<Learning_Outcome>();
        public List<Evidence> FilteredEvidences { get; set; } = new List<Evidence>();
    }
}