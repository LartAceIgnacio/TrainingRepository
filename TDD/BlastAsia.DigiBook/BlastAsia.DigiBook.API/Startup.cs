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
using Microsoft.EntityFrameworkCore;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using BlastAsia.DigiBook.Domain.Employees.Services;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Appointments.Services;
using BlastAsia.DigiBook.Domain.Appointments.Adapters;
using BlastAsia.DigiBook.Domain.Venues.Service;
using BlastAsia.DigiBook.Domain.Venues;

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
            services.AddDbContext<DigiBookDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IDigiBookDbContext, DigiBookDbContext>();
            services.AddTransient<IContactService, ContactService>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<IVenueService, VenueService>();
            services.AddScoped<IVenueRepository, VenueRepository>();
            services.AddTransient<IDateTimeWrapper, DateTimeWrapper>();

            services.AddMvc();
            services.AddCors(config => {
                config.AddPolicy("PrimeNgDemoApp", policy =>
                {
                    policy.AllowAnyMethod();
                    policy.AllowAnyMethod();
                    policy.WithOrigins("http://localhost:4200");

                });
            });

            services.AddSwaggerGen(o => {
                o.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "DigiBook API" } );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseCors(config => {
            //    //config.AllowAnyHeader();
            //    //config.AllowAnyMethod();
            //    //config.WithOrigins("http:\\localhost:4200");


            //} );

            app.UseCors("PrimeNgDemoApp");
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json","DigiBook Api 1");
            });

            app.UseMvc();
        }
    }
}
