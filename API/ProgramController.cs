using DeanInfoSystem.Application.Common.Auth;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Programs;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace DeanInfoSystem.API;


[ApiController]
[Route("/api/programs")]
public class ProgramController(IProgramService _progSvc,
                                IAuthService _authSvc) : ControllerBase
{

    [Authorize(Roles = "Dean,ViceDean")]
    [HttpPost]
    public async Task<IActionResult> NewProgram([FromBody] NewProgramDTO npDTO)
    {
        await _progSvc.AddProgramAsync(npDTO);
        return Created();
    }

    [Authorize(Roles = "EducationalAdvisor,Educator,ViceDean,Dean")]
    [HttpPost("{ProgId}")]
    public async Task<IActionResult> AssignSubjectToProgram([FromRoute] Guid ProgId,
                                                            [FromBody] AddSubjectToProgramDTO astpDTO)
    {
        Guid guid;
        guid = await _progSvc.AssignSubjectToProgramAsync(ProgId, astpDTO);
        return StatusCode(201, new { Id = guid.ToString() });
    }

    [Authorize(Roles = "Dean,ViceDean")]
    [HttpDelete("{ProgramId}")]
    public async Task<IActionResult> RemoveProgram([FromRoute] Guid ProgramId)
    {
        await _progSvc.RemoveProgramAsync(ProgramId);
        return NoContent();
    }


    [Authorize(Roles = "Dean,ViceDean")]
    [HttpPost("{ProgramId}/setStatus/{status}")]
    public async Task<IActionResult> SetProgramStatus([FromRoute] Guid ProgramId,
                                                    [FromRoute] ProgramStatus status)
    {
        await _progSvc.ChangeProgramStatusAsync(status, ProgramId);
        return Ok();
    }

    [HttpPost("MySyllabus")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetSyllabus()
    {
        User user = await _authSvc.AuthenticateUserAsync(HttpContext.Request.Cookies["AccessToken"]!);
        return Ok(await _progSvc.GetProgramCurriculaAsync(user.ProgramId));
    }


    [HttpGet("paginated")]
    [Authorize]
    public async Task<IActionResult> GetProgramsPaginated([FromQuery] int page,
                                                            [FromQuery] int take)
    {
        return Ok(await _progSvc.GetProgramsPageAsync(page, take));
    }
}