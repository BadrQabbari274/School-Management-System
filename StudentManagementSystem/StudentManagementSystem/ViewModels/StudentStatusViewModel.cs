using StudentManagementSystem.ViewModels;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class StudentStatusViewModel
    {
        public Students Students { get; set; } 
        public bool Status  { get; set; } =true;
        public string? CustomReasonDetails;
    }
}
