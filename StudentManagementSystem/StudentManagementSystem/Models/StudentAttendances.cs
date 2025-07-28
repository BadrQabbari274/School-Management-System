using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class StudentAttendances
    {
        [Key]
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime Date { get; set; }
        public int AttendanceTypeId { get; set; }
        public string CustomReasonDetails { get; set; }

        // Composite key reference fields
        public int StudentClassSectionYear_Student_Id { get; set; }
        public int StudentClassSectionYear_Class_Id { get; set; }
        public int StudentClassSectionYear_Working_Year_Id { get; set; }
        public int StudentClassSectionYear_Section_id { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy_Id")]
        public virtual Employees CreatedBy { get; set; }

        [ForeignKey("AttendanceTypeId")]
        public virtual AttendanceTypes AttendanceType { get; set; }

        // Composite foreign key reference
        [ForeignKey("StudentClassSectionYear_Student_Id,StudentClassSectionYear_Class_Id,StudentClassSectionYear_Working_Year_Id,StudentClassSectionYear_Section_id")]
        public virtual Student_Class_Section_Year StudentClassSectionYear { get; set; }

        public virtual ICollection<RequestExits> RequestExits { get; set; }
    }
}
