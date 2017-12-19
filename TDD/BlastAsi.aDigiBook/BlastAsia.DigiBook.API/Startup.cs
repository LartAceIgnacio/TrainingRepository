using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace BlastAsia.DigiBook.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DigiBookDbContext>(
                  options => options.UseSqlServer(Configuration
                  .GetConnectionString("DefaultConnection"))
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
            services.AddScoped<IContactRepository, ContactRepository>();


            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
           {
               c.SwaggerEndpoint("/swagger/v1/swagger.json",
                   "DigiBook Api v1");
           });

            app.UseMvc();
        }
    }
}
