using DeanInfoSystem.Application.Analytics;
using DeanInfoSystem.Application.Common.Auth;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Redis;

namespace DeanInfoSystem.API;



[ApiController]
[Route("/api/analytics")]
public class AnalyticsController(IAnalyticsService _analSvc,
                                    IAuthService _authSvc) : ControllerBase
{
    [HttpGet("programs/{ProgramId}/gradeDist")]
    [Authorize(Roles = "Dean,ViceDean,Secretary,EducationalAdvisor")]
    public async Task<IActionResult> GetProgGradeDist([FromRoute] Guid ProgramId)
    {
        return Ok(await _analSvc.GetGradeDistInProgramAsync(ProgramId));
    }

    [HttpGet("cohort/SholarshipRating/{ProgramId}")]
    [Authorize(Roles = "Dean,ViceDean,Secretary")]
    public async Task<IActionResult> GetProgramRating([FromRoute] Guid ProgramId)
    {
        return Ok(await _analSvc.GetStudentRankingInProgramAsync(ProgramId));
    }

    [HttpGet("MyPerformance")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetPerformance()
    {
        User? user = await _authSvc.AuthenticateUserAsync(
            HttpContext.Request.Cookies["AccessToken"]!);
        return Ok(await _analSvc.GetStudentsPerformanceAsync(user.Id));
    }

    [HttpGet("EducatorPerformance")]
    [Authorize(Roles = "Educator")]
    public async Task<IActionResult> GetEducatorPerformance()
    {
        User? user = await _authSvc.AuthenticateUserAsync(
            HttpContext.Request.Cookies["AccessToken"]!);
        return Ok(await _analSvc.GetEducatorDashboardAsync(user.Id));
    }


    [HttpGet("DeanDashboard")]
    [Authorize(Roles = "Dean")]
    public async Task<IActionResult> GetDeanDashboard()
    {
        return Ok(await _analSvc.GetDeanDashboardAsync(new Guid()));
    }
}