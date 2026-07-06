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
    public async Task<IActionResult> PatchDepartment([FromRoute] Guid DeptId, [FromBody] JsonPatchDocument<Department> DeptPatch)
    {
        await _deptSvc.PatchDepartmentAsync(DeptId, DeptPatch);
        return Ok();
    }

    [HttpGet("{DeptId}")]
    public async Task<ActionResult<Department>> GetDepartmentByGuid([FromRoute] Guid DeptId)
    {
        return Ok(await _deptSvc.GetDepartmentByGuidAsync(DeptId));
    }


    [HttpDelete("{DeptId}")]
    [Authorize(Policy = "Dean")]
    public async Task<IActionResult> DeleteDepartment([FromRoute] Guid DeptId)
    {
        await _deptSvc.RemoveDepartmentAsync(DeptId);

        return Ok();
    }
}

