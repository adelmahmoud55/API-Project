using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstaction.Models.Auth
{
    public class RegisterDto
    {
        [Required]
        public required string DisplayName { get; set; }

        [Required]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PhoneNumber { get; set; }

        [Required] // best practice ti use the reugular expression for the password validation, rather than in the AddIdentity method
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#%^&amp;()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
                 ErrorMessage = "Password must have 1 UpperCase,1 LowerCase,1 number , 1 non alphanumberic and at least 6 characters ")]
        public required string Password { get; set; }

        //public required string ConfirmPassword { get; set;  // no need for this property , just used for the client side validation, in backend we need the password only
    }
}
