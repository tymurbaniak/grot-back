using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.ViewModels;
using UserManagement.Services;
using BenjaminAbt.HCaptcha;

namespace GrotWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService)
        {
            this._userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("newuser")]
        public IActionResult NewUser([FromBody] RegisterRequest model)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            bool isProduction = environment.Equals("Production");

            if (isProduction && string.IsNullOrEmpty(model.CaptchaToken))
            {
                return BadRequest(new { message = "Bot are not allowed to create accounts" });
            }

            var response = _userService.Register(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}
