using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TestApi.Services;

namespace TestApi.Controllers.Weather.V1;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpPost("")]
    public async Task<IActionResult> Post([FromServices]IBusinessLogicImplementation service,UserRequest req)
    {
        return Ok(await service.SaveToDbIfConditionsAreRight(req.ToInternalModel()));
    }
    
    [HttpGet("")]
    public async Task<IActionResult> Get([FromServices]IBusinessLogicImplementation service,[Required]string userId)
    {
        return Ok(await service.User(userId));
    }
}