using CleanArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace CleanArchitecture.Web
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = 
                    WebHost
                        .CreateDefaultBuilder(args)
                        .UseStartup<Startup>()
                        .Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<AppDbContext>();

                    if (context.Database.IsSqlite())
                    {
                        context.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                    throw;
                }

                await host.RunAsync();
            }
        }
    }
}
