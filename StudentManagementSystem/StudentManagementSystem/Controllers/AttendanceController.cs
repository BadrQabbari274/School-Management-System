using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class AttendanceController : BaseController
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IClassService _classService;
        private readonly IAbsenceReasonService _absenceReasonService;

        public AttendanceController(
            IAttendanceService attendanceService,
            IClassService classService,
            IAbsenceReasonService absenceReasonService)
        {
            _attendanceService = attendanceService;
            _classService = classService;
            _absenceReasonService = absenceReasonService;
        }

        #region Daily Attendance

        [HttpGet]
        public async Task<IActionResult> DailyAttendance(int? classId, DateTime? date)
        {
            // Default to today if no date specified (excluding Fridays and Saturdays)
            var selectedDate = date ?? GetNextValidDate(DateTime.Today);

            // Get all active classes
            var classes = await _classService.GetActiveClassesAsync();

            // If no class selected, default to first class
            var selectedClass = classId.HasValue
                ? await _classService.GetClassByIdAsync(classId.Value)
                : classes.FirstOrDefault();

            if (selectedClass == null)
            {
                SetErrorMessage("No active classes found");
                return RedirectToHome();
            }

            // Get students for the selected class
            var students = await _classService.GetClassByIdAsync(selectedClass.Id);

            // Get existing attendance records for the date
            var existingAttendance = await _attendanceService.GetDailyAttendanceByClassAndDateAsync(selectedClass.Id, selectedDate);

            var model = new DailyAttendanceViewModel
            {
                Classes = classes,
                SelectedClassId = selectedClass.Id,
                SelectedDate = selectedDate,
                Students = students.Students.Select(s => new StudentAttendanceViewModel
                {
                    StudentId = s.Id,
                    StudentName = s.Name,
                    StudentCode = s.Code,
                    IsPresent = existingAttendance.Any(a => a.StudentId == s.Id && a.State == "Present")
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDailyAttendance(DailyAttendanceViewModel model)
        {
            if (model.SelectedDate.DayOfWeek == DayOfWeek.Friday || model.SelectedDate.DayOfWeek == DayOfWeek.Saturday)
            {
                SetErrorMessage("Attendance cannot be recorded on Fridays or Saturdays");
                return RedirectToAction(nameof(DailyAttendance), new { classId = model.SelectedClassId, date = model.SelectedDate });
            }

            var presentStudentIds = model.Students
                .Where(s => s.IsPresent)
                .Select(s => s.StudentId)
                .ToList();

            var success = await _attendanceService.SaveDailyAttendanceBulkAsync(
                model.SelectedClassId,
                model.SelectedDate,
                presentStudentIds,
                GetCurrentUserId());

            if (success)
            {
                SetSuccessMessage("Daily attendance saved successfully");
            }
            else
            {
                SetErrorMessage("Failed to save daily attendance");
            }

            return RedirectToAction(nameof(DailyAttendance), new { classId = model.SelectedClassId, date = model.SelectedDate });
        }

        #endregion

        #region Field Attendance

        [HttpGet]
        public async Task<IActionResult> FieldAttendance(int? classId, DateTime? date)
        {
            // Default to today if no date specified (excluding Fridays and Saturdays)
            var selectedDate = date ?? GetNextValidDate(DateTime.Today);

            // Get classes that have field attendance enabled
            var classes = (await _classService.GetActiveClassesAsync())
                .ToList();

            // If no class selected, default to first class
            var selectedClass = classId.HasValue
                ? await _classService.GetClassByIdAsync(classId.Value)
                : classes.FirstOrDefault();

            if (selectedClass == null)
            {
                SetErrorMessage("No classes with field attendance found");
                return RedirectToHome();
            }

            // Get students for the selected class
            var students = await _classService.GetClassByIdAsync(selectedClass.Id);

            // Get existing attendance records to check who was present in daily attendance
            var dailyAttendance = await _attendanceService.GetDailyAttendanceByClassAndDateAsync(selectedClass.Id, selectedDate);

            // Get existing field attendance records
            var existingFieldAttendance = await _attendanceService.GetFieldAttendanceByClassAndDateAsync(selectedClass.Id, selectedDate);

            // Get absence reasons
            var absenceReasons = await _absenceReasonService.GetActiveAbsenceReasonsAsync();

            var model = new FieldAttendanceViewModel
            {
                Classes = classes,
                SelectedClassId = selectedClass.Id,
                SelectedDate = selectedDate,
                AbsenceReasons = absenceReasons,
                Students = students.Students.Select(s => new FieldAttendanceStudentViewModel
                {
                    StudentId = s.Id,
                    StudentName = s.Name,
                    StudentCode = s.Code,
                    WasPresentDaily = dailyAttendance.Any(a => a.StudentId == s.Id && a.State == "Present"),
                    IsAbsent = existingFieldAttendance.Any(a => a.StudentId == s.Id),
                    AbsenceReasonId = existingFieldAttendance.FirstOrDefault(a => a.StudentId == s.Id)?.AbsenceReasonId,
                    CustomAbsenceReason = existingFieldAttendance.FirstOrDefault(a => a.StudentId == s.Id)?.CustomReasonDetails,
                    WithoutIncentive = existingFieldAttendance.FirstOrDefault(a => a.StudentId == s.Id)?.AttendanceType == 1
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveFieldAttendance(FieldAttendanceViewModel model)
        {
            if (model.SelectedDate.DayOfWeek == DayOfWeek.Friday || model.SelectedDate.DayOfWeek == DayOfWeek.Saturday)
            {
                SetErrorMessage("Attendance cannot be recorded on Fridays or Saturdays");
                return RedirectToAction(nameof(FieldAttendance), new { classId = model.SelectedClassId, date = model.SelectedDate });
            }

            // Prepare field attendance records
            var fieldRecords = model.Students
                .Where(s => s.IsAbsent)
                .Select(s => new FieldAttendanceRecord
                {
                    StudentId = s.StudentId,
                    IsAbsent = true,
                    AbsenceReasonId = s.AbsenceReasonId,
                    CustomAbsenceReason = s.CustomAbsenceReason,
                    WithoutIncentive = s.WithoutIncentive
                }).ToList();

            // Validate records
            foreach (var record in fieldRecords)
            {
                if (record.AbsenceReasonId == null)
                {
                    SetErrorMessage("Absence reason is required for all absent students");
                    return RedirectToAction(nameof(FieldAttendance), new { classId = model.SelectedClassId, date = model.SelectedDate });
                }

                // If "Other" reason is selected, custom reason is required
                var otherReason = await _attendanceService.GetOrCreateOtherReasonAsync(GetCurrentUserId());
                if (record.AbsenceReasonId == otherReason.Id && string.IsNullOrWhiteSpace(record.CustomAbsenceReason))
                {
                    SetErrorMessage("Custom reason is required when 'Other' is selected");
                    return RedirectToAction(nameof(FieldAttendance), new { classId = model.SelectedClassId, date = model.SelectedDate });
                }
            }

            var success = await _attendanceService.SaveFieldAttendanceBulkAsync(
                model.SelectedClassId,
                model.SelectedDate,
                fieldRecords,
                GetCurrentUserId());

            if (success)
            {
                SetSuccessMessage("Field attendance saved successfully");
            }
            else
            {
                SetErrorMessage("Failed to save field attendance");
            }

            return RedirectToAction(nameof(FieldAttendance), new { classId = model.SelectedClassId, date = model.SelectedDate });
        }

        #endregion

        #region Exit Requests

        [HttpGet]
        public async Task<IActionResult> CreateExitRequest(int studentId)
        {
            // Check if student was present today
            var isPresent = await _attendanceService.IsStudentPresentAsync(studentId, DateTime.Today);
            if (!isPresent)
            {
                SetErrorMessage("Cannot create exit request for absent student");
                return RedirectToAction("DailyAttendance");
            }

            var model = new ExitRequestViewModel
            {
                StudentId = studentId,
                ExitTime = DateTime.Now.TimeOfDay
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExitRequest(ExitRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if student was present today
            var isPresent = await _attendanceService.IsStudentPresentAsync(model.StudentId, DateTime.Today);
            if (!isPresent)
            {
                SetErrorMessage("Cannot create exit request for absent student");
              return RedirectToAction("DailyAttendance");
            }

            var exitRequest = new RequestExit
            {
                StudentId = model.StudentId,
                ExitTime = model.ExitTime,
                Reason = model.Reason,
                CreatedBy = GetCurrentUserId(),
                Status = 0 // Pending
            };

            var success = await _attendanceService.CreateExitRequestAsync(exitRequest);

            //if (success)
            //{
                SetSuccessMessage("Exit request submitted successfully");
                return RedirectToAction(nameof(ExitRequests));
           

            //SetErrorMessage("Failed to submit exit request");
            //return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ExitRequests(DateTime? date)
        {
            var selectedDate = date ?? DateTime.Today;
            var requests = await _attendanceService.GetExitRequestsByDateAsync(selectedDate);

            return View(new ExitRequestsViewModel
            {
                SelectedDate = selectedDate,
                Requests = requests
            });
        }

        [HttpPost]
        public async Task<IActionResult> ProcessExitRequest(int exitRequestId, int status, string notes)
        {
            var success = await _attendanceService.ProcessExitRequestAsync(
                exitRequestId,
                status,
                GetCurrentUserId(),
                notes);

            if (success)
            {
                SetSuccessMessage("Exit request processed successfully");
            }
            else
            {
                SetErrorMessage("Failed to process exit request");
            }

            return RedirectToAction(nameof(ExitRequests));
        }

        #endregion

        #region Reports

        [HttpGet]
        public async Task<IActionResult> DailyReport(int? classId, DateTime? date)
        {
            var selectedDate = date ?? DateTime.Today;
            Class selectedClass = null;

            if (classId.HasValue)
            {
                selectedClass = await _classService.GetClassByIdAsync(classId.Value);
            }
            else
            {
                var classes = await _classService.GetActiveClassesAsync();
                selectedClass = classes.FirstOrDefault();
            }

            if (selectedClass == null)
            {
                SetErrorMessage("No classes found");
                return RedirectToHome();
            }

            var report = await _attendanceService.GetClassAttendanceReportAsync(selectedClass.Id, selectedDate);

            return View(report);
        }

        [HttpGet]
        public async Task<IActionResult> WeeklyReport(int? classId, DateTime? startDate)
        {
            var selectedStartDate = startDate ?? GetStartOfWeek(DateTime.Today);
            var selectedEndDate = selectedStartDate.AddDays(6);
            Class selectedClass = null;

            if (classId.HasValue)
            {
                selectedClass = await _classService.GetClassByIdAsync(classId.Value);
            }
            else
            {
                var classes = await _classService.GetActiveClassesAsync();
                selectedClass = classes.FirstOrDefault();
            }

            if (selectedClass == null)
            {
                SetErrorMessage("No classes found");
                return RedirectToHome();
            }

            var report = await _attendanceService.GetClassAttendanceReportByDateRangeAsync(
                selectedClass.Id,
                selectedStartDate,
                selectedEndDate);

            return View(report);
        }

        [HttpGet]
        public async Task<IActionResult> StudentReport(int studentId, DateTime? startDate, DateTime? endDate)
        {
            var selectedStartDate = startDate ?? DateTime.Today.AddDays(-30);
            var selectedEndDate = endDate ?? DateTime.Today;

            var report = await _attendanceService.GetStudentAttendanceSummaryAsync(
                studentId,
                selectedStartDate,
                selectedEndDate);

            return View(report);
        }

        #endregion

        #region Helper Methods

        private DateTime GetNextValidDate(DateTime date)
        {
            // Skip Fridays and Saturdays
            while (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(1);
            }
            return date;
        }

        private DateTime GetStartOfWeek(DateTime date)
        {
            // Our week starts on Sunday
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        #endregion
    }

    #region View Models

    public class DailyAttendanceViewModel
    {
        public IEnumerable<Class> Classes { get; set; }
        public int SelectedClassId { get; set; }
        public DateTime SelectedDate { get; set; }
        public List<StudentAttendanceViewModel> Students { get; set; }
    }

    public class StudentAttendanceViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public bool IsPresent { get; set; }
    }

    public class FieldAttendanceViewModel
    {
        public IEnumerable<Class> Classes { get; set; }
        public int SelectedClassId { get; set; }
        public DateTime SelectedDate { get; set; }
        public IEnumerable<AbsenceReason> AbsenceReasons { get; set; }
        public List<FieldAttendanceStudentViewModel> Students { get; set; }
    }

    public class FieldAttendanceStudentViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public bool WasPresentDaily { get; set; }
        public bool IsAbsent { get; set; }
        public int? AbsenceReasonId { get; set; }
        public string CustomAbsenceReason { get; set; }
        public bool WithoutIncentive { get; set; }
    }

    public class ExitRequestViewModel
    {
        public int StudentId { get; set; }
        public TimeSpan ExitTime { get; set; }
        public string Reason { get; set; }
    }

    public class ExitRequestsViewModel
    {
        public DateTime SelectedDate { get; set; }
        public IEnumerable<RequestExit> Requests { get; set; }
    }

    #endregion
}