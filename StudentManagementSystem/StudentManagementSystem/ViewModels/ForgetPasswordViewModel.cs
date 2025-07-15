using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class ForgetPassword
    {
        [Required(ErrorMessage = "ID Required")]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [Display(Name = "Password")]
        public string? Password { get; set; }



        [Required(ErrorMessage = "ConfirmPassword Required")]
        [Display(Name = "ConfirmPassword")]
        public string? ConfirmPassword { get; set; }
    }

}
