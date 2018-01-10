﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EFTraining.Data;
using EFTraining.Data.Seeders;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EFTraining
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            RunSeeder(host);

            host.Run();
        }

        private static void RunSeeder(IWebHost host)
        {
            // using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            // {

            //     var dbContext = serviceScope.ServiceProvider.GetService<DigiBookDbContext>();

            //     Seeder.Seed(dbContext);

            // }

            var serviceScopeFactory = (IServiceScopeFactory)host.Services.GetRequiredService(typeof(IServiceScopeFactory));

            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<DigiBookDbContext>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
                Seeder.Seed(dbContext, userManager, roleManager);
            }

        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
