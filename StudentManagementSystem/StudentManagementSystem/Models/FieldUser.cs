using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    // Field Users (Many-to-Many relationship)
    public class FieldUser
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }
        public int? FieldId { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("FieldId")]
        public virtual Field Field { get; set; }
    }
}
