using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class TryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }

        [Display(Name = "تم الإنشاء بواسطة")]
        public int CreatedBy_Id { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedDate { get; set; }

        // Display properties
        public string CreatedByName { get; set; }

        // SelectLists
        public List<SelectListItem> Employees { get; set; } = new List<SelectListItem>();
    }

    public class TryIndexViewModel
    {
        public List<TryViewModel> Tries { get; set; } = new List<TryViewModel>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
        public bool? IsActiveFilter { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

    public class TryDetailsViewModel
    {
        public TryViewModel Try { get; set; }
        public int StudentEvidencesCount { get; set; }
    }
}