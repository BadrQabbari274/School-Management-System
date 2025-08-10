using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }
        public int LastEditBy_Id { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [Display(Name = "اسم المستخدم")]
        public string Username { get; set; }

        [Required(ErrorMessage = "الدور مطلوب")]
        [Display(Name = "الدور")]
        public int? RoleId { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;


    }
}
