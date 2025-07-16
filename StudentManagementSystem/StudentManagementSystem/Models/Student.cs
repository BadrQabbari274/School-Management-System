using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace StudentManagementSystem.Models
{

    // Student Model
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Natrual_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Date_of_birth { get; set; }

        [Required]
        [StringLength(100)]
        public string Adress { get; set; }

        [Required]
        [StringLength(100)]
        public string Jop_of_Father { get; set; }
        [Required]
        [StringLength(100)]
        public string Jop_of_Mother { get; set; }
        [Required]
        [StringLength(100)]
        public string Phone_Number { get; set; }

        [Required]
        [StringLength(100)]
        public string Phone_Number_Father { get; set; }
        [Required]
        [StringLength(100)]
        public string Phone_Number_Mother { get; set; }

        [Required]
        [StringLength(100)]
        public string Governarate { get; set; }
        [Required]
        [StringLength(250)]
        public string Picture_Profile { get; set; }
        [Required]
        [StringLength(100)]
        public string birth_Certificate { get; set; }

        [Required]
        [StringLength(100)]
        public string Code { get; set; }
        public bool IsActive { get; set; } = true;
        public int? CreatedBy { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int? ClassId { get; set; }

        // Navigation Properties
        [ForeignKey("CreatedBy")]
        public virtual Employee CreatedByUser { get; set; }

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

        // Collections
        public virtual ICollection<TaskEvaluation> TaskEvaluations { get; set; }
        public virtual ICollection<StudentAbsent> MajorAttendances { get; set; }
        public virtual ICollection<StudentAttendance> StudentAttendances { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
    }

}
