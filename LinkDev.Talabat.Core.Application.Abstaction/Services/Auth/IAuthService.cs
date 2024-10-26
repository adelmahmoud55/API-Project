using LinkDev.Talabat.Core.Application.Abstaction.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstaction.Services.Auth
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(LoginDto model);

        Task<UserDto> RegisterAsync(RegisterDto model);
    }
}
