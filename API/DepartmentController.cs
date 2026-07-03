namespace DeanInfoSystem.API;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/departments")]
public class DepartmentController : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> NewDepartment()
    {
        return Ok();
    }
}

