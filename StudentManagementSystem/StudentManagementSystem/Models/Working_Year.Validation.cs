using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    [MetadataType(typeof(WorkingYearMetadata))]
    public partial class Working_Year
    {
    }

    public class WorkingYearMetadata
    {
        [Required(ErrorMessage = "اسم السنة العملية مطلوب.")]
        [StringLength(100, ErrorMessage = "اسم السنة العملية يجب أن يكون أقل من 100 حرف.")]
        [Display(Name = "اسم السنة العملية")]
        public string Name { get; set; }

        [Display(Name = "تاريخ البداية")]
        [DataType(DataType.Date)]
        public DateTime? Start_date { get; set; }

        [Display(Name = "تاريخ النهاية")]
        [DataType(DataType.Date)]
        public DateTime? End_date { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name = "أنشئ بواسطة")]
        public int? CreatedBy_Id { get; set; }
    }
}