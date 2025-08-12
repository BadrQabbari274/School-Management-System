using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class EvidenceController : BaseController
    {
        private readonly IEvidenceService _evidenceService;

        public EvidenceController(IEvidenceService evidenceService)
        {
            _evidenceService = evidenceService;
        }

        // GET: Evidence
        public async Task<IActionResult> Index()
        {
            try
            {
                var evidences = await _evidenceService.GetAllEvidencesAsync();
                return View(evidences);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return View(new List<Evidence>());
            }
        }

        // GET: Evidence/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var evidence = await _evidenceService.GetEvidenceByIdAsync(id);
                if (evidence == null)
                {
                    SetErrorMessage("الدليل غير موجود");
                    return RedirectToAction(nameof(Index));
                }
                return View(evidence);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Evidence/Create
        public IActionResult Create()
        {
            var evidence = new Evidence();
            return View(evidence);
        }

        // POST: Evidence/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evidence evidence)
        {
            try
            {
                // Set the created by user
                evidence.CreatedBy_Id = GetCurrentUserId();
                evidence.CreatedDate = DateTime.Now;
                evidence.IsActive = true;

                await _evidenceService.CreateEvidenceAsync(evidence);
                SetSuccessMessage("تم إضافة الدليل بنجاح");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في إضافة الدليل: {ex.Message}");
                return View(evidence);
            }
        }

        // GET: Evidence/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var evidence = await _evidenceService.GetEvidenceByIdAsync(id);
                if (evidence == null)
                {
                    SetErrorMessage("الدليل غير موجود");
                    return RedirectToAction(nameof(Index));
                }
                return View(evidence);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Evidence/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Evidence evidence)
        {
            if (id != evidence.Id)
            {
                SetErrorMessage("خطأ في البيانات المرسلة");
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Get the existing evidence to preserve some properties
                var existingEvidence = await _evidenceService.GetEvidenceByIdAsync(id);
                if (existingEvidence == null)
                {
                    SetErrorMessage("الدليل غير موجود");
                    return RedirectToAction(nameof(Index));
                }

                // Update only the allowed properties
                existingEvidence.Name = evidence.Name;
                existingEvidence.Ispractical = evidence.Ispractical;
                existingEvidence.IsActive = evidence.IsActive;
                existingEvidence.Outcome_Id = evidence.Outcome_Id;
                // Keep the original creation date and creator

                await _evidenceService.UpdateEvidenceAsync(id, existingEvidence);
                SetSuccessMessage("تم تحديث الدليل بنجاح");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحديث الدليل: {ex.Message}");
                return View(evidence);
            }
        }

        // POST: Evidence/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _evidenceService.DeleteEvidenceAsync(id);
                if (result)
                {
                    SetSuccessMessage("تم حذف الدليل بنجاح");
                }
                else
                {
                    SetErrorMessage("الدليل غير موجود");
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في حذف الدليل: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Evidence/GetActiveEvidences
        [HttpGet]
        public async Task<IActionResult> GetActiveEvidences()
        {
            try
            {
                var evidences = await _evidenceService.GetActiveEvidencesAsync();
                return Json(evidences.Select(e => new {
                    Id = e.Id,
                    Name = e.Name,
                    Ispractical = e.Ispractical,
                    OutcomeName = e.Learning_Outcome?.Name ?? "",
                    CreatedDate = e.CreatedDate.ToString("dd/MM/yyyy")
                }));
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // GET: Evidence/GetEvidencesByOutcome/5
        [HttpGet]
        public async Task<IActionResult> GetEvidencesByOutcome(int outcomeId)
        {
            try
            {
                var evidences = await _evidenceService.GetEvidencesByOutcomeIdAsync(outcomeId);
                return Json(evidences.Select(e => new {
                    Id = e.Id,
                    Name = e.Name,
                    Ispractical = e.Ispractical,
                    IsActive = e.IsActive
                }));
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // GET: Evidence/GetPracticalEvidences
        [HttpGet]
        public async Task<IActionResult> GetPracticalEvidences()
        {
            try
            {
                var evidences = await _evidenceService.GetPracticalEvidencesAsync();
                return Json(evidences.Select(e => new {
                    Id = e.Id,
                    Name = e.Name,
                    OutcomeName = e.Learning_Outcome?.Name ?? "",
                    CreatedDate = e.CreatedDate.ToString("dd/MM/yyyy")
                }));
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // POST: Evidence/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var evidence = await _evidenceService.GetEvidenceByIdAsync(id);
                if (evidence == null)
                {
                    SetErrorMessage("الدليل غير موجود");
                    return RedirectToAction(nameof(Index));
                }

                evidence.IsActive = !evidence.IsActive;
                await _evidenceService.UpdateEvidenceAsync(id, evidence);

                string status = evidence.IsActive ? "مفعل" : "معطل";
                SetSuccessMessage($"تم تغيير حالة الدليل إلى {status}");
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تغيير حالة الدليل: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}