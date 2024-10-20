using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev_Talabat.APIs.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Buggy
{
    public class BuggyController : ApiControllerBase
    {

        [HttpGet("notfound")] // Get: api/Buggy/notfound
        public IActionResult GetNotFoundRequest() 
        {
            // second way for handling error is to throw exception , and use middleware to handle it
            // throw new Exception("test exception");


            //throw new NotFoundException();

            //helper method to return not found response
            return NotFound(new ApiResponse(404)); // 404 , better not to be anonymous object , cuz its used alot
        }



        [HttpGet("servererror")] // Get: api/Buggy/servererror
        public IActionResult GetServerError() 
        {
            throw new Exception(); // 500
        }



        [HttpGet("badrequest")] // Get: api/Buggy/badrequest
        public IActionResult GetBadRequest() 
        {

            return BadRequest(new ApiResponse(400)); // 400
        }


        [HttpGet("badrequest/{id}")] // Get: api/Buggy/badrequest/id
        public IActionResult GetValidationError(int id)  // 400
        {
            // if we will suppress the validatione error model state 
            // we will use if (!ModelState.IsValid) return BadRequest(new ApiValidationErrorResponse(ModelState)); // 400


           return Ok(); // this wont be excuted cux we inherit from controllerBase , a factory will be created to handle the request , get from [ApiController] attribute 
        }



        [HttpGet("unauthorized")] // Get: api/Buggy/unauthorized
        public IActionResult GetUnauthorizedError() 
        {
            return Unauthorized(new ApiResponse(401)); // 401
        }



        [HttpGet("forbidden")] // Get: api/Buggy/forbidden
        public IActionResult GetForbidden() 
        { 
            return Forbid(); // 403
        }

    }
}
