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

        public int UserId { get; set; }

        public required ApplicationUser AppUser { get; set; }
    }
}

