using Microsoft.AspNetCore.Mvc;
using System;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            // 200 OK
            return Ok(new { message = "Hello, World!" + DateTime.Now.ToString() });
        }

        [HttpGet("created")]
        public IActionResult GetCreated()
        {
            // 201 Created
            return Created("/api/helloworld/created", new { message = "Resource created successfully" });
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            // 400 Bad Request
            return BadRequest(new { error = "Bad request, check your input" });
        }

        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            // 401 Unauthorized
            return Unauthorized(new { error = "Unauthorized access" });
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            // 404 Not Found
            return NotFound(new { error = "Resource not found" });
        }

        [HttpGet("internalservererror")]
        public IActionResult GetInternalServerError()
        {
            // 500 Internal Server Error
            // Note: This is just an example, you might want to handle errors more gracefully in a real application.
            throw new Exception("Internal server error occurred");
        }
    }
}
