using DeanInfoSystem.API.DTO;
using DeanInfoSystem.Application.Common.Exceptions;
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
        try
        {
            guid = await _subjSvc.AddSubjectAsync(nsDTO);
        }
        catch (SubjectAlreadyExistsException e)
        {
            return Conflict(new { e.Message });
        }
        return StatusCode(201, new { Id = guid.ToString() });
    }

    [HttpPatch("{Id}")]
    [Authorize(Roles = "Dean,ViceDean,EducationalAdvisor")]
    public async Task<IActionResult> ChangeSubjectName([FromBody] SubjectUpdateDTO NewName,
                                                        [FromRoute] Guid Id)
    {
        try
        {
            await _subjSvc.ChangeSubjectNameAsync(Id, NewName.NewName);
        }
        catch (SubjectDoesntExistException e)
        {
            return NotFound(new { e.Message });
        }
        return Ok();

    }

    [HttpDelete("{Id}")]
    [Authorize(Roles = "Dean,ViceDean,EducationalAdvisor")]
    public async Task<IActionResult> DeleteSubject([FromRoute] Guid Id)
    {
        await _subjSvc.RemoveSubjectAsync(Id);
        return Ok();
    }

}