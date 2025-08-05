namespace StudentManagementSystem.ViewModels
{
    public class StudentAbsentsViewModel
    {

            public int Id { get; set; }
            public int StudentId { get; set; }
            public string StudentName { get; set; }
            public DateTime Date { get; set; }
            public int AbsenceReasonId { get; set; }
            public string AbsenceReasonName { get; set; }
            public string CustomReasonDetails { get; set; }
        

    }
}
