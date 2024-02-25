using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("throw-error")]
    public IActionResult ThrowError(int id)
    {
        throw new InvalidOperationException("This is a test exception.");
    }

    [HttpPost("test-post")]
    public IActionResult TestPost(int num)
    {
        throw new InvalidOperationException("This is a test exception with post method.");
    }
}
