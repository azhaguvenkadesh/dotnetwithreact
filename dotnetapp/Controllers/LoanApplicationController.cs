using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnetapp.Models;

namespace dotnetapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanApplicationController : ControllerBase
    {
        private readonly LoanApplicationDbContext _context;

        public LoanApplicationController(LoanApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllLoanApplications")]
        public async Task<ActionResult<IEnumerable<LoanApplication>>> GetAllLoanApplications()
        {
            var loanApplications = await _context.LoanApplications.ToListAsync();
            return Ok(loanApplications);
        }

        [HttpPost("AddLoanApplication")]
        public async Task<ActionResult> AddLoanApplication(LoanApplication loanApplication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return detailed validation errors
            }
            await _context.LoanApplications.AddAsync(loanApplication);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("GetLoanApplicationsByUserId")]
public async Task<ActionResult<IEnumerable<LoanApplication>>> GetLoanApplicationsByUserId(string userId)
{
    if (string.IsNullOrEmpty(userId))
    {
        return BadRequest("UserId is required");
    }

    var loanApplications = await _context.LoanApplications
        .Where(l => l.userId == userId)
        .ToListAsync();

    if (loanApplications == null || !loanApplications.Any())
    {
        return NotFound("No loan applications found for the provided userId");
    }

    return Ok(loanApplications);
}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoanApplication(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid LoanApplication id");

            var loanApplication = await _context.LoanApplications.FindAsync(id);
            _context.LoanApplications.Remove(loanApplication);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{LoanApplicationID}")]
        public async Task<IActionResult> UpdateLoanApplication(int LoanApplicationID, LoanApplication updatedLoanApplication)
        {
            if (LoanApplicationID <= 0)
            {
                return BadRequest("Not a valid LoanApplication id");
            }

            var existingLoanApplication = await _context.LoanApplications.FindAsync(LoanApplicationID);

            if (existingLoanApplication == null)
            {
                return NotFound("LoanApplication not found");
            }

            // Update the existingLoanApplication with the values from updatedLoanApplication
            existingLoanApplication.userName = updatedLoanApplication.userName;
            existingLoanApplication.RequestedAmount = updatedLoanApplication.RequestedAmount;
            existingLoanApplication.SubmissionDate = updatedLoanApplication.SubmissionDate;
            existingLoanApplication.EmploymentStatus = updatedLoanApplication.EmploymentStatus;
            existingLoanApplication.Income = updatedLoanApplication.Income;
            existingLoanApplication.CreditScore = updatedLoanApplication.CreditScore;
 existingLoanApplication.LoanStatus = updatedLoanApplication.LoanStatus;

 existingLoanApplication.LoanType = updatedLoanApplication.LoanType;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanApplicationExists(LoanApplicationID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool LoanApplicationExists(int LoanApplicationID)
        {
            return _context.LoanApplications.Any(e => e.LoanApplicationID == LoanApplicationID);
        }

    }
}
