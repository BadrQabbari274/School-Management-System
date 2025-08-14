using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class AbsenceReasonViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم سبب الغياب مطلوب")]
        [StringLength(100, ErrorMessage = "اسم سبب الغياب يجب أن يكون أقل من 100 حرف")]
        [Display(Name = "اسم سبب الغياب")]
        public string Name { get; set; }
    }
}