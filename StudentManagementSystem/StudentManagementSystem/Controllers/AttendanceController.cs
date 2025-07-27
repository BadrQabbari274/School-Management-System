using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class AttendanceController : BaseController
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        // GET: Attendance
        public async Task<IActionResult> Index()
        {
            var classes = await _attendanceService.GetActiveClassesAsync();
            return View(classes);
        }

        // GET: Attendance/RegularAttendance/5
        public async Task<IActionResult> RegularAttendance(int classId)
        {
            if (!await _attendanceService.CanTakeRegularAttendanceAsync())
            {
                SetErrorMessage("لا يمكن أخذ الغياب في يوم الجمعة والسبت");
                return RedirectToAction(nameof(Index));
            }

            var students = await _attendanceService.GetStudentsForAttendanceAsync(classId);
            var todayAttendance = await _attendanceService.GetTodayRegularAttendanceAsync(classId);
            var todayAbsent = await _attendanceService.GetTodayRegularAbsentAsync(classId);
            var absenceReasons = await _attendanceService.GetAbsenceReasonsAsync();
            var hasAttendanceToday = await _attendanceService.HasRegularAttendanceTodayAsync(classId);

            ViewBag.ClassId = classId;
            ViewBag.TodayAttendance = todayAttendance;
            ViewBag.TodayAbsent = todayAbsent;
            ViewBag.AbsenceReasons = absenceReasons;
            ViewBag.HasAttendanceToday = hasAttendanceToday;
            ViewBag.CanModify = await _attendanceService.CanModifyAttendanceAsync(DateTime.Today, GetCurrentUserId(), IsAdmin());

            return View(students);
        }

        // POST: Attendance/TakeRegularAttendance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TakeRegularAttendance(int classId, List<RegularAttendanceDto> attendance)
        {
            if (!await _attendanceService.CanTakeRegularAttendanceAsync())
            {
                SetErrorMessage("لا يمكن أخذ الغياب في يوم الجمعة والسبت");
                return RedirectToAction(nameof(RegularAttendance), new { classId });
            }

            if (!await _attendanceService.CanModifyAttendanceAsync(DateTime.Today, GetCurrentUserId(), IsAdmin()))
            {
                SetErrorMessage("لا يمكنك تعديل الغياب لهذا اليوم");
                return RedirectToAction(nameof(RegularAttendance), new { classId });
            }

            var result = await _attendanceService.TakeRegularAttendanceAsync(attendance, GetCurrentUserId());

            if (result)
            {
                SetSuccessMessage("تم حفظ الغياب بنجاح");
            }
            else
            {
                SetErrorMessage("حدث خطأ أثناء حفظ الغياب");
            }

            return RedirectToAction(nameof(RegularAttendance), new { classId });
        }

        // GET: Attendance/FieldAttendance/5
        public async Task<IActionResult> FieldAttendance(int classId)
        {
            var hasRegularAttendance = await _attendanceService.HasRegularAttendanceTodayAsync(classId);
            if (!hasRegularAttendance)
            {
                SetErrorMessage("يجب أخذ الغياب العادي أولاً قبل أخذ الغياب الميداني");
                return RedirectToAction(nameof(RegularAttendance), new { classId });
            }

            var students = await _attendanceService.GetStudentsForAttendanceAsync(classId);
            var todayRegularAttendance = await _attendanceService.GetTodayRegularAttendanceAsync(classId);
            var todayRegularAbsent = await _attendanceService.GetTodayRegularAbsentAsync(classId);
            var todayFieldAttendance = await _attendanceService.GetTodayFieldAttendanceAsync(classId);
            var todayFieldAbsent = await _attendanceService.GetTodayFieldAbsentAsync(classId);
            var absenceReasons = await _attendanceService.GetAbsenceReasonsAsync();

            ViewBag.ClassId = classId;
            ViewBag.TodayRegularAttendance = todayRegularAttendance;
            ViewBag.TodayRegularAbsent = todayRegularAbsent;
            ViewBag.TodayFieldAttendance = todayFieldAttendance;
            ViewBag.TodayFieldAbsent = todayFieldAbsent;
            ViewBag.AbsenceReasons = absenceReasons;

            return View(students);
        }

        // POST: Attendance/TakeFieldAttendance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TakeFieldAttendance(int classId, List<FieldAttendanceDto> attendance)
        {
            var result = await _attendanceService.TakeFieldAttendanceAsync(attendance, GetCurrentUserId());

            if (result)
            {
                SetSuccessMessage("تم حفظ الغياب الميداني بنجاح");
            }
            else
            {
                SetErrorMessage("حدث خطأ أثناء حفظ الغياب الميداني");
            }

            return RedirectToAction(nameof(FieldAttendance), new { classId });
        }

        // GET: Attendance/ExitRequest/5
        public async Task<IActionResult> ExitRequest(int classId)
        {
            var students = await _attendanceService.GetStudentsForAttendanceAsync(classId);
            var todayAttendance = await _attendanceService.GetTodayRegularAttendanceAsync(classId);
            var exitRequests = await _attendanceService.GetTodayExitRequestsAsync(classId);

            ViewBag.ClassId = classId;
            ViewBag.TodayAttendance = todayAttendance;
            ViewBag.ExitRequests = exitRequests;

            return View(students);
        }

        // POST: Attendance/CreateExitRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExitRequest(int classId, ExitRequestDto exitRequest)
        {
            var result = await _attendanceService.CreateExitRequestAsync(exitRequest, GetCurrentUserId());

            if (result)
            {
                SetSuccessMessage("تم إنشاء طلب الخروج بنجاح");
            }
            else
            {
                SetErrorMessage("حدث خطأ أثناء إنشاء طلب الخروج. تأكد من أن الطالب حاضر اليوم");
            }

            return RedirectToAction(nameof(ExitRequest), new { classId });
        }

        // GET: Attendance/ViewAttendance/5
        public async Task<IActionResult> ViewAttendance(int classId, DateTime? date)
        {
            var selectedDate = date ?? DateTime.Today;
            var students = await _attendanceService.GetStudentsForAttendanceAsync(classId);

            // Get attendance for selected date
            // This would need additional methods in service for historical data

            ViewBag.ClassId = classId;
            ViewBag.SelectedDate = selectedDate;

            return View(students);
        }
    }
}

