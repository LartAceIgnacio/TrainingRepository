﻿using BlastAsia.DigiBook.Domain;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Appointments.Adapters;
using BlastAsia.DigiBook.Domain.Appointments.Services;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Employees.Services;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Venues.Service;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using BlastAsia.DigiBook.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

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
            services.AddEntityFrameworkSqlServer();

            services.AddDbContext<DigiBookDbContext>(
                   options => options.UseSqlServer(Configuration
                   .GetConnectionString("DefaultConnection"))
                   );


            #region Security
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(options =>
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
                options.Cookie.Name = "DigiBookCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
                // Requires `using Microsoft.AspNetCore.Authentication.Cookies;`
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });
            #endregion

            #region Authentication
            services
                .AddAuthentication(opts =>
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

            #region DI
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


            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped(it => it.GetService<IUrlHelperFactory>()
                    .GetUrlHelper(it.GetService<IActionContextAccessor>().ActionContext));


            #endregion

            #region Swagger
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "DigiBook API" });
            });

            #endregion

            #region Cors
            services.AddCors(config =>
            {
                config.AddPolicy("PrimeNgDemoApp", policy =>
                {
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                    policy.AllowAnyOrigin();
                    policy.AllowCredentials();
                    //policy.WithOrigins("http://localhost:4200", "http://localhost:4200/");

                });
            });
            #endregion

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("PrimeNgDemoApp");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DigiBook Api v1");
            });

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
