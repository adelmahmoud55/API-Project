using LinkDev.Talabat.Core.Application.Abstaction.Models.Auth;
using LinkDev.Talabat.Core.Application.Abstaction.Services;
using LinkDev_Talabat.APIs.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Account
{
    public class AccountController(IServiceManager serviceManager ) : ApiControllerBase

    {

        [HttpPost("login")] // Post: api/Account/register
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var result = await serviceManager.AuthService.LoginAsync(model);
            return Ok(result);
        }


        [HttpPost("register")] // Post: api/Account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var result = await serviceManager.AuthService.RegisterAsync(model);
            return Ok(result);
        }



        [Authorize]
        [HttpGet] // Get: api/Account 
        public async Task<ActionResult<UserDto>> GetCurrentUser()   // for frontend
        {
            var result = await serviceManager.AuthService.GetCurrentUser(User);
            return Ok(result);
        }   
    }
}

