using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;
namespace StudentManagementSystem.ViewModels
{


    public class GradeSelectionViewModel
    {
        public int? SelectedGradeId { get; set; }
        public IEnumerable<SelectListItem> GradesList { get; set; }
        public IEnumerable<Classes> ClassesResult { get; set; }

        public GradeSelectionViewModel()
        {
            GradesList = new List<SelectListItem>();
            ClassesResult = new List<Classes>();
        }
    }
}
