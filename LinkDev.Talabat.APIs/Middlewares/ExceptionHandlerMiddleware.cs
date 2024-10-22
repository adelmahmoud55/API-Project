using Azure;
using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.APIs.Controllers.Exceptions;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Application.Models.Products;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;

namespace LinkDev.Talabat.APIs.Middlewares
{
    // 3 ways to create middleware
    // 1.conventuin based
    // 2.factorybased
    // 3.or direct in program.cs
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger, IWebHostEnvironment env)
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


                // we already handle the not found using middleware app.UseStatusCodePagesWithReExecute("/Error/{0}"); in prgram , so we dont need this code
                //if (httpContext.Response.StatusCode == (int) HttpStatusCode.NotFound)
                //{
                //   var response = new ApiResponse((int)HttpStatusCode.NotFound,$"the request endpoint: {httpContext.Request.Path} is not found");
                //    await httpContext.Response.WriteAsync(response.ToString());
                //}

            }
            catch (Exception ex)
            {

                #region Logging : TO DO

                if (_env.IsDevelopment())
                {
                    // Development mode.

                    _logger.LogError(ex, ex.Message); // log it into the console, but it must be logged in the database or file.
                }

                else
                {
                    // Production mode.
                    // Log the exception Details in the database. | File (text,jason).

                } 
                #endregion


                await HnadleExceptionAsync(httpContext, ex);

            }
        }

        private async Task HnadleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            ApiResponse response;

            switch (ex)
            {
                case NotFoundException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    httpContext.Response.ContentType = "application/json";

                    response = new ApiResponse(404, ex.Message);

                    await httpContext.Response.WriteAsync(response.ToString());
                    break;


                case BadRequestException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";

                    response = new ApiResponse(404, ex.Message);

                    await httpContext.Response.WriteAsync(response.ToString());
                    break;



                default:
                    // here we handle the system exception, {null reference, divide by zero,....}

                    response = _env.IsDevelopment()
                        ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace?.ToString()) // stacktrace is the details of the exception
                         : 
                         new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);



                    // here we set the response status code and content type
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.ContentType = "application/json";

                    await httpContext.Response.WriteAsync(response.ToString());

                    break;
            }
        }
    }
}
