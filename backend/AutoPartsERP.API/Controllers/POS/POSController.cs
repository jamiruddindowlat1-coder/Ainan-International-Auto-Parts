using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoPartsERP.API.DTOs.Common;

namespace AutoPartsERP.API.Controllers.POS;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class POSController : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<object>> GetPOSData()
    {
        return Ok(ApiResponse<object>.Ok(new { Message = "POS endpoints will be implemented here. Sales are handled by SalesController." }));
    }
}

