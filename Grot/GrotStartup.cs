using Grot.Hubs;
using Grot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Grot
{
    public static class GrotStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IParametersService, ParametersService>();
            services.AddScoped<IProcessService, ProcessService>();
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<GrotHub>("/hub");
            });
        }
    }
}
