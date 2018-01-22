﻿using Microsoft.AspNetCore.Builder;
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
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Models;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlastAsia.DigiBook.Infrastructure.Security;
using BlastAsia.DigiBook.Domain.Luigis;

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
            services.AddEntityFrameworkSqlServer();

            services.AddDbContext<DigiBookDbContext>(
                   options => options.UseSqlServer(Configuration
                   .GetConnectionString("DefaultConnection"))
                   );

            services.AddScoped<IDigiBookDbContext, DigiBookDbContext>();

            services.AddTransient<IContactService, ContactService>();
            services.AddScoped<IContactRepository, ContactRepository>();

            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            services.AddTransient<IVenueService, VenueService>();
            services.AddScoped<IVenueRepository, VenueRepository>();

            services.AddTransient<ILuigiService, LuigiService>();
            services.AddScoped<ILuigiRepository, LuigiRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info { Title = "DigiBook API" });
            });

            // Add framework services.
            

            services.AddCors(config => {
                config.AddPolicy("DemoApp", policy =>
                {
                    policy.AllowAnyMethod();
                    policy.WithOrigins("http://localhost:4200");

                });
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Signin settings
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<DigiBookDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "DigBookCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
                // Requires `using Microsoft.AspNetCore.Authentication.Cookies;`
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });
            #region Authentication
            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        // standard configuration
                        ValidIssuer = Configuration["Auth:Jwt:Issuer"],
                        ValidAudience = Configuration["Auth:Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero,
                        // security switches
                        RequireExpirationTime = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true
                    };
                });
            #endregion
            services.AddMvc();
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
                    "DigiBook Api v1");
            });
            
            app.UseCors("DemoApp");

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
