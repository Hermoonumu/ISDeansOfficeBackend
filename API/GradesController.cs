using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.StudentGrades;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeanInfoSystem.API;



[ApiController]
[Route("/api/grades")]
public class GradesController(IStudentGradeService _sgSvc) : ControllerBase
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

    [HttpGet("{StudentId}")]
    public async Task<IActionResult> GetStudentGrades([FromRoute] Guid StudentId)
    {
        return Ok(await _sgSvc.GetStudentGradesAsync(StudentId));
    }
}