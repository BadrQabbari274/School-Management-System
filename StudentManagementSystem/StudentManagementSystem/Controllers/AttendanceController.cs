using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Service.Implementation;
using StudentManagementSystem.Service.Interface;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class AttendanceController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AttendanceController(IStudentService studentService, IWebHostEnvironment webHostEnvironment)
        {
            _studentService = studentService;

            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Normal(int classId)
        {
            var classwithstudent = await _studentService.GetStudentsAsync(classId);
            return View(classwithstudent);
        }
        public async Task<IActionResult> Field(int classId)
        {
            var classwithstudent = await _studentService.GetStudentsFieldAsync(classId);
            return View(classwithstudent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAttendance(AttendanceViewModel model, DateTime attendanceDate)
        {
            bool success = await _studentService.SaveAttendanceAsync(model, attendanceDate, GetCurrentUserId());

            if (success)
            {
                SetSuccessMessage("تم حفظ الحضور بنجاح");
                return RedirectToAction("Index"); // أو أي صفحة تريد التوجه إليها
            }
            else
            {
                SetErrorMessage("حدث خطأ أثناء حفظ الحضور");

                return View("Normal", model);
            }
        }
    }
}