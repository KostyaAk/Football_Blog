using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using football_blog.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace football_blog
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File("Logs\\AllLogs.txt")
                .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Error)
                .WriteTo.File("Logs\\ErrorLogs.txt"))
                .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Error)
                .WriteTo.Console())
                .CreateLogger();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    //await CustomIdentityApp.RoleInitializer.InitializeAsync(userManager, rolesManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        })
            .UseSerilog();
    }
}
