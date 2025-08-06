using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class UserShowProfileViewModel
    {
        public int Id { get; set; }

        [Display(Name = "الاسم الكامل")]
        public string Name { get; set; }

        [Display(Name = "اسم المستخدم")]
        public string Username { get; set; }

        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Display(Name = "كلمة المرور")]
        public string MaskedPassword { get; set; } // سيتم عرضها كـ ********

        [Display(Name = "النوع/الدور")]
        public string RoleName { get; set; }

        [Display(Name = "الحالة")]
        public bool IsActive { get; set; }

        [Display(Name = "تاريخ التسجيل")]
        public DateTime Date { get; set; }

        [Display(Name = "تاريخ الانضمام")]
        public DateTime? JoinDate { get; set; }

        [Display(Name = "آخر تسجيل دخول")]
        public DateTime? LastLogin { get; set; }

        [Display(Name = "تم الإنشاء بواسطة")]
        public string CreatedByName { get; set; }

        // للتعديل
        [Display(Name = "كلمة المرور الجديدة")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "تأكيد كلمة المرور")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
        public string ConfirmPassword { get; set; }
    }
}