using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.APIs.Controllers.Exceptions;
using LinkDev.Talabat.Core.Application.Products.Models;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;

namespace LinkDev.Talabat.APIs.Middlewares
{
    // 3 ways to create middleware
    // 1.conventuin based
    // 2.factorybased
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }



        // here there is no logic , its built to be the first middleware for the request and response to catch the exception , next go to the next middle ware, and logic after code happen when the response is back to the client
        public async Task InvokeAsync(HttpContext httpContext)
        {




            try
            {
                // Logic Excuted with the Request.

                await _next(httpContext); //just call the delegate and pass the current request

                // Logic Excuted with the Response.


                // we already handle the not found using middleware app.UseStatusCodePagesWithReExecute("/Error/{0}");, so we dont need this code
                //if (httpContext.Response.StatusCode == (int) HttpStatusCode.NotFound)
                //{
                //   var response = new ApiResponse((int)HttpStatusCode.NotFound,$"the request endpoint: {httpContext.Request.Path} is not found");
                //    await httpContext.Response.WriteAsync(response.ToString());
                //}

            }
            catch (Exception ex)
            {
                ApiResponse response;

                switch ( ex )
                {
                    case NotFoundException:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        httpContext.Response.ContentType = "application/json";

                        response = new ApiResponse(404, ex.Message);

                        await httpContext.Response.WriteAsync(response.ToString());
                        break;
                    


                    default: 
                        
                

                    if (_env.IsDevelopment())
                    {
                        // Development mode.

                        _logger.LogError(ex, ex.Message);
                        response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString());
                    }

                    else
                    {
                        // Production mode.
                        // Log the exception Details in the database. | File (text,jason).
                        response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                    }
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.ContentType = "application/json";

                    await httpContext.Response.WriteAsync(response.ToString());

                    break;
                }


            }
        }
    }
}
