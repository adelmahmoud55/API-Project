using LinkDev.Talabat.Core.Application;
using LinkDev.Talabat.APIs.Extensions;
using LinkDev.Talabat.APIs.Services;
using LinkDev.Talabat.Core.Application.Abstaction;
using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Infrastructure.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.APIs.Middlewares;

namespace LinkDev.Talabat.APIs
{
    public class Program
    {
        // Entry point for the application.
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);


            #region Configure Services

            // Add services to the container.

            //AddApplicationPart is used to add the controllers to the DI container. and to tell that the controllers are in the same assembly as the AssemblyInformation class.
            webApplicationBuilder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options => // this handling the validation errors in the model binding across the application.
                {
                    options.SuppressModelStateInvalidFilter = false; // to disable the default behavior of returning 400 bad request when the model is invalid.
                    options.InvalidModelStateResponseFactory = actioncontext =>
                    {
                        var errors = actioncontext.ModelState //modelState is a dictionary of key value pairs , key is the name of the property and value is the error message, contains all the properties for the endpoint that has been called.
                            .Where(e => e.Value!.Errors.Count > 0) // get all the properties that have errors.
                            .SelectMany(x => x.Value!.Errors) // get all the errors for each property.
                            .Select(x => x.ErrorMessage); // get the error message.

                        var errorResponse = new ApiValidationErrorResponse("Validation Error") 
                        {
                            Errors = errors
                        };

                        return new BadRequestObjectResult(errorResponse); //we cannot use hellepr method like BadRequest() or NotFound() because we are not in the controller. there is no [ApiController] attribute.
                    };
                })
                .AddApplicationPart(typeof(LinkDev.Talabat.APIs.Controllers.AssemblyInformation).Assembly); // Register Controllers To DI Container.
                                                                                                           // Register Required Services By ASP.NET Core Web APIs To Di Container.

                
           ///webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options =>
           ///{
           ///    options.SuppressModelStateInvalidFilter = false;
           ///    options.InvalidModelStateResponseFactory = actionContext =>
           ///    {
           ///        var errors = actionContext.ModelState
           ///            .Where(e => e.Value.Errors.Count > 0)
           ///            .SelectMany(x => x.Value.Errors)
           ///            .Select(x => x.ErrorMessage).ToArray();
           ///
           ///
           ///
           ///        var errorResponse = new ApiValidationErrorResponse("Validation Error") 
           ///        {
           ///            Errors = errors
           ///        };
           ///        return new BadRequestObjectResult(errorResponse);
           ///    };
           ///
           ///});    
           

           /// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();


            webApplicationBuilder.Services.AddPersistenceServices(webApplicationBuilder.Configuration);
            //DependencyInjection.AddPersistenceServices(webApplicationBuilder.Services, webApplicationBuilder.Configuration); // Register Persistence Services To DI Container.

            webApplicationBuilder.Services.AddApplicationServices(); // Register Application Services To DI Container.

            //webApplicationBuilder.Services.AddScoped(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
            webApplicationBuilder.Services.AddHttpContextAccessor(); // IHttpContextAccessor depend on another service , so u have to use this method , not only register the IHttpContextAccessor as above 

            webApplicationBuilder.Services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService)); // Register LoggedInUserService To DI Container.

            #endregion

            var app = webApplicationBuilder.Build();

            #region Update DataBase and Data Seeding
           
           await  app.InitializerStoreContextAsync();
           
            #endregion

            #region Configure Kestrel Middlewares

            app.UseMiddleware<CustomExceptionHandlerMiddleware>(); 

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                //app.UseDeveloperExceptionPage(); this used internally from .net 6 , so no need to add it. means return the exception as a response.
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization(); 

            app.UseStaticFiles(); // to allow kestrel to serve the requests that ask for any static file like from wwwroot.
                                  // enable static file serving for the current request path {current : wwwroot path}

            app.MapControllers();

            #endregion




            app.Run();
        }
    }
}
