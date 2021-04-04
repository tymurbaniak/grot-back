using Grot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Grot
{
    public static class GrotStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IParametersService, ParametersService>();
            services.AddScoped<IProcessService, ProcessService>();
        }
    }
}
