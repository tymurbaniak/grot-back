using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
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
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
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
