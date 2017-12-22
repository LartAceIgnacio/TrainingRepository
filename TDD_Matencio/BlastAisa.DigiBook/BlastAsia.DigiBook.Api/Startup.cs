using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain;

namespace BlastAsia.DigiBook.Api
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
                options => options.UseSqlServer
                (Configuration.GetConnectionString("DefaultConnection"))
                );
            //services.AddScoped<IContact>

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info { Title = "DigiBook Api" });
            });

            //Contacts
            services.AddScoped<IDigiBookDbContext, DigiBookDbContext>();
            services.AddTransient<IContactService,ContactService>();
            services.AddScoped<IContactRepository,ContactRepository>();

            //Employees
            services.AddTransient<IEmpolyeeService, EmpolyeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            //Appointments
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            //Venues
            services.AddTransient<IVenueService, VenueService>();
            services.AddScoped<IVenueRepository, VenueRepository>();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DigiBook Api v1");
            });

            app.UseMvc();
        }
    }
}
