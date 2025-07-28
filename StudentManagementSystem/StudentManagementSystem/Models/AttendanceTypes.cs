using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class AttendanceTypes
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation properties
        public virtual ICollection<StudentAbsents> StudentAbsents { get; set; }
        public virtual ICollection<StudentAttendances> StudentAttendances { get; set; }
    }
}
