using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.ViewModels;
using UserManagement.Services;
using BenjaminAbt.HCaptcha;
using System.Net.Http;
using UserManagement.Settings;
using Microsoft.Extensions.Options;

namespace GrotWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IHCaptchaTokenService captchaService;
        private readonly bool isDev = false;

        public RegisterController(
            IUserService userService,
            IHCaptchaTokenService captchaService
            )
        {
            this.userService = userService;
            this.captchaService = captchaService;
            isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development");
        }

        [AllowAnonymous]
        [HttpPost("newuser")]
        public IActionResult NewUser([FromBody] RegisterRequest model)
        {            
            if (!isDev)
            {
                if (string.IsNullOrEmpty(model.CaptchaToken))
                {
                    return BadRequest(new { message = "Bots are not allowed to create accounts" });
                }

                var hCaptchaVerification = captchaService.IsCaptchaValid(model.CaptchaToken);

                if (!hCaptchaVerification)
                {
                    return BadRequest(new { message = "Bots are not allowed to create accounts" });
                }
            }


            var response = userService.Register(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}
