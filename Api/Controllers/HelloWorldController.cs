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
            return StatusCode(201, new { message = "Resource created successfully" });
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            // 400 Bad Request
            return BadRequest("BadRequest");
        }

        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            // 401 Unauthorized
            return StatusCode(401, new { error = "Unauthorized access" });
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            // 404 Not Found
            return StatusCode(404, new { error = "Resource not found" });
        }

        [HttpGet("internalservererror")]
        public IActionResult GetInternalServerError()
        {
            // 500 Internal Server Error
            return StatusCode(500, new { error = "Internal server error occurred" });
        }
    }
}
