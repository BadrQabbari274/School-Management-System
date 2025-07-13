using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    // User Model
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        public int? RoleId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        // Collections
        public virtual ICollection<User> CreatedUsers { get; set; }
        public virtual ICollection<Field> CreatedFields { get; set; }
        public virtual ICollection<AcademicYear> CreatedAcademicYears { get; set; }
        public virtual ICollection<Class> CreatedClasses { get; set; }
        public virtual ICollection<Student> CreatedStudents { get; set; }
        public virtual ICollection<Competence> CreatedCompetences { get; set; }
        public virtual ICollection<MajorAttendance> CreatedMajorAttendances { get; set; }
        public virtual ICollection<StudentAttendance> CreatedStudentAttendances { get; set; }
        public virtual ICollection<AbsenceReason> CreatedAbsenceReasons { get; set; }
        public virtual ICollection<Picture> CreatedPictures { get; set; }
        public virtual ICollection<FieldUser> FieldUsers { get; set; }
    }
}