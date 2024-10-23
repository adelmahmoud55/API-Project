using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Identity
{
    public class Address /*: BaseEntity<int>*/ // we will not use this cuz we will make a new seperate database seprate from store Context
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Street { get; set; }

        public required string City { get; set; }

        public required string Country { get; set; }

        // u have to make it unique , 3shan kol address yb2a relate to one user ,cuz by default it will be nullable 3shan ana 3aml el prop 3nd user compsition "optional"
        public required string UserId { get; set; } // this must be string cuz in the ApplicationUser we inherit from IdentityUser which inherit from IdentityUser<string> so the primary key of user is string

        public virtual required ApplicationUser User { get; set; }
    }
}

