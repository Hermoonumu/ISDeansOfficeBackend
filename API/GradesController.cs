using DeanInfoSystem.Application.Common.Auth;
using DeanInfoSystem.Application.Common.Mappers;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.StudentGrades;
using DeanInfoSystem.Domain;
using DeanInfoSystem.Infrastructure.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeanInfoSystem.API;



[ApiController]
[Route("/api/grades")]
public class GradesController(IStudentGradeService _sgSvc,
                            IAuthService _authSvc) : ControllerBase
{
    [HttpPost("{GradeId}")]
    [Authorize(Roles = "Educator,Dean,ViceDean")]
    public async Task<IActionResult> Grade([FromBody] GradeDTO gDTO, [FromRoute] Guid GradeId)
    {
        User user = await _authSvc.AuthenticateUserAsync(HttpContext.Request.Cookies["AccessToken"]!);
        await _sgSvc.GradeAsync(user, GradeId, gDTO.Grade);
        return Ok();
    }

    [HttpPost("bulkGrade")]
    [Authorize(Roles = "Educator,Dean,ViceDean")]
    public async Task<IActionResult> Grade([FromBody] List<BulkGradeDTO> bgDTO)
    {
        User user = await _authSvc.AuthenticateUserAsync(HttpContext.Request.Cookies["AccessToken"]!);
        await _sgSvc.BulkGradeAsync(user, bgDTO);
        return Ok();
    }

    [HttpGet("student/{StudentId}")]
    [Authorize(Roles = "Dean,ViceDean,Educator,Secretary")]
    public async Task<IActionResult> GetStudentGrades([FromRoute] Guid StudentId)
    {
        var list = (await _sgSvc.GetStudentGradesAsync(StudentId)).Select(StudentGradeMapper.SGToDTO).ToList();
        return Ok(list);
    }

    [HttpGet("MyGrades")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetOwnGrades()
    {
        User user = await _authSvc.AuthenticateUserAsync(HttpContext.Request.Cookies["AccessToken"]!);
        return Ok(await _sgSvc.GetStudentGradesDTOAsync(user.Id));
    }

    [HttpGet("MyRecentGrades")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetOwnRecentGrades([FromQuery] int amount = 10)
    {
        User user = await _authSvc.AuthenticateUserAsync(HttpContext.Request.Cookies["AccessToken"]!);
        return Ok(await _sgSvc.GetStudentRecentGradesAsync(user.Id, amount));
    }

    [HttpGet("curriculum/{CurrId}")]
    [Authorize(Roles = "Dean,ViceDean,Educator")]
    public async Task<IActionResult> GetGradesByCurriculum([FromRoute] Guid CurrId)
    {
        User user = await _authSvc.AuthenticateUserAsync(HttpContext.Request.Cookies["AccessToken"]!);
        return Ok(await _sgSvc.GetGradesByCurriculumAsync(user, CurrId));
    }

    [HttpGet("MyStudentsGrades")]
    [Authorize(Roles = "Educator")]
    public async Task<IActionResult> MyStudentsGrades()
    {
        User user = await _authSvc.AuthenticateUserAsync(HttpContext.Request.Cookies["AccessToken"]!);
        return Ok(await _sgSvc.GetGradesByEducatorIdAsync(user.Id));

    }

    [HttpGet("MyUngradedStudents")]
    [Authorize(Roles = "Educator")]
    public async Task<IActionResult> GetUngradedStudents()
    {
        User user = await _authSvc.AuthenticateUserAsync(HttpContext.Request.Cookies["AccessToken"]!);
        return Ok(await _sgSvc.GetUngradedStudentsAsync(user.Id));
    }

    [HttpPost("ConfirmGradeFailure/{GradeId}")]
    [Authorize(Roles = "Educator")]
    public async Task<IActionResult> ConfirmGradeFail([FromRoute] Guid GradeId)
    {
        await _sgSvc.ConfirmFailingGrade(GradeId);
        return Ok();
    }
}