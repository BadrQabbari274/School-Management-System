using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvidenceController : ControllerBase
    {
        private readonly IEvidenceService _evidenceService;

        public EvidenceController(IEvidenceService evidenceService)
        {
            _evidenceService = evidenceService;
        }

        // GET: api/Evidence
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evidence>>> GetEvidences()
        {
            try
            {
                var evidences = await _evidenceService.GetAllEvidencesAsync();
                return Ok(evidences);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Evidence/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evidence>> GetEvidence(int id)
        {
            try
            {
                var evidence = await _evidenceService.GetEvidenceByIdAsync(id);

                if (evidence == null)
                {
                    return NotFound($"Evidence with ID {id} not found.");
                }

                return Ok(evidence);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Evidence/outcome/5
        [HttpGet("outcome/{outcomeId}")]
        public async Task<ActionResult<IEnumerable<Evidence>>> GetEvidencesByOutcome(int outcomeId)
        {
            try
            {
                var evidences = await _evidenceService.GetEvidencesByOutcomeIdAsync(outcomeId);
                return Ok(evidences);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Evidence/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Evidence>>> GetActiveEvidences()
        {
            try
            {
                var evidences = await _evidenceService.GetActiveEvidencesAsync();
                return Ok(evidences);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Evidence/practical
        [HttpGet("practical")]
        public async Task<ActionResult<IEnumerable<Evidence>>> GetPracticalEvidences()
        {
            try
            {
                var evidences = await _evidenceService.GetPracticalEvidencesAsync();
                return Ok(evidences);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Evidence
        [HttpPost]
        public async Task<ActionResult<Evidence>> CreateEvidence(Evidence evidence)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdEvidence = await _evidenceService.CreateEvidenceAsync(evidence);

                return CreatedAtAction(
                    nameof(GetEvidence),
                    new { id = createdEvidence.Id },
                    createdEvidence
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Evidence/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvidence(int id, Evidence evidence)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != evidence.Id)
                {
                    return BadRequest("ID mismatch.");
                }

                var updatedEvidence = await _evidenceService.UpdateEvidenceAsync(id, evidence);

                if (updatedEvidence == null)
                {
                    return NotFound($"Evidence with ID {id} not found.");
                }

                return Ok(updatedEvidence);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Evidence/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvidence(int id)
        {
            try
            {
                var result = await _evidenceService.DeleteEvidenceAsync(id);

                if (!result)
                {
                    return NotFound($"Evidence with ID {id} not found.");
                }

                return Ok($"Evidence with ID {id} has been deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PATCH: api/Evidence/5/toggle-status
        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleEvidenceStatus(int id)
        {
            try
            {
                var result = await _evidenceService.ToggleEvidenceStatusAsync(id);

                if (!result)
                {
                    return NotFound($"Evidence with ID {id} not found.");
                }

                return Ok($"Evidence status has been toggled successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Evidence/exists/5
        [HttpGet("exists/{id}")]
        public async Task<ActionResult<bool>> EvidenceExists(int id)
        {
            try
            {
                var exists = await _evidenceService.EvidenceExistsAsync(id);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}