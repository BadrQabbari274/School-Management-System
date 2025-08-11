using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Service.Interface
{
    public interface IExcelExportService
    {
        Task<byte[]> ExportStudentStatisticsToExcel(List<StudentAttendanceStatistics> statistics);
    }
}
