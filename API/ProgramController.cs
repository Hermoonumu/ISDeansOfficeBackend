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


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPrograms()
    {
        return StatusCode(500);
    }
}