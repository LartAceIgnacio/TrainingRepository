using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using BlastAsia.DigiBook.Infrastructure.Security;

namespace BlastAsia.DigiBook.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseStartup<Startup>()
            //    .UseApplicationInsights()
            //    .Build();

            //host.Run();

            var host = BuildWebHost(args);

            RunSeeder(host);

            host.Run();
        }

        private static void RunSeeder(IWebHost host)
        {
            var serviceScopeFactory = (IServiceScopeFactory)
                host.Services.GetRequiredService(typeof(IServiceScopeFactory));
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider
                    .GetService<DigiBookDbContext>();
                var userManager = serviceScope.ServiceProvider
                    .GetService<UserManager<ApplicationUser>>();
                var roleManager = serviceScope.ServiceProvider
                    .GetService<RoleManager<ApplicationRole>>();

                Seeder.Seed(dbContext, userManager, roleManager);
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
