using DeanInfoSystem.Application.Common.Auth;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.StudentGrades;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeanInfoSystem.API;



[ApiController]
[Route("/api/grades")]
public class GradesController(IStudentGradeService _sgSvc,
                            IAuthService _authSvc) : ControllerBase
{
    [HttpPost("{GradeId}")]
    [Authorize(Roles = "Educator,Assistant,EducationalAdvisor")]
    public async Task<IActionResult> Grade([FromBody] GradeDTO gDTO, [FromRoute] Guid GradeId)
    {
        await _sgSvc.GradeAsync(GradeId, gDTO.Grade);
        return Ok();
    }

    [HttpPost("bulkGrade")]
    [Authorize(Roles = "Educator,Assistant,EducationalAdvisor")]
    public async Task<IActionResult> Grade([FromBody] List<BulkGradeDTO> bgDTO)
    {
        await _sgSvc.BulkGradeAsync(bgDTO);
        return Ok();
    }

    [HttpGet("student/{StudentId}")]
    [Authorize(Roles = "Dean,ViceDean,Educator,Secretary")]
    public async Task<IActionResult> GetStudentGrades([FromRoute] Guid StudentId)
    {
        return Ok(await _sgSvc.GetStudentGradesAsync(StudentId));
    }

    [HttpGet("MyGrades")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetOwnGrades()
    {
        User user = await _authSvc.AuthenticateUserAsync(HttpContext.Request.Cookies["AccessToken"]!);
        return Ok(await _sgSvc.GetStudentGradesAsync(user.Id));
    }

    [HttpGet("curriculum/{CurrId}")]
    [Authorize(Roles = "Dean,ViceDean,Educator")]
    public async Task<IActionResult> GetGradesByCurriculum([FromRoute] Guid CurrId)
    {
        return Ok(await _sgSvc.GetGradesByCurriculumAsync(CurrId));
    }

    [HttpGet("MyStudentsGrades")]
    [Authorize(Roles = "Educator")]
    public async Task<IActionResult> MyStudentsGrades()
    {
        User user = await _authSvc.AuthenticateUserAsync(HttpContext.Request.Cookies["AccessToken"]!);
        return Ok(await _sgSvc.GetGradesByEducatorIdAsync(user.Id));

    }
}