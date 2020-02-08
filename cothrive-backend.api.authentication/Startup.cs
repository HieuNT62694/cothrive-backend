using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using cothrive_backend.api.authentication.Application.Authentication.Queries;
using cothrive_backend.api.authentication.Domain.Entities;
using cothrive_backend.api.authentication.Helpers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace cothrive_backend.api.authentication
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
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0.0-beta",
                    Title = "Cothrive Authentication API",
                    Description = "Cothrive Authentication API",
                    Contact = new OpenApiContact
                    {
                        Name = "Cothrive Developers",
                        Email = "developers@cothrive.xyz",
                        Url = new Uri("https://www.cothirve.tech")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under permission of Cothrive"
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using Bearer scheme. E.g: \"Authorization: Bearer + your jwt token\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
            // Add DatabaseContext with DB connection string
            var dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
            var dbPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
            var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var dbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB");
            string connectionString;

            if (string.IsNullOrEmpty(dbHost) || string.IsNullOrEmpty(dbPort) || string.IsNullOrEmpty(dbUser)
                || string.IsNullOrEmpty(dbPassword) || string.IsNullOrEmpty(dbName))
                connectionString = Configuration.GetConnectionString("DefaultConnection");
            else
                connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

            services.AddDbContext<cothriveContext>(options => options.UseNpgsql(connectionString));

            services.AddAutoMapper(typeof(Startup));


            //and scoped
            services.AddScoped<IAccountHelper, AccountHelper>();
            //MediatR and flutentvalidator
            services.AddMediatR(typeof(LoginRequest).Assembly);
            services.AddRouting(o => o.LowercaseUrls = true);

            //====== Add AddAuthentication =====
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(x =>
               {
                   x.RequireHttpsMetadata = false;
                   x.SaveToken = true;
                   x.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidIssuer = Configuration["JwtIssuer"],
                       ValidAudience = Configuration["JwtIssuer"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                       ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
               });
            services.AddAuthorization();
            services.AddCors(c => c.AddDefaultPolicy(p => { p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); }));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var basePath = Environment.GetEnvironmentVariable("API_PATH_BASE");
            var swaggerString = "/swagger/v1/swagger.json";

            if (!string.IsNullOrWhiteSpace(basePath))
            {
                app.UsePathBase($"/{basePath.TrimStart('/')}");
                swaggerString = basePath + swaggerString;
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swaggerString, "Cothrive Authentication API v1");
                c.DocumentTitle = "Cothrive Authentication API Docs";
                c.RoutePrefix = string.Empty;
            });


            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAllHeaders");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
