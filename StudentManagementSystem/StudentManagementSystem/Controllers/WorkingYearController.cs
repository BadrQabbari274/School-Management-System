using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Service;

namespace StudentManagementSystem.Controllers
{
    public class WorkingYearController : BaseController
    {
        private readonly IWorkingYearService _workingYearService;

        public WorkingYearController(IWorkingYearService workingYearService)
        {
            _workingYearService = workingYearService;
        }

        // GET: WorkingYear
        public async Task<IActionResult> Index()
        {
            var workingYears = await _workingYearService.GetAllWorkingYearsAsync();
            return View(workingYears);
        }

        // GET: WorkingYear/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workingYear = await _workingYearService.GetWorkingYearByIdAsync(id.Value);
            if (workingYear == null)
            {
                return NotFound();
            }

            return View(workingYear);
        }

        // GET: WorkingYear/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkingYear/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Working_Year workingYear)
        {
            workingYear.CreatedBy_Id =GetCurrentUserId();
                  // Check if name is unique
                  var isUnique = await _workingYearService.IsWorkingYearNameUniqueAsync(workingYear.Name);
                if (!isUnique)
                {
                    ModelState.AddModelError("Name", "اسم السنة العملية موجود بالفعل.");
                    return View(workingYear);
                }

                await _workingYearService.CreateWorkingYearAsync(workingYear);
                TempData["SuccessMessage"] = "تم إنشاء السنة العملية بنجاح.";
                return RedirectToAction(nameof(Index));
 
        }

        // GET: WorkingYear/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workingYear = await _workingYearService.GetWorkingYearByIdAsync(id.Value);
            if (workingYear == null)
            {
                return NotFound();
            }
            return View(workingYear);
        }

        // POST: WorkingYear/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Start_date,End_date,IsActive,CreatedBy_Id,Date")] Working_Year workingYear)
        {
            if (id != workingYear.Id)
            {
                return NotFound();
            }


                try
                {
                    // Check if name is unique (excluding current record)
                    var isUnique = await _workingYearService.IsWorkingYearNameUniqueAsync(workingYear.Name, workingYear.Id);
                    if (!isUnique)
                    {
                        ModelState.AddModelError("Name", "اسم السنة العملية موجود بالفعل.");
                        return View(workingYear);
                    }

                    await _workingYearService.UpdateWorkingYearAsync(workingYear);
                    TempData["SuccessMessage"] = "تم تحديث السنة العملية بنجاح.";
                }
                catch (Exception)
                {
                    if (!await _workingYearService.WorkingYearExistsAsync(workingYear.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));

        }

        // GET: WorkingYear/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workingYear = await _workingYearService.GetWorkingYearByIdAsync(id.Value);
            if (workingYear == null)
            {
                return NotFound();
            }

            return View(workingYear);
        }

        // POST: WorkingYear/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _workingYearService.DeleteWorkingYearAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "تم حذف السنة العملية بنجاح.";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف السنة العملية.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}