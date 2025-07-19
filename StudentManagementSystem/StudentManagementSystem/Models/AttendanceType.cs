using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class AttendanceType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        // To distinguish between different attendance contexts
        public bool IsForFieldAttendance { get; set; } = false;

        // Collections
        public virtual ICollection<StudentAbsent> StudentAbsents { get; set; }
    }
}
