using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev_Talabat.APIs.Controllers.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")] // This attribute is used to enable various features for API controllers, such as automatic model validation and binding, automatic HTTP 400 responses for invalid models, and more.
    public class ApiControllerBase : ControllerBase
    {

    }
}
