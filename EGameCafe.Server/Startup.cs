using System.Collections.Generic;
using System.Linq;
using EGameCafe.Application;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Infrastructure;
using EGameCafe.Server.Common.Options;
using EGameCafe.Server.Filters;
using EGameCafe.Server.Hubs;
using EGameCafe.Server.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace EGameCafe.Server
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
            services.AddApplication();

            services.AddInfrastructure(Configuration);

            //services.AddScoped<IIdentityService, IdentityService>();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddHttpContextAccessor();

            //services.AddHealthChecks()
            //    .AddDbContextCheck<ApplicationDbContext>();


            services.AddApiVersioning(o=>{
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddControllersWithViews(options =>
                options.Filters.Add(new ApiExceptionFilter()))
                    .AddFluentValidation();

            services.AddRazorPages();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSignalR();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "EGameCafe API",
                    Version = "v1",
                    Description= "API V1 description"
                });

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });


                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {new OpenApiSecurityScheme{Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }}, new List<string>()}
                });

                x.ResolveConflictingActions(a=>a.First());
                x.OperationFilter<RemoveVersionFromParameter>();
                x.DocumentFilter<ReplaceVersionWithExactValueInPath>();

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseCustomExceptionHandler();
            //app.UseHealthChecks("/health");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseOpenApi();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            var swaggerOptions = new SwaggerOption();
            Configuration.GetSection(nameof(SwaggerOption)).Bind(swaggerOptions);

            app.UseSwagger(option => 
            { 
                option.RouteTemplate = swaggerOptions.JsonRoute;
            });
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
}
