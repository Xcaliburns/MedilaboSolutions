using DiabeteRiskReportService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiabeteRiskReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Praticien")]
    public class DiabeteReportController : ControllerBase
    {
        private readonly IDiabeteReportService _diabeteReportService;

        public DiabeteReportController(IDiabeteReportService diabeteReportService)
        {
            _diabeteReportService = diabeteReportService;
        }

        // GET: api/DiabeteReport/Report/{patientId}
        [HttpGet("Report/{patientId}")]
        public async Task<IActionResult> GenerateReport(int patientId, [FromHeader(Name = "Authorization")] string authToken)
        {
            if (string.IsNullOrEmpty(authToken))
            {
                return Unauthorized("Missing Authorization header");
            }

            // Remove "Bearer " prefix from the token
            var token = authToken.StartsWith("Bearer ") ? authToken.Substring(7) : authToken;

            var report = await _diabeteReportService.Report(patientId, token);
            if (report == "Patient not found")
            {
                return NotFound(report);
            }
            return Ok(report);
        }
    }
}
