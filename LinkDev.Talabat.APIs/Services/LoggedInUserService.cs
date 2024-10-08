using LinkDev.Talabat.Core.Application.Abstaction;
using System.Security.Claims;

namespace LinkDev.Talabat.APIs.Services
{
    public class LoggedInUserService : ILoggedInUserService
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public string UserId  { get ;}


        public LoggedInUserService(/*IHttpContextAccessor? httpContextAccessor*/) // this inteface contain the http context to get the user id from the token
        {
            //_httpContextAccessor = httpContextAccessor;


            // This line retrieves the user ID from the token sent in the request's auth header.
            // The token contains the user ID as a claim, and the HttpContext.User property is populated with the user data.
            // Here, we are accessing the claim with the type NameIdentifier, which represents the user ID.
            UserId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;   // enta motalp lma t generste token , set  name identifier 3shan t2dr t3ml get ll user id
        }
    }
}
