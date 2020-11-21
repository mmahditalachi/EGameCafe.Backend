using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EGameCafe.Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;


namespace EGameCafe.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            var UseInMemoryDatabase = bool.Parse(builder.GetSection("UseInMemoryDatabase").Value);

            if (UseInMemoryDatabase)
            {
                using (var scope = host.Services.CreateScope())
                {
                    var userManager = scope.ServiceProvider
                        .GetRequiredService<UserManager<ApplicationUser>>();

                    var user = new ApplicationUser()
                    {
                        Email = "test@test.com",
                        FirstName = "mohammad",
                        LastName = "Talachi",
                        UserName = "test_test",
                        PhoneNumber = "0933333333",
                        BirthDate = new DateTime(1999, 11, 24)
                    };

                    userManager.CreateAsync(user, "password").GetAwaiter().GetResult();

                    var token = userManager.GenerateEmailConfirmationTokenAsync(user).GetAwaiter().GetResult();

                    userManager.ConfirmEmailAsync(user, token).GetAwaiter();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                    // Enable NLog as one of the Logging Provider
                    logging.AddNLog();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
