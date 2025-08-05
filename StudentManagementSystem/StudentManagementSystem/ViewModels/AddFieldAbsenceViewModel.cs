using StudentManagementSystem.Models;
namespace StudentManagementSystem.ViewModels
{
    public class AddFieldAbsenceViewModel
    {
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public int WorkingYearId { get; set; }
        public int SectionId { get; set; }
        public DateTime Date { get; set; }
        public int AbsenceReasonId { get; set; }
        public string Reason { get; set; } // ··€Ì«» «·„Ìœ«‰Ì ‰” Œœ„ Reason
        public int CreatedById { get; set; }
    }
}