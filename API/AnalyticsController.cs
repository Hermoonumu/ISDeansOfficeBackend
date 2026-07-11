using DeanInfoSystem.Application.Analytics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeanInfoSystem.API;



[ApiController]
[Route("/api/analytics")]
public class AnalyticsController(IAnalyticsService _analSvc) : ControllerBase
{
    [HttpGet("programs/{ProgramId}/gradeDist")]
    [Authorize(Roles = "Dean,ViceDean,Secretary,EducationalAdvisor")]
    public async Task<IActionResult> GetProgGradeDist([FromRoute] Guid ProgramId)
    {
        return Ok(await _analSvc.GetGradeDistInProgramAsync(ProgramId));
    }
}