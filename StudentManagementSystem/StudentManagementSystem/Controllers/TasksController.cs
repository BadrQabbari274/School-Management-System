using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service.Interface;
using System.Security.Claims;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    public class TaskController : BaseController
    {
        private readonly ITasksService _tasksService;

        public TaskController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            try
            {
                var tasks = await _tasksService.GetAllTasksAsync();
                return View(tasks);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return View(new List<Tasks>());
            }
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var task = await _tasksService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    SetErrorMessage("المهمة غير موجودة");
                    return RedirectToAction(nameof(Index));
                }
                return View(task);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Tasks/Create?learningOutcomeId=5
        public async Task<IActionResult> Create(int learningOutcomeId)
        {
            try
            {
                if (learningOutcomeId <= 0)
                {
                    SetErrorMessage("يجب تحديد مخرجات التعلم");
                    return RedirectToAction(nameof(Index));
                }

                // Check if learning outcome exists
                var learningOutcome = await _tasksService.GetLearningOutcomeByIdAsync(learningOutcomeId);
                if (learningOutcome == null)
                {
                    SetErrorMessage("مخرجات التعلم المحددة غير موجودة");
                    return RedirectToAction(nameof(Index));
                }

                // Get competency for this learning outcome
                var competency = await _tasksService.GetCompetencyByLearningOutcomeIdAsync(learningOutcomeId);
                if (competency == null)
                {
                    SetErrorMessage("لا توجد كفاءة مرتبطة بمخرجات التعلم المحددة");
                    return RedirectToAction(nameof(Index));
                }

                // Get evidences for this learning outcome
                var evidences = await _tasksService.GetEvidencesByLearningOutcomeIdAsync(learningOutcomeId);
                if (!evidences.Any())
                {
                    SetErrorMessage("لا توجد أدلة متاحة لمخرجات التعلم المحددة");
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.LearningOutcomeId = learningOutcomeId;
                ViewBag.LearningOutcomeName = learningOutcome.Name;
                ViewBag.CompetencyName = competency.Name;
                ViewBag.Evidences = evidences.ToList();

                var newTask = new Tasks();
                return View(newTask);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tasks task, int learningOutcomeId, int[] selectedEvidenceIds)
        {
            try
            {
                if (learningOutcomeId <= 0)
                {
                    SetErrorMessage("يجب تحديد مخرجات التعلم");
                    return RedirectToAction(nameof(Index));
                }

                if (selectedEvidenceIds == null || selectedEvidenceIds.Length == 0)
                {
                    ModelState.AddModelError("SelectedEvidences", "يجب اختيار دليل واحد على الأقل");
                }

                if (!ModelState.IsValid)
                {
                    var learningOutcome = await _tasksService.GetLearningOutcomeByIdAsync(learningOutcomeId);
                    var competency = await _tasksService.GetCompetencyByLearningOutcomeIdAsync(learningOutcomeId);
                    var evidences = await _tasksService.GetEvidencesByLearningOutcomeIdAsync(learningOutcomeId);

                    ViewBag.LearningOutcomeId = learningOutcomeId;
                    ViewBag.LearningOutcomeName = learningOutcome?.Name;
                    ViewBag.CompetencyName = competency?.Name;
                    ViewBag.Evidences = evidences.ToList();

                    return View(task);
                }

                // Get competency for this learning outcome
                var taskCompetency = await _tasksService.GetCompetencyByLearningOutcomeIdAsync(learningOutcomeId);

                // Set the task properties automatically
                task.CreatedBy_Id = GetCurrentUserId();
                task.CreatedDate = DateTime.Now;
                task.Competencies_Id = taskCompetency.Id;

                // Generate task name: LearningOutcomeId - EvidenceIds (comma separated)
                var evidenceIdsString = string.Join(",", selectedEvidenceIds.OrderBy(x => x));
                task.Name = $"{learningOutcomeId} - {evidenceIdsString}";

                var createdTask = await _tasksService.CreateTaskAsync(task);

                // Here you would need to handle the many-to-many relationship between Task and Evidence
                // This depends on your database schema - you might need a junction table
                // For now, I'll add a comment showing where this logic should go

                /* 
                 * If you have a junction table like Task_Evidence, you would do something like:
                 * foreach (var evidenceId in selectedEvidenceIds)
                 * {
                 *     await _tasksService.AddTaskEvidenceAsync(createdTask.Id, evidenceId);
                 * }
                 */

                SetSuccessMessage("تم إضافة المهمة بنجاح");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في حفظ البيانات: {ex.Message}");

                var learningOutcome = await _tasksService.GetLearningOutcomeByIdAsync(learningOutcomeId);
                var competency = await _tasksService.GetCompetencyByLearningOutcomeIdAsync(learningOutcomeId);
                var evidences = await _tasksService.GetEvidencesByLearningOutcomeIdAsync(learningOutcomeId);

                ViewBag.LearningOutcomeId = learningOutcomeId;
                ViewBag.LearningOutcomeName = learningOutcome?.Name;
                ViewBag.CompetencyName = competency?.Name;
                ViewBag.Evidences = evidences.ToList();

                return View(task);
            }
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var task = await _tasksService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    SetErrorMessage("المهمة غير موجودة");
                    return RedirectToAction(nameof(Index));
                }

                await PopulateViewBag();
                return View(task);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Tasks task)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await PopulateViewBag();
                    return View(task);
                }

                var updatedTask = await _tasksService.UpdateTaskAsync(task);

                if (updatedTask == null)
                {
                    SetErrorMessage("المهمة غير موجودة");
                    return RedirectToAction(nameof(Index));
                }

                SetSuccessMessage("تم تحديث المهمة بنجاح");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحديث البيانات: {ex.Message}");
                await PopulateViewBag();
                return View(task);
            }
        }

        // POST: Tasks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var task = await _tasksService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    SetErrorMessage("المهمة غير موجودة");
                    return RedirectToAction(nameof(Index));
                }

                var success = await _tasksService.DeleteTaskAsync(id);
                if (success)
                {
                    SetSuccessMessage($"تم حذف المهمة '{task.Name ?? "غير محدد"}' بنجاح");
                }
                else
                {
                    SetErrorMessage("فشل في حذف المهمة");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في حذف البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Tasks/GetTasksByCreator/5
        public async Task<IActionResult> GetTasksByCreator(int createdById)
        {
            try
            {
                var tasks = await _tasksService.GetTasksByCreatorIdAsync(createdById);
                ViewBag.CreatorId = createdById;
                return View("Index", tasks);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Tasks/GetTasksByCompetency/5
        public async Task<IActionResult> GetTasksByCompetency(int competencyId)
        {
            try
            {
                var tasks = await _tasksService.GetTasksByCompetencyIdAsync(competencyId);
                ViewBag.CompetencyId = competencyId;
                return View("Index", tasks);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"خطأ في تحميل البيانات: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Tasks/GetTasksJson (For AJAX calls)
        [HttpGet]
        public async Task<IActionResult> GetTasksJson()
        {
            try
            {
                var tasks = await _tasksService.GetAllTasksAsync();
                return Json(tasks.Select(t => new {
                    id = t.Id,
                    name = t.Name,
                    createdBy = t.CreatedBy?.Name,
                    competency = t.Competencies?.Name,
                    // Remove evidence reference since Tasks doesn't have direct Evidence relationship
                    createdDate = t.CreatedDate.ToString("yyyy-MM-dd")
                }));
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // AJAX method to get evidences by learning outcome
        [HttpGet]
        public async Task<IActionResult> GetEvidencesByLearningOutcome(int learningOutcomeId)
        {
            try
            {
                var evidences = await _tasksService.GetEvidencesByLearningOutcomeIdAsync(learningOutcomeId);
                return Json(evidences.Select(e => new {
                    value = e.Id,
                    text = e.Name
                }));
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // Helper method to populate ViewBag with dropdown data
        private async Task PopulateViewBag(int? learningOutcomeId = null)
        {
            try
            {
                var employees = await _tasksService.GetActiveEmployeesAsync();
                var competencies = await _tasksService.GetActiveCompetenciesAsync();

                ViewBag.CreatedBy_Id = new SelectList(employees, "Id", "Name");
                ViewBag.Competencies_Id = new SelectList(competencies, "Id", "Name");
            }
            catch (Exception)
            {
                ViewBag.CreatedBy_Id = new SelectList(new List<Employees>(), "Id", "Name");
                ViewBag.Competencies_Id = new SelectList(new List<Competencies>(), "Id", "Name");
            }
        }
    }
}