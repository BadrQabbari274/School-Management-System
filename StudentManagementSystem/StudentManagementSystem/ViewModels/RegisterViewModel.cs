using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        [Display(Name = "الاسم")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [Display(Name = "اسم المستخدم")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "البريد الالكتروني مطلوب")]
        [Display(Name = "البريد الالكتروني")]
        public string? Email { get; set; }


        [Required(ErrorMessage = "الدور مطلوب")]
        [Display(Name = "الدور")]
        public int? RoleId { get; set; }
    }

}
