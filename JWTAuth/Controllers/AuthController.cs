using JWTAuth.Models;
using JWTAuth.Business.AuthService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace JWTAuth.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthService authService;
        
        public AuthController(IAuthService AuthService)
        {
            authService = AuthService;
        }

        // POST: auth/login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUser user)
        {
            if (String.IsNullOrEmpty(user.UserName))
            {
                return BadRequest(new { message = "Email address needs to entered" });
            }
            else if (String.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { message = "Password needs to entered" });
            }

            //User loggedInUser = await authService.Login(user.UserName, user.Password);
            string loggedInUser = await authService.Login(user.UserName, user.Password);

            if (loggedInUser != null)
            {
                return Ok(loggedInUser);
            }

            return BadRequest(new { message = "User login unsuccessful" });
        }

        // POST: auth/register
        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser user)
        {
            if (String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.UserName) || String.IsNullOrEmpty(user.Password))
                return BadRequest(new { message = "Name needs to entered" });
            User userToRegister = new(user.UserName, user.Name, user.Password, user.Role);

            User registeredUser = await authService.Register(userToRegister);

            //User loggedInUser = await authService.Login(registeredUser.UserName, user.Password);
            string loggedInUser = await authService.Login(registeredUser.UserName, user.Password);

            if (loggedInUser != null) return Ok(loggedInUser);

            return BadRequest(new { message = "User registration unsuccessful" });
        }
    }
}
