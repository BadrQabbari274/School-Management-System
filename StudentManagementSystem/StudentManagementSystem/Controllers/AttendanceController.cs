//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using StudentManagementSystem.Service.Interface;
//using StudentManagementSystem.Service.Implementation;
//using Microsoft.AspNetCore.Mvc.Rendering;

//namespace StudentManagementSystem.Controllers
//{
//    [Authorize]
//    public class AttendanceController : BaseController
//    {
//        private readonly IAttendanceService _attendanceService;

//        public AttendanceController(IAttendanceService attendanceService)
//        {
//            _attendanceService = attendanceService;
//        }

//        #region جلب الطلاب وعرض صفحة التسجيل

//        /// <summary>
//        /// عرض صفحة اختيار الفصل
//        /// </summary>
//        public async Task<IActionResult> Index()
//        {
//            await LoadClassesDropdown();
//            return View();
//        }

//        /// <summary>
//        /// جلب طلاب الفصل عبر Ajax
//        /// </summary>
//        [HttpGet]
//        public async Task<IActionResult> GetStudentsByClass(int classId)
//        {
//            try
//            {
//                var students = await _attendanceService.GetStudentsByClassAsync(classId);
//                return Json(new { success = true, data = students });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        #endregion

//        #region تسجيل الحضور والغياب العادي

//        /// <summary>
//        /// عرض صفحة تسجيل الحضور العادي
//        /// </summary>
//        [HttpGet]
//        public async Task<IActionResult> RecordRegular(int classId)
//        {
//            if (classId == 0)
//            {
//                SetErrorMessage("يجب اختيار فصل صحيح");
//                return RedirectToAction("Index");
//            }

//            try
//            {
//                var students = await _attendanceService.GetStudentsByClassAsync(classId);
//                if (!students.Any())
//                {
//                    SetWarningMessage("لا يوجد طلاب في هذا الفصل");
//                    return RedirectToAction("Index");
//                }

//                ViewBag.ClassId = classId;
//                ViewBag.ClassName = students.First().ClassName;
//                ViewBag.WorkingYearId = students.First().WorkingYearId;
//                ViewBag.SectionId = students.First().SectionId;
//                ViewBag.AttendanceType = "عادي (يومي)";

//                return View("RecordAttendance", students);
//            }
//            catch (Exception ex)
//            {
//                SetErrorMessage($"حدث خطأ: {ex.Message}");
//                return RedirectToAction("Index");
//            }
//        }

//        /// <summary>
//        /// حفظ الحضور العادي
//        /// </summary>
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> SaveRegularAttendance(RegularAttendanceDto model)
//        {
//            try
//            {
//                // التحقق من صحة التاريخ - لا يمكن تعديل تاريخ قديم إلا للأدمن
//                if (model.Date.Date < DateTime.Now.Date && !IsAdmin())
//                {
//                    return Json(new { success = false, message = "لا يمكن تسجيل حضور لتاريخ سابق. الأدمن فقط يمكنه ذلك." });
//                }

//                model.CreatedById = GetCurrentUserId();
//                var result = await _attendanceService.RecordRegularAttendanceAsync(model);

//                if (result)
//                {
//                    return Json(new { success = true, message = "تم حفظ الحضور بنجاح" });
//                }
//                else
//                {
//                    return Json(new { success = false, message = "فشل في حفظ الحضور" });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        #endregion

//        #region تسجيل الحضور والغياب الميداني

//        /// <summary>
//        /// عرض صفحة تسجيل الحضور الميداني
//        /// </summary>
//        [HttpGet]
//        public async Task<IActionResult> RecordField(int classId)
//        {
//            if (classId == 0)
//            {
//                SetErrorMessage("يجب اختيار فصل صحيح");
//                return RedirectToAction("Index");
//            }

//            try
//            {
//                var students = await _attendanceService.GetStudentsByClassAsync(classId);
//                if (!students.Any())
//                {
//                    SetWarningMessage("لا يوجد طلاب في هذا الفصل");
//                    return RedirectToAction("Index");
//                }

//                // جلب أسباب الغياب
//                var absenceReasons = await _attendanceService.GetAbsenceReasonsAsync();
//                ViewBag.AbsenceReasons = new SelectList(absenceReasons, "Id", "Name");

//                ViewBag.ClassId = classId;
//                ViewBag.ClassName = students.First().ClassName;
//                ViewBag.WorkingYearId = students.First().WorkingYearId;
//                ViewBag.SectionId = students.First().SectionId;
//                ViewBag.AttendanceType = "ميداني";
//                ViewBag.IsField = true;

//                return View("RecordAttendance", students);
//            }
//            catch (Exception ex)
//            {
//                SetErrorMessage($"حدث خطأ: {ex.Message}");
//                return RedirectToAction("Index");
//            }
//        }

//        /// <summary>
//        /// حفظ الحضور الميداني
//        /// </summary>
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> SaveFieldAttendance(FieldAttendanceDto model)
//        {
//            try
//            {
//                // التحقق من صحة التاريخ - لا يمكن تعديل تاريخ قديم إلا للأدمن
//                if (model.Date.Date < DateTime.Now.Date && !IsAdmin())
//                {
//                    return Json(new { success = false, message = "لا يمكن تسجيل حضور لتاريخ سابق. الأدمن فقط يمكنه ذلك." });
//                }

//                model.CreatedById = GetCurrentUserId();

//                // التحقق من وجود غياب عادي للطلاب وتطبيق القواعد
//                await ApplyFieldAttendanceRules(model);

//                var result = await _attendanceService.RecordFieldAttendanceAsync(model);

//                if (result)
//                {
//                    return Json(new { success = true, message = "تم حفظ الحضور الميداني بنجاح" });
//                }
//                else
//                {
//                    return Json(new { success = false, message = "فشل في حفظ الحضور الميداني" });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        /// <summary>
//        /// جلب حالة الطلاب للحضور الميداني مع تطبيق القواعد
//        /// </summary>
//        [HttpGet]
//        public async Task<IActionResult> GetStudentsForFieldAttendance(int classId, DateTime date)
//        {
//            try
//            {
//                var students = await _attendanceService.GetStudentsByClassAsync(classId);

//                // جلب الحضور العادي لنفس اليوم
//                var regularAttendance = await _attendanceService.GetAttendanceRecordsByDateAsync(classId, date, true);

//                var studentsWithStatus = students.Select(s => new
//                {
//                    StudentId = s.StudentId,
//                    StudentName = s.StudentName,
//                    StudentCode = s.StudentCode,
//                    Gender = s.Gender,
//                    IsRegularAbsent = regularAttendance?.Records?.Any(r => r.StudentId == s.StudentId && !r.IsPresent) ?? false,
//                    CanEdit = !(regularAttendance?.Records?.Any(r => r.StudentId == s.StudentId && !r.IsPresent) ?? false)
//                }).ToList();

//                return Json(new { success = true, data = studentsWithStatus });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        #endregion

//        #region عرض سجلات الحضور والغياب

//        /// <summary>
//        /// عرض صفحة استعلام الحضور
//        /// </summary>
//        [HttpGet]
//        public async Task<IActionResult> ViewRecords()
//        {
//            await LoadClassesDropdown();
//            return View();
//        }

//        /// <summary>
//        /// عرض سجلات الحضور لتاريخ معين
//        /// </summary>
//        [HttpGet]
//        public async Task<IActionResult> GetAttendanceRecords(int classId, DateTime date, bool isRegular = true)
//        {
//            try
//            {
//                var records = await _attendanceService.GetAttendanceRecordsByDateAsync(classId, date, isRegular);

//                if (records == null)
//                {
//                    return Json(new { success = false, message = "لا توجد بيانات للعرض" });
//                }

//                // إضافة معلومة إمكانية التعديل
//                //foreach (var record in records.Records)
//                //{
//                //    record.CanEdit = date.Date >= DateTime.Now.Date || IsAdmin();
//                //}

//                return Json(new { success = true, data = records });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        #endregion

//        #region تعديل سجلات الحضور

//        /// <summary>
//        /// تعديل سجل حضور طالب
//        /// </summary>
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> UpdateAttendance([FromBody] UpdateAttendanceDto model)
//        {
//            try
//            {
//                // التحقق من صحة التاريخ - لا يمكن تعديل تاريخ قديم إلا للأدمن
//                if (model.Date.Date < DateTime.Now.Date && !IsAdmin())
//                {
//                    return Json(new { success = false, message = "لا يمكن تعديل حضور لتاريخ سابق. الأدمن فقط يمكنه ذلك." });
//                }

//                model.CreatedById = GetCurrentUserId();
//                var result = await _attendanceService.UpdateAttendanceRecordAsync(model);

//                if (result)
//                {
//                    return Json(new { success = true, message = "تم تحديث السجل بنجاح" });
//                }
//                else
//                {
//                    return Json(new { success = false, message = "فشل في تحديث السجل" });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        #endregion

//        #region تقارير الطلاب الفردية

//        /// <summary>
//        /// عرض صفحة تقارير الطلاب
//        /// </summary>
//        [HttpGet]
//        public async Task<IActionResult> StudentReports()
//        {
//            await LoadClassesDropdown();
//            return View();
//        }

//        /// <summary>
//        /// جلب تقرير طالب معين
//        /// </summary>
//        [HttpGet]
//        public async Task<IActionResult> GetStudentReport(int studentId, DateTime fromDate, DateTime toDate)
//        {
//            try
//            {
//                var report = await _attendanceService.GetStudentAttendanceReportAsync(studentId, fromDate, toDate);

//                if (report == null)
//                {
//                    return Json(new { success = false, message = "لا توجد بيانات للطالب المحدد" });
//                }

//                return Json(new { success = true, data = report });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        #endregion

//        #region طلبات الخروج

//        /// <summary>
//        /// عرض صفحة طلبات الخروج
//        /// </summary>
//        [HttpGet]
//        public async Task<IActionResult> ExitRequests()
//        {
//            await LoadClassesDropdown();
//            return View();
//        }

//        /// <summary>
//        /// إنشاء طلب خروج
//        /// </summary>
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> CreateExitRequest([FromBody] ExitRequestCreateDto model)
//        {
//            try
//            {
//                model.CreatedById = GetCurrentUserId();
//                var result = await _attendanceService.CreateExitRequestAsync(model);

//                if (result)
//                {
//                    return Json(new { success = true, message = "تم إنشاء طلب الخروج بنجاح" });
//                }
//                else
//                {
//                    return Json(new { success = false, message = "فشل في إنشاء طلب الخروج. تأكد من أن الطالب حاضر ولا يوجد طلب خروج مسبق" });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        #endregion

//        #region تقارير الفصول

//        /// <summary>
//        /// عرض صفحة تقارير الفصول
//        /// </summary>
//        [HttpGet]
//        public async Task<IActionResult> ClassReports()
//        {
//            await LoadClassesDropdown();
//            return View();
//        }

//        /// <summary>
//        /// جلب تقرير فصل معين
//        /// </summary>
//        [HttpGet]
//        public async Task<IActionResult> GetClassReport(int classId, DateTime fromDate, DateTime toDate)
//        {
//            try
//            {
//                var report = await _attendanceService.GetClassAttendanceReportAsync(classId, fromDate, toDate);

//                if (report == null)
//                {
//                    return Json(new { success = false, message = "لا توجد بيانات للفصل المحدد" });
//                }

//                return Json(new { success = true, data = report });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        #endregion

//        #region Helper Methods

//        /// <summary>
//        /// تحميل قائمة الفصول
//        /// </summary>
//        private async Task LoadClassesDropdown()
//        {
//            // يجب إنشاء خدمة للفصول أو استخدام DbContext مباشرة
//            // هذا مثال مؤقت - يجب تعديله حسب التطبيق
//            ViewBag.Classes = new SelectList(new List<object>(), "Id", "Name");
//        }

//        /// <summary>
//        /// تطبيق قواعد الحضور الميداني
//        /// </summary>
//        private async Task ApplyFieldAttendanceRules(FieldAttendanceDto model)
//        {
//            // جلب الحضور العادي لنفس اليوم
//            var regularAttendance = await _attendanceService.GetAttendanceRecordsByDateAsync(
//                model.ClassId, model.Date, true);

//            if (regularAttendance?.Records != null)
//            {
//                foreach (var studentRecord in model.StudentsAttendance)
//                {
//                    var regularRecord = regularAttendance.Records
//                        .FirstOrDefault(r => r.StudentId == studentRecord.StudentId);

//                    // إذا كان الطالب غائباً في الحضور العادي، يسجل تلقائياً كغائب ميداني
//                    if (regularRecord != null && !regularRecord.IsPresent)
//                    {
//                        studentRecord.IsPresent = false;
//                        studentRecord.CustomReasonDetails = "غائب من الحضور اليومي";
//                    }
//                }
//            }
//        }

//        #endregion
//    }
//}