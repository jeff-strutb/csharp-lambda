using Microsoft.AspNetCore.Mvc;

namespace webapi.Personas.Controllers;

[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> logger;

    public TestController(
        ILogger<TestController> logger
    )
    {
        this.logger = logger;
    }

    [HttpGet("is-alive")]
    public IActionResult IsAlive() {
        return Ok("Alive!");
    }
}