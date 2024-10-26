using LinkDev.Talabat.Core.Application.Abstaction.Models.Auth;
using LinkDev.Talabat.Core.Application.Abstaction.Services.Auth;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services.Auth
{
    // we cannot reach the singin manager service here in core, and we will not use it cuz it use the dotnet way 
    //when i sign in it will generate a token and send it to the client and save it in the cookies storage , but we will use jwt(JSON Web Token) package and we will find the sigin manager
    internal class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IAuthService
    {

        public async Task<UserDto> LoginAsync(LoginDto model)
        {

            var user = await userManager.FindByEmailAsync(model.Email);

            //steps for  user authentication

            // incase email is not found or the user is null
            if (user is null) throw new UnAuthorizedException("Invalid Login.");



            // to check if the user is locked out or not
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);

            if (result.IsNotAllowed) throw new UnAuthorizedException("Account not confirmed yet.");

            if (result.IsLockedOut) throw new UnAuthorizedException("Account is locked.");
            //if (!result.RequiresTwoFactor) throw new UnAuthorizedException("Requires Two Factor Authentication."); // it will be handled in the client side
            #region Two Factor authentication

            /*
             
           Implementing Two-Factor Authentication (2FA) on the client side provides a better user experience and improves security. Here are a few reasons why 2FA is typically handled on the client side:
           1.	User Interaction: 2FA often involves sending a verification code to the user's mobile device or email. By handling 2FA on the client side, the user can directly interact with their device to receive and enter the verification code. This allows for a smoother and more seamless user experience.
           2.	Security: Handling 2FA on the client side ensures that the verification code is sent directly to the user's device, reducing the risk of interception or tampering. It adds an extra layer of security by requiring something the user has (their device) in addition to something they know (their password).
           3.	Flexibility: By implementing 2FA on the client side, you have more flexibility in choosing the authentication methods. You can leverage various methods such as SMS, email, authenticator apps, or hardware tokens. This allows users to choose the method that best suits their preferences and security needs.
           4.	Scalability: Handling 2FA on the client side allows for easier scaling of the authentication system. The server does not need to generate and manage verification codes for each user, as the client handles this responsibility. This can reduce the load on the server and improve performance.
           It's important to note that while 2FA can be handled on the client side, the server should still enforce the requirement for 2FA and validate the verification code received from the client. This ensures that the server has the final say in granting access to protected resources.
           By implementing 2FA on the client side, you can provide a more user-friendly and secure authentication process for your application.
             
             */


            #endregion


            if (!result.Succeeded) throw new UnauthorizedAccessException("Invalid Login.");

            #region Authentication 


            /*
             the above cheks for user authentication , not authorization. Authentication is the process of verifying the identity of a user, while authorization determines what actions a user is allowed to perform.
              In this code, the selected portion is performing authentication checks for a user attempting to log in. Let's break it down step by step:
              1.	The code first checks if the user object is null. If it is null, it means that the user with the provided email does not exist, so an UnAuthorizedException is thrown with the message "Invalid Login." This check ensures that only existing users can attempt to log in.
              2.	Next, the code calls the CheckPasswordSignInAsync method of the signInManager to verify the user's password. This method checks if the provided password matches the password stored for the user in the database. It also checks if the user is locked out due to multiple failed login attempts. The lockoutOnFailure parameter is set to true, which means that if the user is locked out, the method will increment the failed login attempt count and lock the account if the maximum number of attempts is reached.
              3.	If the IsNotAllowed property of the result is true, it means that the user's account has not been confirmed yet. In this case, an UnAuthorizedException is thrown with the message "Account not confirmed yet." This check ensures that only confirmed users can log in.
              4.	If the IsLockedOut property of the result is true, it means that the user's account is locked due to too many failed login attempts. In this case, an UnAuthorizedException is thrown with the message "Account is locked." This check prevents locked accounts from logging in.
              5.	Finally, if the Succeeded property of the result is false, it means that the password verification failed. This could happen if the provided password is incorrect. In this case, an UnauthorizedAccessException is thrown with the message "Invalid Login." This check ensures that only users with the correct password can log in.
              To summarize, this code performs various checks to ensure that the user attempting to log in is valid and authorized to access the system. It verifies the user's existence, checks if the account is confirmed, checks if the account is locked, and verifies the password. If any of these checks fail, an exception is thrown to indicate that the login attempt is invalid.
             
             
             
             
             
             
             
             
             
             */
            #endregion

            // this will return the user with the token to the client after the user is authenticated 
            var response =  new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = "Token" // this for authrization
            };


            #region Token Jwt

            /*
             Typically, in a web application, after a user successfully logs in, the server generates a token (such as a JSON Web Token or JWT) that represents the user's identity and permissions. This token is then sent back to the client as part of the response. The client can store this token in various ways, such as in a cookie, local storage, or session storage.
              Storing the token in a cookie is a common approach. The client can include the token in subsequent requests by automatically attaching it to the request headers or by explicitly adding it to the request when needed. The server can then validate the token to ensure that the client has the necessary authorization to access protected resources.
             
             */
            #endregion

            return response;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser() // here id will be set to guit when it chain to the base class
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
            };


            //user will be created and saved in the database, usermanger act as repository
            var result = await userManager.CreateAsync(user, model.Password);


            // this to check if the user is created or not,and meet the requirements of the password, and the email is unique .... or not
            if (!result.Succeeded) throw new ValidationException() { Errors =  result.Errors.Select(E => E.Description) };


            // this will return to the client wiht the token
            var response = new UserDto 
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = "Token"
            };

            return response;



        }
    }
}