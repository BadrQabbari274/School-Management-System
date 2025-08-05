// ›Ì „Ã·œ ViewModels
namespace StudentManagementSystem.ViewModels
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public string CreatedBy { get; set; }
    }
}