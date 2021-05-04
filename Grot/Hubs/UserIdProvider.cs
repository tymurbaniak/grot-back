using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using UserManagement.Services;

namespace Grot.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public UserIdProvider(
            IHttpContextAccessor httpContextAccessor,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.serviceScopeFactory = serviceScopeFactory;
        }
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.Claims.First(x => x.Type.Equals(ClaimTypes.Name)).Value;

            //string userName = string.Empty;

            //using (var scope = this.serviceScopeFactory.CreateScope())
            //{
            //    IUserService service = scope.ServiceProvider.GetService<IUserService>();
            //    IHttpContextAccessor http = scope.ServiceProvider.GetService<IHttpContextAccessor>();

            //    string token = http.HttpContext.Request.Query["access_token"];
            //    var user = service.GetUserByToken(token);
            //    userName = user.Name;
            //}

            //return userName;
        }
    }
}
