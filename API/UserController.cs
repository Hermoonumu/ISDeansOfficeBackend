

using DeanInfoSystem.Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/users")]
public class UserController : ControllerBase
{
    [Authorize(Policy = "Dean")]
    [HttpPost]
    public async Task<IActionResult> NewUserAsync([FromBody] NewUserDTO nuDTO)
    {
        return Ok();
    }
}