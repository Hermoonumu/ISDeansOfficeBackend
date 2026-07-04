namespace DeanInfoSystem.API;

using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/departments")]
public class DepartmentController(IDepartmentService _deptSvc) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "Dean")]
    public async Task<IActionResult> NewDepartment([FromBody] NewDeptDTO ndDTO)
    {
        return Ok(await _deptSvc.NewDepartmentAsync(ndDTO.DepartmentName));
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<Department>>> GetDepartments()
    {
        return Ok(await _deptSvc.GetAllDepartmentsAsync());
    }


    [HttpPatch("{DeptId}")]
    [Authorize(Policy = "Dean")]
    public async Task<IActionResult> PatchDepartment([FromRoute] string DeptId, [FromBody] JsonPatchDocument<Department> DeptPatch)
    {
        try
        {
            await _deptSvc.PatchDepartmentAsync(DeptId, DeptPatch);
        }
        catch (UpdateFailedException e)
        {
            return BadRequest(new { e.Message });
        }
        return Ok();
    }

    [HttpGet("{DeptId}")]
    public async Task<ActionResult<Department>> GetDepartmentByGuid([FromRoute] string DeptId)
    {
        return Ok(await _deptSvc.GetDepartmentByGuidAsync(DeptId));
    }


    [HttpDelete("{DeptId}")]
    [Authorize(Policy = "Dean")]
    public async Task<IActionResult> DeleteDepartment([FromRoute] string DeptId)
    {
        try
        {
            await _deptSvc.RemoveDepartmentAsync(DeptId);
        }
        catch (DepartmentDoesntExistException e)
        {
            return BadRequest(new { e.Message });
        }
        return Ok();
    }
}

