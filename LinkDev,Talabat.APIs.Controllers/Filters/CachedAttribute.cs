using LinkDev.Talabat.Core.Application.Abstaction.Comman.Contracts.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.APIs.Controllers.Filters
{
    internal class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }

        public async Task  OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var responseCachService = context.HttpContext.RequestServices.GetRequiredService<IResponseCachService>();

            var cachKey = GenerateCachKeyFromRequest(context.HttpContext.Request);

            var response = await responseCachService.GetCachedResponseAsync(cachKey);

            if (!string.IsNullOrEmpty(response)) // Resonse is already Cached
            {
               var  Result = new ContentResult()
               {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200

               };
               
                context.Result = Result;

                return;
            }

          var excutedActionContext =   await next.Invoke(); //Excute the endpoint ==> Call the next action in the pipeline 

            if (excutedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
            { 
              await responseCachService.CachResponseAsync(cachKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        
        }

        private string GenerateCachKeyFromRequest(HttpRequest request)
        {
            // {{baseUrl}}/api/Products?pageIndex=1&pageSize=5&sort=name

            var keyBuilder = new StringBuilder();

            keyBuilder.Append(request.Path); // /api/Products


            //all are dictionary of key value pairs
            // pageIndex = 1
            // pageSize  = 5
            // sort      = name

            foreach(var (key, value) in request.Query.OrderBy(q => q.Key)) // OrderBy to make sure the key is always the same in case 8yrt f trteeb el query string 3shan myb2ash 3ndy key mo5tlf w el response hwa hwa f a3ml cach l7gat mwgoda aslun 
            {
                keyBuilder.Append($"|{key}-{value}"); // /api/Products?pageIndex-1&pageSize-5&sort-name
            }

            return keyBuilder.ToString(); // return the cach key as string, it will be used to store the response in the cach service

        }
    }
}
