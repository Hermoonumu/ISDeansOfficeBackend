using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.StudentGrades;
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
        try
        {
            await _sgSvc.GradeAsync(GradeId, gDTO.Grade);
        }
        catch (GradeDoesntExistException e)
        {
            return NotFound(new { e.Message });
        }
        catch (UpdateFailedException e)
        {
            return StatusCode(422, new { e.Message });
        }
        return Ok();
    }
}