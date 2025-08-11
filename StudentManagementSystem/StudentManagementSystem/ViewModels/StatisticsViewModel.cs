using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class StatisticsViewModel
    {
        [Display(Name = "الصف")]
        public int? SelectedGradeId { get; set; }
        public SelectList GradesList { get; set; }

        [Display(Name = "الفصل")]
        public int? SelectedClassId { get; set; }
        public SelectList ClassesList { get; set; }

        [Display(Name = "تاريخ البداية")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; } = DateTime.Today.AddDays(-7);

        [Display(Name = "تاريخ النهاية")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; } = DateTime.Today;
    }
}