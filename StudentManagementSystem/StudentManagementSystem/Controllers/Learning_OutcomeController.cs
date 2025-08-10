using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Security.Claims;

namespace StudentManagementSystem.Controllers
{
    public class Learning_OutcomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Learning_OutcomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Learning_Outcome
        public async Task<IActionResult> Index()
        {
            var learningOutcomes = _context.Outcomes
                .Include(l => l.CreatedBy)
                .Include(l => l.Competency)
                .Where(l => l.IsActive);
            return View(await learningOutcomes.ToListAsync());
        }

        // GET: Learning_Outcome/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningOutcome = await _context.Outcomes
                .Include(l => l.CreatedBy)
                .Include(l => l.Competency)
                .Include(l => l.Evidences)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (learningOutcome == null)
            {
                return NotFound();
            }

            return View(learningOutcome);
        }

        // GET: Learning_Outcome/Create
        public IActionResult Create()
        {
            ViewData["Competency_Id"] = new SelectList(_context.Competencies, "Id", "Name");
            // We no longer need to populate the CreatedBy_Id dropdown here
            return View();
        }
        // POST: Learning_Outcome/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Competency_Id,IsActive")] Learning_Outcome learningOutcome)
        {
            try
            {
                // ≈“«·… validation errors €Ì— «·„ÿ·Ê»…
                ModelState.Remove("Id");
                ModelState.Remove("CreatedDate");
                ModelState.Remove("CreatedBy");
                ModelState.Remove("Competency");
                ModelState.Remove("Evidences");
                // Remove the CreatedBy_Id from ModelState since we will set it manually
                ModelState.Remove("CreatedBy_Id");

                if (ModelState.IsValid)
                {
                    // Get the logged-in user's ID
                    var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    // Check if the user ID is not null and can be parsed as an integer
                    if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
                    {
                        learningOutcome.CreatedBy_Id = userId;
                        learningOutcome.CreatedDate = DateTime.Now;
                        learningOutcome.IsActive = true;

                        _context.Outcomes.Add(learningOutcome);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = " „ ≈‰‘«¡ „Œ—Ã «· ⁄·„ »‰Ã«Õ";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "ÕœÀ Œÿ√: ·„ Ì „ «·⁄ÀÊ— ⁄·Ï „⁄·Ê„«  «·„” Œœ„ √Ê √‰Â« €Ì— ’«·Õ….";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    // ⁄—÷ √Œÿ«¡ «·‹ validation ·· ‘ŒÌ’
                    foreach (var modelError in ModelState)
                    {
                        if (modelError.Value.Errors.Count > 0)
                        {
                            foreach (var error in modelError.Value.Errors)
                            {
                                System.Diagnostics.Debug.WriteLine($"Validation Error in {modelError.Key}: {error.ErrorMessage}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // «· ⁄«„· „⁄ «·√Œÿ«¡
                TempData["ErrorMessage"] = "ÕœÀ Œÿ√ √À‰«¡ «·Õ›Ÿ: " + ex.Message;
                System.Diagnostics.Debug.WriteLine($"Error saving: {ex.Message}");
            }

            ViewData["Competency_Id"] = new SelectList(_context.Competencies, "Id", "Name", learningOutcome.Competency_Id);
            return View(learningOutcome);
        }
        // GET: Learning_Outcome/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningOutcome = await _context.Outcomes.FindAsync(id);
            if (learningOutcome == null)
            {
                return NotFound();
            }
            ViewData["Competency_Id"] = new SelectList(_context.Competencies, "Id", "Name", learningOutcome.Competency_Id);
            ViewData["CreatedBy_Id"] = new SelectList(_context.Employees, "Id", "Name", learningOutcome.CreatedBy_Id);
            return View(learningOutcome);
        }

        // POST: Learning_Outcome/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Competency_Id,IsActive,CreatedBy_Id,CreatedDate")] Learning_Outcome learningOutcome)
        {
            if (id != learningOutcome.Id)
            {
                return NotFound();
            }

            try
            {
                // ≈“«·… validation errors €Ì— «·„ÿ·Ê»…
                ModelState.Remove("CreatedBy");
                ModelState.Remove("Competency");
                ModelState.Remove("Evidences");

                if (ModelState.IsValid)
                {
                    _context.Outcomes.Update(learningOutcome);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = " „  ⁄œÌ· „Œ—Ã «· ⁄·„ »‰Ã«Õ";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // ⁄—÷ √Œÿ«¡ «·‹ validation ·· ‘ŒÌ’
                    foreach (var modelError in ModelState)
                    {
                        if (modelError.Value.Errors.Count > 0)
                        {
                            foreach (var error in modelError.Value.Errors)
                            {
                                System.Diagnostics.Debug.WriteLine($"Validation Error in {modelError.Key}: {error.ErrorMessage}");
                            }
                        }
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Learning_OutcomeExists(learningOutcome.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "ÕœÀ Œÿ√ √À‰«¡ «· ⁄œÌ·: " + ex.Message;
                System.Diagnostics.Debug.WriteLine($"Error updating: {ex.Message}");
            }

            ViewData["Competency_Id"] = new SelectList(_context.Competencies, "Id", "Name", learningOutcome.Competency_Id);
            ViewData["CreatedBy_Id"] = new SelectList(_context.Employees, "Id", "Name", learningOutcome.CreatedBy_Id);
            return View(learningOutcome);
        }

        // GET: Learning_Outcome/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningOutcome = await _context.Outcomes
                .Include(l => l.CreatedBy)
                .Include(l => l.Competency)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (learningOutcome == null)
            {
                return NotFound();
            }

            return View(learningOutcome);
        }

        // POST: Learning_Outcome/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learningOutcome = await _context.Outcomes.FindAsync(id);
            if (learningOutcome != null)
            {
                // Soft delete
                learningOutcome.IsActive = false;
                _context.Update(learningOutcome);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = " „ Õ–› „Œ—Ã «· ⁄·„ »‰Ã«Õ";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool Learning_OutcomeExists(int id)
        {
            return _context.Outcomes.Any(e => e.Id == id);
        }
    }
}