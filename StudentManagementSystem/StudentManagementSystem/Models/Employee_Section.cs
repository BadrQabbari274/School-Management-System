using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Employee_Section
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Department_Id { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual Employees User { get; set; }

        [ForeignKey("Section_Id")]
        public virtual Section Section { get; set; }
    }
}
