//using StudentManagementSystem.ViewModels;
//using System.Drawing;
//using StudentManagementSystem.Service.Interface;
//using OfficeOpenXml;
//using OfficeOpenXml.Style;
//namespace StudentManagementSystem.Service.Implementation
//{

//    public class ExcelExportService : IExcelExportService
//    {
//        public async Task<byte[]> ExportStudentStatisticsToExcel(List<StudentAttendanceStatistics> statistics)
//        {
//            using (var package = new ExcelPackage())
//            {
//                var worksheet = package.Workbook.Worksheets.Add("إحصائيات الطلاب");

//                // إضافة العناوين
//                worksheet.Cells[1, 1].Value = "اسم الطالب";
//                worksheet.Cells[1, 2].Value = "كود الطالب";
//                worksheet.Cells[1, 3].Value = "عدد أيام الحضور";
//                worksheet.Cells[1, 4].Value = "عدد أيام الغياب";
//                worksheet.Cells[1, 5].Value = "نسبة الحضور";
//                worksheet.Cells[1, 6].Value = "أكثر أسباب الغياب";
//                worksheet.Cells[1, 7].Value = "عدد مرات السبب";

//                // تعبئة البيانات
//                for (int i = 0; i < statistics.Count; i++)
//                {
//                    var stat = statistics[i];
//                    var row = i + 2;

//                    worksheet.Cells[row, 1].Value = stat.Student.Name;
//                    worksheet.Cells[row, 2].Value = stat.Student.Code;
//                    worksheet.Cells[row, 3].Value = stat.PresentDays;
//                    worksheet.Cells[row, 4].Value = stat.AbsentDays;
//                    worksheet.Cells[row, 5].Value = stat.AttendancePercentage.ToString("0.00") + "%";

//                    var topReason = stat.AbsenceReasons.OrderByDescending(r => r.Value).FirstOrDefault();
//                    worksheet.Cells[row, 6].Value = topReason.Key ?? "لا يوجد";
//                    worksheet.Cells[row, 7].Value = topReason.Value;
//                }

//                // تنسيق الجدول
//                worksheet.Cells.AutoFitColumns();
//                using (var range = worksheet.Cells[1, 1, 1, 7])
//                {
//                    range.Style.Font.Bold = true;
//                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
//                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
//                }

//                return await package.GetAsByteArrayAsync();
//            }
//        }
//    }
//}
