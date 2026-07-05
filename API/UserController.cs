namespace DeanInfoSystem.API;

using DeanInfoSystem.Application.Common.Auth;
using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Enrollment;
using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/users")]


public class UserController(IUserService _userSvc,
                            IAuthService _authSvc,
                            IEnrollmentService _enrollSvc) : ControllerBase
{
    [Authorize(Policy = "DeanViceDeanSecretary")]
    [HttpPost]
    public async Task<IActionResult> NewUserAsync([FromBody] NewUserDTO nuDTO)
    {
        try
        {
            await _authSvc.RegisterAsync(nuDTO);
        }
        catch
        {
            return StatusCode(500);
        }
        return StatusCode(201);
    }

    [Authorize(Roles = "Dean,ViceDean,Secretary")]
    [HttpPatch("{UserId}")]
    public async Task<IActionResult> PatchUser([FromRoute] string UserId, [FromBody] JsonPatchDocument<UserUpdateDTO> UserPatch)
    {
        try
        {
            await _userSvc.PatchUserAsync(UserId, UserPatch);
        }
        catch (UpdateFailedException e)
        {
            return BadRequest(new { e.Message });
        }
        return Ok();
    }

    [HttpDelete("{UserId}")]
    [Authorize(Policy = "DeanViceDeanSecretary")]
    public async Task<IActionResult> RemoveUser([FromRoute] string UserId)
    {
        try
        {
            await _userSvc.RemoveUserAsync(UserId);
        }
        catch (UserDoesntExistException e)
        {
            return BadRequest(new { e.Message });
        }
        return Ok();
    }

    [HttpPost("{UserId}/enroll/{ProgramId}")]
    [Authorize(Roles = "Dean,ViceDean,Secretary")]
    public async Task<IActionResult> EnrollStudent([FromRoute] string UserId,
                                                    [FromRoute] string ProgramId)
    {
        try
        {
            await _enrollSvc.EnrollStudentIntoProgramAsync(UserId, ProgramId);
        }
        catch (UserDoesntExistException e)
        {
            return NotFound(new { e.Message });
        }
        catch (ProgramDoesntExistException e)
        {
            return NotFound(new { e.Message });
        }
        catch (PositionException e)
        {
            return StatusCode(422, new { e.Message });
        }
        return Ok();
    }

    [HttpPost("{UserId}/assignSubject/{SubjectId}")]
    [Authorize(Roles = "Dean,ViceDean")]
    public async Task<IActionResult> AssignProfessorToSubject([FromRoute] string UserId,
                                                                [FromRoute] string SubjectId)
    {
        try
        {
            await _userSvc.AssignProfToSubjectAsync(SubjectId, UserId);
        }
        catch (UserDoesntExistException e)
        {
            return NotFound(new { e.Message });
        }
        catch (SubjectDoesntExistException e)
        {
            return NotFound(new { e.Message });
        }
        catch (UpdateFailedException e)
        {
            return Conflict(new { e.Message });
        }
        catch (PositionException e)
        {
            return StatusCode(422, new { e.Message });
        }
        return Ok();
    }

    [HttpGet("{UserId}/subjects")]
    [Authorize]
    public async Task<IActionResult> GetAllProfSubjects([FromRoute] string UserId)
    {
        List<Subject> subjects;
        try
        {
            subjects = await _userSvc.GetSubjectsAssignedAsync(UserId);
        }
        catch (UserDoesntExistException e)
        {
            return NotFound(new { e.Message });
        }
        return Ok(subjects);
    }
}