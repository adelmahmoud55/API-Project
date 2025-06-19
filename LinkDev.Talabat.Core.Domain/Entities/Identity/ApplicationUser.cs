using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Entities.Identity
{
    // here we custmize on one of the 7 Identitties "IdentityUser"
    public class ApplicationUser : IdentityUser  //The default implementation of Microsoft.AspNetCore.Identity.IdentityUser`1 which
                                                 //     uses a string as a primary key. so if we inherit from non generic IdentityUser its the same cuz IdentityUser inherit from IdentityUser<string>
    {
        public required string DisplayName { get; set; }

        public virtual Address? Address { get; set; }
    }
}
