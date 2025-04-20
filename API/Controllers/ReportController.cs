using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

     
        [HttpGet("performance")]
        public async Task<IActionResult> GetUserPerformanceReport([FromHeader(Name = "X-User")] string user)
        {
            
            if (user != "admin")
                return Forbid("Apenas o usuário 'admin' pode acessar este relatório.");

            var report = await _reportService.GetUserPerformanceReportAsync();
            return Ok(report);
        }
    }
}
