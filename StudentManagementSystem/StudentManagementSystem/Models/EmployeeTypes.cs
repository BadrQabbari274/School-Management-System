using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class EmployeeTypes
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<Employees> Employees { get; set; }
    }
}
