using Grot.Hubs;
using Grot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Grot
{
    public static class GrotStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IParametersService, ParametersService>();
            services.AddScoped<IProcessService, ProcessService>();
            services.AddScoped<IProjectsService, ProjectsService>();
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<GrotHub>("/hub");
            });

            char separator = Path.DirectorySeparatorChar;
            string projectsPath = $"{env.ContentRootPath}{separator}projects";

            if (!Directory.Exists(projectsPath))
            {
                Directory.CreateDirectory(projectsPath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "projects")),
                RequestPath = "/projects"                
            });
        }
    }
}
