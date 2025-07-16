using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace StudentManagementSystem.Models
{
    // User Model
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }
<<<<<<< HEAD
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
=======
>>>>>>> c39fc7a54d151426959567b70569143abbefaad2

        public bool IsActive { get; set; } = true;

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        public int? RoleId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; } 

        // Navigation Properties
        [ForeignKey("RoleId")]
        public virtual EmployeeType? Role { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual Employee CreatedByUser { get; set; }

        // Collections
        public virtual ICollection<Employee> CreatedUsers { get; set; }
        public virtual ICollection<Field> CreatedFields { get; set; }
        public virtual ICollection<Grade> CreatedGrades { get; set; }
        public virtual ICollection<Class> CreatedClasses { get; set; }
        public virtual ICollection<Student> CreatedStudents { get; set; }
        public virtual ICollection<Competence> CreatedCompetences { get; set; }
        public virtual ICollection<StudentAttendance> CreatedStudentAttendances { get; set; }
        public virtual ICollection<AbsenceReason> CreatedAbsenceReasons { get; set; }
        public virtual ICollection<Picture> CreatedPictures { get; set; }
        public virtual ICollection<FieldEmployee> FieldEmployees { get; set; }
    }

}