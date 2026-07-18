using DeanInfoSystem.Application.Common.Auth;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Programs;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;
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

    [Authorize(Roles = "EducationalAdvisor,ViceDean,Dean")]
    [HttpPost("{ProgId}")]
    public async Task<IActionResult> AssignSubjectToProgram([FromRoute] Guid ProgId,
                                                            [FromBody] AddSubjectToProgramDTO astpDTO)
    {
        Guid guid;
        guid = await _progSvc.AssignSubjectToProgramAsync(ProgId, astpDTO);
        return StatusCode(201, new { Id = guid.ToString() });
    }

    [Authorize(Roles = "EducationalAdvisor,ViceDean,Dean")]
    [HttpDelete("{ProgId}/subject/{SubjectId}")]
    public async Task<IActionResult> DismissSubjectFromProgram([FromRoute] Guid ProgId,
                                                            [FromRoute] Guid SubjectId)
    {
        await _progSvc.RemoveSubjectFromProgramAsync(SubjectId, ProgId);
        return Ok();
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

    [HttpGet("MySyllabus")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetOwnSyllabus([FromQuery] int semester = 0)
    {
        User user = await _authSvc.AuthenticateUserAsync(HttpContext.Request.Cookies["AccessToken"]!);
        return Ok(await _progSvc.GetProgramCurriculaAsync(user.ProgramId));
    }

    [HttpGet("{ProgramId}/ProgramSyllabus")]
    [Authorize]
    public async Task<IActionResult> GetSyllabusAll([FromRoute] Guid ProgramId)
    {
        return Ok(await _progSvc.GetProgramCurriculaAsync(ProgramId));
    }

    [HttpGet("EducatorSyllabus")]
    [Authorize(Roles = "Educator")]
    public async Task<IActionResult> EducatorSyllabus()
    {
        User user = await _authSvc.AuthenticateUserAsync(HttpContext.Request.Cookies["AccessToken"]!);
        return Ok(await _progSvc.GetEducatorAssignedCurricula(user.Id));
    }



    [HttpPatch("{ProgramId}")]
    [Authorize(Roles = "Dean,ViceDean")]
    public async Task<IActionResult> PatchProgram([FromRoute] Guid ProgramId,
                                                    [FromBody] JsonPatchDocument<ProgramPatchDTO> ppDTO)
    {
        await _progSvc.PatchProgramAsync(ProgramId, ppDTO);
        return Ok();
    }

    [HttpGet("paginated")]
    [Authorize]
    public async Task<IActionResult> GetProgramsPaginated([FromQuery] int page,
                                                            [FromQuery] int take)
    {
        return Ok(await _progSvc.GetProgramsPageAsync(page, take));
    }


    [HttpGet()]
    [Authorize]
    public async Task<IActionResult> GetAllPrograms()
    {
        return Ok(await _progSvc.GetAllProgramsAsync());
    }

    [HttpGet("AllCurricula")]
    [Authorize]
    public async Task<IActionResult> GetAllCurricula()
    {
        return Ok(await _progSvc.GetAllCurriculaAsync());
    }
}