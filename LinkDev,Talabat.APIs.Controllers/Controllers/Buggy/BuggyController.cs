using LinkDev_Talabat.APIs.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Buggy
{
    public class BuggyController : ApiControllerBase
    {

        [HttpGet("notfound")] // Get: api/Buggy/notfound
        public IActionResult GetNotFoundRequest() 
        {
            return NotFound(new { statusCode = 404, message = "not found" }); // 404 , better not to be anonymous object , cuz its used alot
        }



        [HttpGet("servererror")] // Get: api/Buggy/servererror
        public IActionResult GetNotFoundResponse() 
        {
            return StatusCode(500, new { statusCode = 500, message = "server error" }); // 500
        }



        [HttpGet("badrequest")] // Get: api/Buggy/badrequest
        public IActionResult GetBadRequest() 
        {
            return BadRequest(new { statusCode = 400, message = "bad request" }); // 400
        }


        [HttpGet("badrequest/{id}")] // Get: api/Buggy/badrequest/id
        public IActionResult GetValidationError(int id)  // 400
        {
           return Ok(); // this wont be excuted cux we inherit from controllerBase , a factory will be created to handle the request , get from [ApiController] attribute 
        }



        [HttpGet("unauthorized")] // Get: api/Buggy/unauthorized
        public IActionResult GetUnauthorizedError() 
        {
            return Unauthorized(new { statusCode = 401, message = "unauthorized" }); // 401
        }



        [HttpGet("forbidden")] // Get: api/Buggy/forbidden
        public IActionResult GetForbidden() 
        { 
            return Forbid(); // 403
        }

    }
}
