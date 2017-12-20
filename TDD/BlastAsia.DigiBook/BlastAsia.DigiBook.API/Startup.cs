using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Appointments;

namespace BlastAsia.DigiBook.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DigiBookDbContext>(
                 option => option.UseSqlServer
                 (
                     Configuration.GetConnectionString("DefaultConnection")
                 )
             );
            // Add framework services.
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info { Title = "DigiBook API" });
            });

            services.AddScoped<IDigiBookDbContext, DigiBookDbContext>();

            services.AddTransient<IContactService, ContactService>();
            services.AddScoped<IContactRepository, ContactRepositories>();

            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository,EmployeeRepository>();

            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "DigiBook Api V1");
            });
            app.UseMvc();
        }
    }
}
