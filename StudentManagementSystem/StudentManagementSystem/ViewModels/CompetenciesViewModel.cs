using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentManagementSystem.ViewModels
{
    public class CompetenciesViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الجدارة مطلوب")]
        [Display(Name = "اسم الجدارة")]
        [StringLength(200, ErrorMessage = "اسم الجدارة يجب ألا يزيد عن 200 حرف")]
        public string Name { get; set; }

        [Required(ErrorMessage = "المدة مطلوبة")]
        [Display(Name = "المدة")]
        [StringLength(100, ErrorMessage = "المدة يجب ألا تزيد عن 100 حرف")]
        public string Duration { get; set; }

        [Required(ErrorMessage = "القسم مطلوب")]
        [Display(Name = "القسم")]
        public int Section_Id { get; set; }

        [Required(ErrorMessage = "الحد الأقصى للنتائج مطلوب")]
        [Display(Name = "الحد الأقصى للنتائج")]
        [Range(1, int.MaxValue, ErrorMessage = "الحد الأقصى للنتائج يجب أن يكون أكبر من صفر")]
        public int Max_Outcome { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }

        public int? CreatedBy_Id { get; set; }
        public DateTime? CreatedDate { get; set; }

        // Navigation properties for display
        [Display(Name = "اسم القسم")]
        public string? SectionName { get; set; }

        [Display(Name = "منشئ بواسطة")]
        public string? CreatedByName { get; set; }

        // For dropdowns
        public List<SelectListItem>? Sections { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? Employees { get; set; } = new List<SelectListItem>();
    }

    public class CompetenciesIndexViewModel
    {
        public List<CompetenciesViewModel> Competencies { get; set; } = new List<CompetenciesViewModel>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
        public int? SectionFilter { get; set; }
        public bool? IsActiveFilter { get; set; }
    }

    public class CompetenciesDetailsViewModel
    {
        public CompetenciesViewModel Competency { get; set; }
        public List<LearningOutcomeViewModel> LearningOutcomes { get; set; } = new List<LearningOutcomeViewModel>();
    }

    public class LearningOutcomeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}