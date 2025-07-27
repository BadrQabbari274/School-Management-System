using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;

namespace StudentManagementSystem.Service.Implementation
{
    public class StudentManagementService : IStudentManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPictureService _pictureService;
        private readonly ITaskEvaluationService _taskEvaluationService;
        private readonly IStudentService _studentService;

        public StudentManagementService(
            ApplicationDbContext context,
            IPictureService pictureService,
            ITaskEvaluationService taskEvaluationService,
            IStudentService studentService)
        {
            _context = context;
            _pictureService = pictureService;
            _taskEvaluationService = taskEvaluationService;
            _studentService = studentService;
        }

        public async Task<IEnumerable<Students>> GetStudentsWithCompleteDataAsync()
        {
            return await _context.Students
                .Include(s => s.TaskEvaluations)
                .ThenInclude(te => te.Pictures)
                .Include(s => s.Pictures)
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskEvaluations>> GetTaskEvaluationsWithPicturesAsync(int studentId)
        {
            return await _context.TaskEvaluations
                .Include(te => te.Outcome)
                .ThenInclude(o => o.Competence)
                .Include(te => te.Pictures)
                .Where(te => te.StudentId == studentId && !te.IsDeleted)
                .OrderByDescending(te => te.Id)
                .ToListAsync();
        }

        public async Task<bool> SubmitTaskWithPicturesAsync(int taskId, int studentId, IList<IFormFile> pictures, int createdBy)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var taskEvaluation = await _context.TaskEvaluations.FindAsync(taskId);
                if (taskEvaluation == null || taskEvaluation.StudentId != studentId)
                    return false;

                // Save pictures
                foreach (var picture in pictures)
                {
                    var filePath = await _pictureService.SavePictureAsync(picture, studentId, taskId);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        var pictureEntity = new Pictures
                        {
                            FilePath = filePath,
                            StudentId = studentId,
                            TaskId = taskId,
                            CreatedBy_Id = createdBy,
                            CreatedDate = DateTime.Now
                        };
                        await _pictureService.CreatePictureAsync(pictureEntity);
                    }
                }

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<IEnumerable<Students>> GetStudentsByFieldAsync(int fieldId)
        {
            return await _context.Students
   
                .Where(s =>  s.IsActive)
                .ToListAsync();
        }

        public async Task<Dictionary<string, object>> GetStudentDashboardDataAsync(int studentId)
        {
            var student = await _context.Students

                .Include(s => s.TaskEvaluations)
                .ThenInclude(te => te.Pictures)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null) return null;

            return new Dictionary<string, object>
            {
                {"Student", student},
                {"TotalTasks", student.TaskEvaluations.Count(te => !te.IsDeleted)},
                {"CompletedTasks", student.TaskEvaluations.Count(te => !te.IsDeleted && te.Pictures.Any())},
              /*  {"AttendanceRate", CalculateAttendanceRate(student.MajorAttendances.ToList())}*/
                {"RecentTasks", student.TaskEvaluations.Where(te => !te.IsDeleted).OrderByDescending(te => te.Id).Take(5)},
                {"TotalPictures", student.Pictures.Count(p => !p.IsDeleted)}
            };
        }

        public async Task<Dictionary<string, object>> GetFieldStatisticsAsync(int fieldId)
        {
            var students = await GetStudentsByFieldAsync(fieldId);
            var totalStudents = students.Count();
            var totalTasks = await _context.TaskEvaluations
                .CountAsync(te => students.Any(s => s.Id == te.StudentId) && !te.IsDeleted);

            return new Dictionary<string, object>
            {
                {"TotalStudents", totalStudents},
                {"TotalTasks", totalTasks},
                {"ActiveStudents", students.Count(s => s.IsActive)},
                {"AverageTasksPerStudent", totalStudents > 0 ? (double)totalTasks / totalStudents : 0}
            };
        }

        public async Task<bool> BulkUpdateAttendanceAsync(List<StudentAttendances> attendances)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var attendance in attendances)
                {
                    attendance.Date = DateTime.Now;
                    _context.StudentAttendances.Add(attendance);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        //private double CalculateAttendanceRate(List<StudentAbsent> attendances)
        //{
        //    if (!attendances.Any()) return 0;

        //    var presentCount = attendances.Count(a => a == "Present");
        //    return (double)presentCount / attendances.Count * 100;
        //}
    }

}
