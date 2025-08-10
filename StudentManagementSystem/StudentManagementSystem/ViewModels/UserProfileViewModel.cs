namespace StudentManagementSystem.ViewModels
{
    public class UserManagementViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string LastEditBy { get; set; }
    }
}
