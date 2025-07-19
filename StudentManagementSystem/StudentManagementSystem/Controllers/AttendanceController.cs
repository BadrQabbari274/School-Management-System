using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.ViewModels;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
using System.Security.Claims;

namespace StudentManagementSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ApplicationDbContext _context;

        public AttendanceController(IAttendanceService attendanceService, ApplicationDbContext context)
        {
            _attendanceService = attendanceService;
            _context = context;
        }

        // GET: Daily Attendance Page
        [HttpGet]
        public async Task<IActionResult> DailyAttendance(int? classId, DateTime? date)
        {
            var selectedDate = date ?? DateTime.Today;
            var classes = await _context.Classes.Where(c => c.IsActive).ToListAsync();

            ViewBag.Classes = classes;
            ViewBag.SelectedDate = selectedDate;
            ViewBag.SelectedClassId = classId;

            if (classId.HasValue)
            {
                var students = await _context.Students
                    .Where(s => s.ClassId == classId && s.IsActive)
                    .OrderBy(s => s.Code)
                    .ToListAsync();

                // Get existing attendance records for the day
                var existingAttendance = await _context.StudentAttendances
                    .Where(sa => sa.Date == selectedDate.Date &&
                                students.Select(s => s.Id).Contains(sa.StudentId.Value) &&
                                !sa.IsDeleted)
                    .ToListAsync();

                var viewModel = new DailyAttendanceViewModel
                {
                    ClassId = classId.Value,
                    Date = selectedDate,
                    Students = students.Select(s => new StudentAttendanceItem
                    {
                        StudentId = s.Id,
                        StudentName = s.Name,
                        StudentCode = s.Code,
                        IsPresent = existingAttendance.Any(ea => ea.StudentId == s.Id)
                    }).ToList()
                };

                return View(viewModel);
            }

            return View(new DailyAttendanceViewModel());
        }

        // POST: Save Daily Attendance
        [HttpPost]
        public async Task<IActionResult> SaveDailyAttendance(DailyAttendanceViewModel model)
        {
            try
            {
                var userId = GetCurrentUserId();

                // Remove existing attendance records for the day
                var existingRecords = await _context.StudentAttendances
                    .Where(sa => sa.Date == model.Date.Date &&
                                model.Students.Select(s => s.StudentId).Contains(sa.StudentId.Value))
                    .ToListAsync();

                _context.StudentAttendances.RemoveRange(existingRecords);

                // Add attendance records for present students
                foreach (var student in model.Students.Where(s => s.IsPresent))
                {
                    var attendance = new StudentAttendance
                    {
                        StudentId = student.StudentId,
                        Date = model.Date,
                        State = "Present",
                        CreatedBy = userId
                    };
                    _context.StudentAttendances.Add(attendance);
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حفظ الحضور اليومي بنجاح";

                return RedirectToAction("DailyAttendance", new { classId = model.ClassId, date = model.Date });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء حفظ البيانات";
                return View(model);
            }
        }

        // GET: Field Attendance Page
        [HttpGet]
        public async Task<IActionResult> FieldAttendance(int? classId, DateTime? date)
        {
            var selectedDate = date ?? DateTime.Today;
            var classes = await _context.Classes.Where(c => c.IsActive).ToListAsync();
            var absenceReasons = await _context.AbsenceReasons.Where(ar => !ar.IsDeleted).ToListAsync();

            ViewBag.Classes = classes;
            ViewBag.AbsenceReasons = absenceReasons;
            ViewBag.SelectedDate = selectedDate;
            ViewBag.SelectedClassId = classId;

            if (classId.HasValue)
            {
                var students = await _context.Students
                    .Where(s => s.ClassId == classId && s.IsActive)
                    .OrderBy(s => s.Code)
                    .ToListAsync();

                // Get daily attendance (present students)
                var dailyAttendance = await _context.StudentAttendances
                    .Where(sa => sa.Date == selectedDate.Date &&
                                students.Select(s => s.Id).Contains(sa.StudentId.Value) &&
                                !sa.IsDeleted)
                    .Select(sa => sa.StudentId.Value)
                    .ToListAsync();

                // Get existing field attendance records
                var existingFieldAttendance = await _context.StudentAbsents
                    .Include(sa => sa.AbsenceReason)
                    .Where(sa => sa.Date.Date == selectedDate.Date &&
                                students.Select(s => s.Id).Contains(sa.StudentId.Value) &&
                                !sa.IsDeleted)
                    .ToListAsync();

                var viewModel = new FieldAttendanceViewModel
                {
                    ClassId = classId.Value,
                    Date = selectedDate,
                    Students = students.Select(s =>
                    {
                        var isDailyPresent = dailyAttendance.Contains(s.Id);
                        var fieldRecord = existingFieldAttendance.FirstOrDefault(efa => efa.StudentId == s.Id);

                        return new StudentFieldAttendanceItem
                        {
                            StudentId = s.Id,
                            StudentName = s.Name,
                            StudentCode = s.Code,
                            IsDailyPresent = isDailyPresent,
                            IsFieldPresent = isDailyPresent && fieldRecord == null, // Present if daily present and no absent record
                            IsAbsent = !isDailyPresent || fieldRecord != null,
                            AbsenceReasonId = fieldRecord?.AbsenceReasonId,
                            CustomAbsenceReason = fieldRecord?.AbsenceReason?.Name == "Other" ? "Custom reason here" : null,
                            WithoutIncentive = fieldRecord?.AttendanceType == 1, // 1 = Without Incentive
                            CanDisableAttendance = isDailyPresent
                        };
                    }).ToList()
                };

                return View(viewModel);
            }

            return View(new FieldAttendanceViewModel());
        }

        // POST: Save Field Attendance
        [HttpPost]
        public async Task<IActionResult> SaveFieldAttendance(FieldAttendanceViewModel model)
        {
            try
            {
                var userId = GetCurrentUserId();

                // Remove existing field attendance records for the day
                var existingRecords = await _context.StudentAbsents
                    .Where(sa => sa.Date.Date == model.Date.Date &&
                                model.Students.Select(s => s.StudentId).Contains(sa.StudentId.Value))
                    .ToListAsync();

                _context.StudentAbsents.RemoveRange(existingRecords);

                // Process each student
                foreach (var student in model.Students)
                {
                    // If student is absent from field attendance
                    if (student.IsAbsent && student.IsDailyPresent)
                    {
                        var absent = new StudentAbsent
                        {
                            StudentId = student.StudentId,
                            Date = model.Date,
                            AbsenceReasonId = student.AbsenceReasonId,
                            AttendanceType = student.WithoutIncentive ? 1 : 0, // 1 = Without Incentive, 0 = Normal
                            CreatedBy = userId
                        };

                        // Handle custom absence reason (Other)
                        if (student.AbsenceReasonId == null && !string.IsNullOrEmpty(student.CustomAbsenceReason))
                        {
                            // Create or get "Other" reason
                            var otherReason = await _context.AbsenceReasons
                                .FirstOrDefaultAsync(ar => ar.Name == "Other");

                            if (otherReason == null)
                            {
                                otherReason = new AbsenceReason
                                {
                                    Name = "Other",
                                    CreatedBy = userId,
                                    CreatedDate = DateTime.Now
                                };
                                _context.AbsenceReasons.Add(otherReason);
                                await _context.SaveChangesAsync();
                            }

                            absent.AbsenceReasonId = otherReason.Id;
                        }

                        _context.StudentAbsents.Add(absent);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حفظ الحضور الميداني بنجاح";

                return RedirectToAction("FieldAttendance", new { classId = model.ClassId, date = model.Date });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء حفظ البيانات";
                return View(model);
            }
        }

        // GET: Exit Request Form
        [HttpGet]
        public async Task<IActionResult> CreateExitRequest(int studentId, DateTime date)
        {
            // Check if student is present
            var isPresent = await _context.StudentAttendances
                .AnyAsync(sa => sa.StudentId == studentId &&
                               sa.Date == date.Date &&
                               !sa.IsDeleted);

            if (!isPresent)
            {
                TempData["Error"] = "لا يمكن إنشاء طلب خروج للطالب غير الحاضر";
                return RedirectToAction("DailyAttendance");
            }

            var student = await _context.Students.FindAsync(studentId);
            var attendance = await _context.StudentAttendances
                .FirstOrDefaultAsync(sa => sa.StudentId == studentId && sa.Date == date.Date);

            var viewModel = new ExitRequestViewModel
            {
                StudentId = studentId,
                StudentName = student?.Name,
                StudentCode = student?.Code,
                AttendanceId = attendance?.Id,
                Date = date
            };

            return View(viewModel);
        }

        // POST: Create Exit Request
        [HttpPost]
        public async Task<IActionResult> CreateExitRequest(ExitRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = GetCurrentUserId();

                    var exitRequest = new RequestExit
                    {
                        Reason = model.Reason,
                        AttendanceId = model.AttendanceId,
                        Date = model.Date,
                        CreatedBy = userId
                    };

                    _context.RequestExits.Add(exitRequest);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "تم إنشاء طلب الخروج بنجاح";
                    return RedirectToAction("DailyAttendance");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "حدث خطأ أثناء إنشاء طلب الخروج";
                }
            }

            return View(model);
        }

        // Helper method to get current user ID
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 1; // Default to 1 if not found
        }
    }

    // View Models






}