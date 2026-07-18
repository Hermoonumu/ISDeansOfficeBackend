using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Subjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeanInfoSystem.API;



[ApiController]
[Route("/api/subjects")]
public class SubjectController(ISubjectService _subjSvc) : ControllerBase
{
    [Authorize(Roles = "Dean,ViceDean,EducationalAdvisor")]
    [HttpPost]
    public async Task<IActionResult> NewSubject([FromBody] NewSubjectDTO nsDTO)
    {
        Guid guid;
        guid = await _subjSvc.AddSubjectAsync(nsDTO);
        return StatusCode(201, new { Id = guid.ToString() });
    }

    [HttpPatch("{Id}")]
    [Authorize(Roles = "Dean,ViceDean,EducationalAdvisor")]
    public async Task<IActionResult> ChangeSubjectName([FromBody] SubjectUpdateDTO NewName,
                                                        [FromRoute] Guid Id)
    {
        await _subjSvc.ChangeSubjectNameAsync(Id, NewName.NewName);
        return Ok();

    }

    [HttpGet("paginated")]
    [Authorize]
    public async Task<IActionResult> GetAllSubjectsPaginated([FromQuery] int page,
                                                            [FromQuery] int take)
    {
        return Ok(await _subjSvc.GetAllSubjectsPageAsync(page, take));
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllSubjects()
    {
        return Ok(await _subjSvc.GetAllSubjectsAsync());
    }

    [HttpDelete("{Id}")]
    [Authorize(Roles = "Dean,ViceDean,EducationalAdvisor")]
    public async Task<IActionResult> DeleteSubject([FromRoute] Guid Id)
    {
        await _subjSvc.RemoveSubjectAsync(Id);
        return Ok();
    }

}