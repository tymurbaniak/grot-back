using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrotWebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    Console.WriteLine("======================================================");
                    Console.WriteLine("ADDED CUSTOM JSON");
                    Console.WriteLine("======================================================");
                    configurationBuilder.AddJsonFile($"appsettings.{environment}.json", optional: false);                    
                });
    }
}
