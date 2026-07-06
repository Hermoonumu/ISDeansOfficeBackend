using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Programs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeanInfoSystem.API;


[ApiController]
[Route("/api/programs")]
public class ProgramController(IProgramService _progSvc) : ControllerBase
{

    [Authorize(Policy = "DeanViceDean")]
    [HttpPost]
    public async Task<IActionResult> NewProgram([FromBody] NewProgramDTO npDTO)
    {
        Guid guid;
        try
        {
            await _progSvc.AddProgramAsync(npDTO);
        }
        catch (DepartmentDoesntExistException e)
        {
            return BadRequest(new { e.Message });
        }
        return Created();
    }

    [Authorize(Roles = "EducationalAdvisor,Educator,ViceDean,Dean")]
    [HttpPost("{ProgId}")]
    public async Task<IActionResult> AssignSubjectToProgram([FromRoute] Guid ProgId,
                                                            [FromBody] AddSubjectToProgramDTO astpDTO)
    {
        Guid guid;
        try
        {
            guid = await _progSvc.AssignSubjectToProgramAsync(ProgId, astpDTO);
        }
        catch (ProgramDoesntExistException e)
        {
            return NotFound(new { e.Message });
        }
        return StatusCode(201, new { Id = guid.ToString() });
    }
}