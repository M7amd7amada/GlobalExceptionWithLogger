using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("throw-error")]
    public IActionResult ThrowError()
    {
        throw new InvalidOperationException("This is a test exception.");
    }
}
