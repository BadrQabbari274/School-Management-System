using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Employee_Department
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Department_Id { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual Employees User { get; set; }

        [ForeignKey("Department_Id")]
        public virtual Department Department { get; set; }
    }
}
