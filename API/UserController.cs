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
        await _authSvc.RegisterAsync(nuDTO);
        return StatusCode(201);
    }

    [Authorize(Roles = "Dean,ViceDean,Secretary")]
    [HttpPatch("{UserId}")]
    public async Task<IActionResult> PatchUser([FromRoute] Guid UserId, [FromBody] JsonPatchDocument<UserUpdateDTO> UserPatch)
    {
        await _userSvc.PatchUserAsync(UserId, UserPatch);
        return Ok();
    }

    [HttpDelete("{UserId}")]
    [Authorize(Policy = "DeanViceDeanSecretary")]
    public async Task<IActionResult> RemoveUser([FromRoute] Guid UserId)
    {
        await _userSvc.RemoveUserAsync(UserId);
        return Ok();
    }

    [HttpPost("{UserId}/enroll/{ProgramId}")]
    [Authorize(Roles = "Dean,ViceDean,Secretary")]
    public async Task<IActionResult> EnrollStudent([FromRoute] Guid UserId,
                                                    [FromRoute] Guid ProgramId)
    {
        await _enrollSvc.EnrollStudentIntoProgramAsync(UserId, ProgramId);
        return Ok();
    }

    [HttpPost("{UserId}/assignSubject/{SubjectId}")]
    [Authorize(Roles = "Dean,ViceDean")]
    public async Task<IActionResult> AssignProfessorToSubject([FromRoute] Guid UserId,
                                                                [FromRoute] Guid SubjectId)
    {
        await _userSvc.AssignProfToSubjectAsync(SubjectId, UserId);
        return Ok();
    }

    [HttpGet("{UserId}/subjects")]
    [Authorize]
    public async Task<IActionResult> GetAllProfSubjects([FromRoute] Guid UserId)
    {
        List<Subject> subjects;
        subjects = await _userSvc.GetSubjectsAssignedAsync(UserId);
        return Ok(subjects);
    }
}