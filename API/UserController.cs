namespace DeanInfoSystem.API;

using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/users")]


public class UserController(IUserService _userSvc) : ControllerBase
{
    [Authorize(Policy = "DeanViceDeanSecretary")]
    [HttpPost]
    public async Task<IActionResult> NewUserAsync([FromBody] NewUserDTO nuDTO)
    {
        try
        {
            await _userSvc.AddUserAsync(nuDTO);
        }
        catch
        {
            return StatusCode(500);
        }
        return Created();
    }

    [Authorize(Policy = "DeanViceDeanSecretary")]
    [HttpPatch("{UserId}")]
    public async Task<IActionResult> PatchUser([FromRoute] string UserId, [FromBody] JsonPatchDocument<User> UserPatch)
    {
        User? user = await _userSvc.GetUserByIdAsync(UserId);
        if (user is null) return BadRequest("We couldn't find the user");
        try
        {
            await _userSvc.PatchUserAsync(user, UserPatch);
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

}