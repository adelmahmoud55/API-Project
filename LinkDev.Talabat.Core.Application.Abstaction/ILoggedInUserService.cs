using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstaction
{
    public interface ILoggedInUserService
    {
        string? UserId { get; }
    }
}
