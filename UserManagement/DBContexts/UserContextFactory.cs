using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UserManagement.DBContexts
{
    public class UserContextFactory : IDesignTimeDbContextFactory<UsersDBContext>
    {
        public UsersDBContext CreateDbContext(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configBuilder = new ConfigurationBuilder();
            configBuilder.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../GrotWebApi"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables();

            bool isRuningInContainer = bool.Parse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"));

            if (isRuningInContainer)
            {
                configBuilder.AddJsonFile($"appsettings.Docker.json", optional: false, reloadOnChange: true);
            }

            IConfiguration configuration = configBuilder.Build();
            string mySqlConnectionStr = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(mySqlConnectionStr))
            {
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.WriteLine();
                Console.WriteLine("NO CONNECTION STRING FROM ENVIRONMENT ");
                Console.WriteLine();
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }

            var connection = new MySqlConnection(mySqlConnectionStr);

            try
            {
                connection.Open();
                bool ping = connection.Ping();

                if (ping)
                {
                    Console.WriteLine("CONNECTION OK");
                }
                else
                {
                    Console.WriteLine("DATABASE NOT RESPONDING");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"CAN'T CONNECT TO DATABASE WITH CONNECTION STRING: {connection.ConnectionString}");
                Console.WriteLine(e.Message);
                connection.Close();
            }

            var optionsBuilder = new DbContextOptionsBuilder<UsersDBContext>();
            optionsBuilder.UseMySql(mySqlConnectionStr);

            return new UsersDBContext(optionsBuilder.Options);
        }
    }
}
