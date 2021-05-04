using Grot.Hubs;
using Grot.Services;
using Grot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services;

namespace GrotWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GrotController : ControllerBase
    {
        private readonly IParametersService parametersService;
        private readonly IProcessService processService;
        private readonly IUserService userService;
        private readonly IHubContext<GrotHub> grotHub;
        public GrotController(
            IParametersService parametersService, 
            IProcessService processService,
            IUserService userService,
            IHubContext<GrotHub> grotHub)
        {
            this.parametersService = parametersService;
            this.processService = processService;
            this.userService = userService;
            this.grotHub = grotHub;
        }

        [AllowAnonymous]
        [HttpPost("process")]
        public IActionResult Process([FromBody] ProcessCall processCall)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = this.userService.GetUserByToken(refreshToken);

            if(user == null)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            this.processService.RequestProcess(processCall.InputParameters, processCall.Image, user);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("parameters")]
        public IActionResult Parameters()
        {
            var result = this.parametersService.GetParameters();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("signalrtest")]
        public IActionResult SignalRTest()
        {
            grotHub.Clients.All.SendAsync("receive", "SendAsyncTestMessage");
            grotHub.Clients.All.SendCoreAsync("receive", new string[] { "SendAsyncCoreMessage" });
            return Ok();
        }
    }
}
