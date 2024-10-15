using LinkDev.Talabat.APIs.Controllers.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.APIs.Controllers.Comman
{
    [ApiController]
    [Route("Error/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)] // to ignore this controller from swagger, ignore documentation
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult Error(int code)
        {
            if (code == (int)HttpStatusCode.NotFound)
            {
                var response = new ApiResponse((int)HttpStatusCode.NotFound, $"the request endpoint: {Request.Path} is not found");
                return NotFound(response);
            }
            return NotFound(code);
        }

    }
}
