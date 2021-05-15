using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using UserManagement.DBContexts;
using UserManagement.Services;
using UserManagement.Settings;

namespace UserManagement
{
    public static class AuthStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            string mySqlConnectionStr = GetConnectionString(configuration);
            services.AddDbContextPool<UsersDBContext>(options => options.UseMySql(mySqlConnectionStr));

            // configure strongly typed settings objects
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthorization();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };

                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/hub"))) //move to config
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHCaptchaTokenService, HCaptchaTokenService>();

            var hCaptchaSettingsSection = configuration.GetSection("HCaptcha");
            services.Configure<HCaptchaSettings>(hCaptchaSettingsSection);
            var hCaptchaSettings = hCaptchaSettingsSection.Get<HCaptchaSettings>();
            services.AddHttpClient("hCaptcha", client =>
            {
                client.BaseAddress = new Uri(hCaptchaSettings.Api);
            });
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            connectionString = string.IsNullOrEmpty(connectionString) ? configuration["CONNECTION_STRING"] : connectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ApplicationException("No database connection string");
            }

            return connectionString;
        }
    }
}
